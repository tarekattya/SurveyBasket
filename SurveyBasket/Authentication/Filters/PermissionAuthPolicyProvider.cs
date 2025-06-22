using Microsoft.Extensions.Options;

namespace SurveyBasket.Authentication.Filters
{
    public class PermissionAuthPolicyProvider(IOptions<AuthorizationOptions> options) : DefaultAuthorizationPolicyProvider(options)
    {

        private readonly AuthorizationOptions _authorizationBuilder = options.Value;
        public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            var policy = await base.GetPolicyAsync(policyName);

            if (policy is not null)
                return policy;

            var policyPermission = new AuthorizationPolicyBuilder()
               .AddRequirements(new PermissionsRequirement(policyName))
                .Build();


            _authorizationBuilder
                .AddPolicy(policyName, policyPermission);

            return policyPermission;



        }
    }
}
