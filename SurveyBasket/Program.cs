
using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
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
            var app = builder.Build();
            using (var scope = app.Services.CreateScope())
            {
                await SeedDefaultUserAsync(scope.ServiceProvider);
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

            app.UseHttpsRedirection();
            app.UseCors();
            app.UseAuthorization();
            //app.MapIdentityApi<ApplicationUser>();

            app.MapControllers();

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