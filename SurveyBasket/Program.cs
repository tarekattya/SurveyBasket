
using FluentValidation;
using Hangfire;
using Hangfire.Dashboard;
using HangfireBasicAuthenticationFilter;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Serilog;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using SurveyBasket.Presistence.DbContextt;


namespace SurveyBasket
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.Addservices(builder.Configuration);
            //builder.Services.AddIdentityApiEndpoints<ApplicationUser>().
            // AddEntityFrameworkStores<ApplicationDbContext>();
            //builder.Services.AddOutputCache(options =>
            //{
            //    options.AddPolicy("Polls", x => x.Cache().Expire(TimeSpan.FromSeconds(120)).Tag("AvaliableQuestion")
            //    );




            builder.Host.UseSerilog((context, configuration) =>
            {
                configuration.ReadFrom.Configuration(context.Configuration)
                    .WriteTo.Console();
            });



            var app = builder.Build();
            using (var scopee = app.Services.CreateScope())
            {
                await SeedDefaultUserAsync(scopee.ServiceProvider);
            }


            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwaggerUI(op => op.SwaggerEndpoint("/openapi/v1.json", ""));
                app.MapScalarApiReference(opt =>
                {
                    opt.Title = "Scalar Example";
                    opt.Theme = ScalarTheme.Mars;
                    opt.DefaultHttpClient = new(ScalarTarget.Http, ScalarClient.Http11);
                });

            }

            app.UseSerilogRequestLogging();
            app.UseHangfireDashboard("/jobs", new DashboardOptions
            {

                Authorization = [

                    new HangfireCustomBasicAuthenticationFilter{

                        User = app.Configuration.GetValue<string>("HangfireSettings:UserName"),
                        Pass = app.Configuration.GetValue<string>("HangfireSettings:Password")
                    }],
                DashboardTitle = "TatuuJobs",
                IsReadOnlyFunc = (DashboardContext context) => true,


            });

            
            app.UseHttpsRedirection();

             var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var NotifacationServices = scope.ServiceProvider.GetRequiredService<INotifacitionServices>();

            RecurringJob.AddOrUpdate("SendNotificationAsync",() => NotifacationServices.SendNotificationAsync(null), Cron.Daily);

            app.UseCors();
            app.UseAuthorization();


            //app.MapIdentityApi<ApplicationUser>();

            app.MapControllers();
            app.UseExceptionHandler();

            app.Run();
        }


        static async Task SeedDefaultUserAsync(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            var email = "admin@survey.com";
            var password = "Admin@123";
            var roleName = "Admin";
            var FirstName = "Admin";
            var LastName = "Admin";


            if (await roleManager.FindByNameAsync(roleName) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }

            if (await userManager.FindByEmailAsync(email) == null)
            {
                var user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true,
                    LastName = LastName,
                    FirstName = FirstName

                };

                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, roleName);
                }
            }
        }



    }
}