using MapsterMapper;
using System.Reflection;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using SurveyBasket.Presistence.DbContextt;
using SurveyBasket.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SurveyBasket.Options;
using Microsoft.AspNetCore.Identity.UI.Services;
using Hangfire;
using SurveyBasket.Authentication.Filters;


namespace SurveyBasket
{
    public static class DependancyInjection 
    {

        public static IServiceCollection Addservices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IPollServices, PollServices>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IUserServices, UserServices>();
            services.AddScoped<IVoteServices, VoteServices>();
            services.AddScoped<IResultService, ResultService>();
            services.AddScoped<IRoleServices, RoleServices>();
            services.AddScoped<IEmailSender, EmailServices>();
            services.AddScoped<INotifacitionServices, NotifacitionServices>();
                    
            


            services.AddControllers();
            services.AddOpenApi();
            services.AddHybridCache();
            services.AddHttpContextAccessor();
            services.AddDataBackGroundServices(configuration);






            services.AddExceptionHandler<GlobalExecptionsHandler>();
            services.Configure<IdentityOptions>(options => {
                options.Password.RequiredLength = 8;
                options.SignIn.RequireConfirmedEmail = true;
                options.User.RequireUniqueEmail = true;
            });
            services.AddProblemDetails();

            services.AddSMapsterservices();
            services.AddIdentitityServices(configuration);
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

        static IServiceCollection AddDataBackGroundServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Add Hangfire services.
            services.AddHangfire(config => config
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(configuration.GetConnectionString("HangfireCS")));

            // Add the processing server as IHostedService
            services.AddHangfireServer();

            return services;
        }


         static IServiceCollection AddSMapsterservices(this IServiceCollection services)
        {
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(Assembly.GetExecutingAssembly());
            services.AddSingleton<IMapper>(new Mapper(config));

            return services;
        }

        static IServiceCollection AddCORSservices(this IServiceCollection services)
        {
            services.AddCors(opt => opt.AddDefaultPolicy(builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()) );  
            return services;
        }

        static IServiceCollection AddSFluentValidationservices(this IServiceCollection services)
        {
            services
             .AddFluentValidationAutoValidation()
             .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }

        static IServiceCollection AddIdentitityServices(this IServiceCollection services, IConfiguration configuration)
        {

            services
             .AddIdentity<ApplicationUser, ApplicationRole>()
             .AddEntityFrameworkStores<ApplicationDbContext>()
             .AddDefaultTokenProviders();


            services.AddSingleton<IJWTprovider, JWTProvider>();
            //services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));

            services.AddOptions<JwtOptions>()
                .BindConfiguration(JwtOptions.SectionName)
                .ValidateDataAnnotations()
                .ValidateOnStart();

            var JwtSettings = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>();
            services.Configure<EmailSettings>(configuration.GetSection(nameof(EmailSettings)));


            services.AddTransient<IAuthorizationHandler, PermissionAuthorizationHandler>();
            services.AddTransient<IAuthorizationPolicyProvider, PermissionAuthPolicyProvider>();






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
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSettings!.Key)),
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidAudience = JwtSettings.Audience,
                        ValidIssuer = JwtSettings.Issuer

                    };



                });


           


            return services;
        }
    }
}
