using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using UmaDesignli.Application.Interfaces;
using UmaDesignli.Domain.Entities;

namespace UmaDesignli.Infrastructure.Token
{
    /// <summary>
    /// Token provider class.
    /// </summary>
    /// <param name="configuration"></param>
    public sealed class TokenProvider(IConfiguration configuration) : ITokenProvider
    {
        /// <summary>
        /// Method for create the Token base don the Jwt configuration.
        /// </summary>
        /// <param name="user">User entity</param>
        /// <returns>Token</returns>
        public string Create (User user)
        {
            string secretKey = configuration["Jwt:Secret"]!;
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Nickname, user.Username),
                    new Claim(JwtRegisteredClaimNames.Name, $"{user.Name} {user.LastName}".Trim()),
                ]),
                Expires = DateTime.UtcNow.AddMinutes(configuration.GetValue<int>("Jwt:ExpirationInMinutes")),
                SigningCredentials = credentials,
                Issuer = configuration["Jwt:Issuer"],
                Audience = configuration["Jwt:Audience"],
            };

            var handler = new JsonWebTokenHandler();

            string token = handler.CreateToken(tokenDescriptor);
            return token;
        }

 
    }
}
