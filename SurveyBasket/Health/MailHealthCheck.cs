using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using SurveyBasket.Options;

namespace SurveyBasket.Health
{
    public class MailHealthCheck(IOptions<EmailSettings> options) : IHealthCheck
    {
        private readonly EmailSettings _options = options.Value;

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                using var stmp = new SmtpClient();
                stmp.Connect(_options.Host, _options.Port, MailKit.Security.SecureSocketOptions.StartTls);
                stmp.Authenticate(_options.User, _options.Password);


                return await Task.FromResult(HealthCheckResult.Healthy("Mail service is healthy"));

            }
            catch(Exception ex)
            {
                return await Task.FromResult(HealthCheckResult.Unhealthy("Mail service is unhealthy"));

            }


        }
    }
}
