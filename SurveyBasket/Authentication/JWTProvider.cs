
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SurveyBasket.Authentication
{
    public class JWTProvider : IJWTprovider
    {
        public (string token, int Expirein) GenerateToken(ApplicationUser user)
        {
            Claim[] claims = new Claim[] {

                new Claim(JwtRegisteredClaimNames.Sub  , user.Id),
                new Claim(JwtRegisteredClaimNames.Email  , user.Email!),
                new Claim(JwtRegisteredClaimNames.Name  , user.LastName),
                new Claim(JwtRegisteredClaimNames.FamilyName  , user.LastName),
                new Claim(JwtRegisteredClaimNames.Jti  ,Guid.NewGuid().ToString())


            };

            var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("XDi9tvVcnu2BOJ7JSL1c0dcTVLW/9YkOuLs6gzQ4qSI="));

            var credentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);

            var ExpierIN = 30;

            var token = new JwtSecurityToken(
                issuer: "SurveyBasketApp",
                audience: "SurveyBasket",
                claims: claims,
                signingCredentials: credentials,
                expires: DateTime.UtcNow.AddMinutes(ExpierIN)
            );

            return (token: new JwtSecurityTokenHandler().WriteToken(token), Expirein: ExpierIN);
        }
    }

}
