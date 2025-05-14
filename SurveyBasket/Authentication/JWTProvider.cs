
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SurveyBasket.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SurveyBasket.Authentication
{
    public class JWTProvider(IOptions<JwtOptions> options) : IJWTprovider
    {
        private readonly JwtOptions _options = options.Value;

        
        public (string token, int Expirein) GenerateToken(ApplicationUser user)
        {
            Claim[] claims = new Claim[] {

                new Claim(JwtRegisteredClaimNames.Sub  , user.Id),
                new Claim(JwtRegisteredClaimNames.Email  , user.Email!),
                new Claim(JwtRegisteredClaimNames.Name  , user.LastName),
                new Claim(JwtRegisteredClaimNames.FamilyName  , user.LastName),
                new Claim(JwtRegisteredClaimNames.Jti  ,Guid.NewGuid().ToString())


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
    }

}
