
using Mapster;
using MapsterMapper;
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

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();


            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(Assembly.GetExecutingAssembly());
            builder.Services.AddSingleton<IMapper>(new Mapper(config));
            builder.Services.AddScoped<IPollRepo, PollRepo>();
            builder.Services.AddScoped<PollServices>();
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
