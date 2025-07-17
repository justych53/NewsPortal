using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Extensions.Options;
using NewsPortal.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens;
using System.Text;
namespace NewsPortal.Auth
{
    public class JwtService(IOptions<AuthSettings> options)
    {
        public string GenerateToken(Account account)
        {
            var claims = new List<Claim>
            {
                new Claim("userName",account.UserName),
                new Claim("firstName",account.FirstName),
                new Claim("id",account.Id.ToString())
            };
            var jwtToken = new JwtSecurityToken(
                expires: DateTime.UtcNow.Add(options.Value.Expires),
                claims: claims,
                signingCredentials:
                new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.SecretKey)),
                SecurityAlgorithms.HmacSha256));
            
            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
                
        }
    }
}
