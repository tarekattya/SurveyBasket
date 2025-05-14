
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
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.Addservices(builder.Configuration);
            //builder.Services.AddIdentityApiEndpoints<ApplicationUser>().
            //   AddEntityFrameworkStores<ApplicationDbContext>();
            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwaggerUI(op => op.SwaggerEndpoint("/openapi/v1.json",""));
                app.MapScalarApiReference(opt =>
                {
                    opt.Title = "Scalar Example";
                    opt.Theme = ScalarTheme.Mars;
                    opt.DefaultHttpClient = new(ScalarTarget.Http, ScalarClient.Http11);
                });

            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            //app.MapIdentityApi<ApplicationUser>();

            app.MapControllers();

            app.Run();
        }
    }
}
