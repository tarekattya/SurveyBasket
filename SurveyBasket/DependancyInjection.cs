using MapsterMapper;
using System.Reflection;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using SurveyBasket.Presistence.DbContextt;
using SurveyBasket.Services.NewFolder;


namespace SurveyBasket
{
    public static class DependancyInjection 
    {

        public static IServiceCollection Addservices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IPollServices, PollServices>();

            

            services.AddControllers();
            services.AddOpenApi();


            services.AddSMapsterservices();
            services.AddSFluentValidationservices();
            services.AddDataBaseCon(configuration);

            return services;
        }

        public static IServiceCollection AddDataBaseCon(this IServiceCollection services , IConfiguration configuration)
        {

            var ConnectionString = configuration.GetConnectionString("DefaultConnection") ??
               throw new InvalidOperationException("Not founded as connectionstring");
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(ConnectionString));

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
