
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





    }
}