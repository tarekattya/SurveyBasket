using MapsterMapper;
using SurveyBasket.Repositories.IDEMO;
using SurveyBasket.Repositories;
using System.Reflection;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using FluentValidation;

namespace SurveyBasket
{
    public static class DependancyInjection 
    {

        public static IServiceCollection Addservices(this IServiceCollection services)
        {
            services.AddScoped<IPollRepo, PollRepo>();
            services.AddScoped<PollServices>();

            services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            services.AddOpenApi();


            services.AddSMapsterservices();
            services.AddSFluentValidationservices();

            return services;
        }

        public static IServiceCollection AddSMapsterservices(this IServiceCollection services)
        {
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(Assembly.GetExecutingAssembly());
            services.AddSingleton<IMapper>(new Mapper(config));

            return services;
        }


        public static IServiceCollection AddSFluentValidationservices(this IServiceCollection services)
        {
            services
             .AddFluentValidationAutoValidation()
             .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }

    }
}
