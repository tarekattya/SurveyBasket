
using FluentValidation;
using Mapster;
using MapsterMapper;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using SurveyBasket.Contracts.Requests;
using SurveyBasket.Contracts.Validation.CreatePoll;
using SurveyBasket.Repositories;
using SurveyBasket.Repositories.IDEMO;
using SurveyBasket.Services;
using System.Reflection;

namespace SurveyBasket
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.



            builder.Services.Addservices();

          
           
            
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwaggerUI(op => op.SwaggerEndpoint("/openapi/v1.json",""));
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
             

            app.MapControllers();

            app.Run();
        }
    }
}
