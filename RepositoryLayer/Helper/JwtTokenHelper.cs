using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Repository_Layer.Helper
{
    public class JwtTokenHelper
    {
        private readonly IConfiguration _configuration;

        public JwtTokenHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GenerateToken(string email, int userId, string role)
        {
            if (string.IsNullOrEmpty(email))
                throw new ArgumentNullException(nameof(email), "Email cannot be null or empty");

            if (string.IsNullOrEmpty(role))
                throw new ArgumentNullException(nameof(role), "Role cannot be null or empty");

            var key = _configuration["JWT:SecretKey"];
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key), "JWT Key is missing in the configuration");


            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                 new Claim(ClaimTypes.Email, email),
                 new Claim("UserId", userId.ToString()),
                 new Claim(ClaimTypes.Role, role),
                 new Claim(JwtRegisteredClaimNames.Sub, email),
                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
             };

            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];

            if (string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience))
                throw new ArgumentNullException("Jwt:Issuer or Jwt:Audience", "JWT Issuer or Audience is missing in the configuration");

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
