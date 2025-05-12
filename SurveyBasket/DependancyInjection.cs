using MapsterMapper;
using System.Reflection;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using SurveyBasket.Presistence.DbContextt;
using SurveyBasket.Services.NewFolder;
using SurveyBasket.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


namespace SurveyBasket
{
    public static class DependancyInjection 
    {

        public static IServiceCollection Addservices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IPollServices, PollServices>();
            services.AddScoped<IAuthService, AuthService>();

            

            services.AddControllers();
            services.AddOpenApi();


            services.AddSMapsterservices();
            services.AddIdentitityServices();
            services.AddSFluentValidationservices();
            services.AddDataBaseServices(configuration);

            return services;
        }

         static IServiceCollection AddDataBaseServices(this IServiceCollection services , IConfiguration configuration)
        {

            var ConnectionString = configuration.GetConnectionString("DefaultConnection") ??
               throw new InvalidOperationException("Not founded as connectionstring");
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(ConnectionString));

            return services;
        }

         static IServiceCollection AddSMapsterservices(this IServiceCollection services)
        {
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(Assembly.GetExecutingAssembly());
            services.AddSingleton<IMapper>(new Mapper(config));

            return services;
        }


         static IServiceCollection AddSFluentValidationservices(this IServiceCollection services)
        {
            services
             .AddFluentValidationAutoValidation()
             .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }

        static IServiceCollection AddIdentitityServices(this IServiceCollection services)
        {

            services.AddSingleton<IJWTprovider, JWTProvider>();
            services
             .AddIdentity<ApplicationUser, IdentityRole >()
             .AddEntityFrameworkStores<ApplicationDbContext>();




            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(
                o =>
                {
                    o.SaveToken = true;
                    o.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("XDi9tvVcnu2BOJ7JSL1c0dcTVLW/9YkOuLs6gzQ4qSI=\r\n")),
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidAudience = "SurveyBasketApp",
                        ValidIssuer = "SurveyBasket"

                    };



                });

            

            return services;
        }
    }
}
