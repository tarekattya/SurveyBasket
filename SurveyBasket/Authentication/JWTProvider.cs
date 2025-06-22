
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SurveyBasket.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace SurveyBasket.Authentication
{
    public class JWTProvider(IOptions<JwtOptions> options) : IJWTprovider
    {
        private readonly JwtOptions _options = options.Value;

        
        public (string token, int Expirein) GenerateToken(ApplicationUser user , IEnumerable<string> roles, IEnumerable<string> permissions)
        {
            Claim[] claims ={

                new (JwtRegisteredClaimNames.Sub  , user.Id),
                new (JwtRegisteredClaimNames.Email  , user.Email!),
                new (JwtRegisteredClaimNames.Name  , user.FirstName),
                new (JwtRegisteredClaimNames.FamilyName  , user.LastName),
                new (JwtRegisteredClaimNames.Jti  ,Guid.NewGuid().ToString()),

                new(nameof(roles), JsonSerializer.Serialize(roles) , JsonClaimValueTypes.JsonArray),
                new(nameof(permissions), JsonSerializer.Serialize(permissions) , JsonClaimValueTypes.JsonArray)


            };
            var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));
            var credentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                signingCredentials: credentials,
                expires: DateTime.UtcNow.AddMinutes(_options.ExpireTime)
            );

            return (token: new JwtSecurityTokenHandler().WriteToken(token), Expirein: _options.ExpireTime);
        }

        public string ValidateToken(string token)
        {
            var TokenHandler = new JwtSecurityTokenHandler();
            var IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));

            try
            {


                 TokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = IssuerSigningKey,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);


                var JwtToken = validatedToken as JwtSecurityToken;
                return JwtToken!.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sub).Value;
            }
            catch (Exception)
            {
                return null!;
            }
        }
    }

}
