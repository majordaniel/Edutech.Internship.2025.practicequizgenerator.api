using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Globalization;
using Practice_Quiz_Generator.Domain.Models;
using Practice_Quiz_Generator.DummyData;

namespace Practice_Quiz_Generator.Service
{
    public class AuthService
    {
        public User Authenticate(string email, string password)
        {
            var user = DummyUsers.User
                .FirstOrDefault(u => u.Email == email && u.PasswordHash == password);
            return user;
        }


        private readonly IConfiguration _config;

        public AuthService(IConfiguration config)
        {
            _config = config;
        }


        public string GenerateJwt(User user)
        {
            var claims = new[]
            {
                new Claim("UserId", user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
            (
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_config["Jwt:ExpiresInMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string ForgotPassword(string email)
        {
            var user = DummyUsers.User.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                return null;
            }

            //generate reset token
            var token = Guid.NewGuid().ToString();

            //store it in password reset
            PasswordResetStore.ResetTokens[email] = token;

            //returns token. (for demo, this stimulates sending it by mail)
            return token;
        }

        public bool ResetPassword(string email, string token, string newPassword)
        {
            // Validate if email has a stored token
            if (!PasswordResetStore.ResetTokens.ContainsKey(email))
            {
                return false; // no reset request for this email
            }

            // Check if token matches
            if (PasswordResetStore.ResetTokens[email] != token)
            {
                return false; 
            }

            // Find the user
            var user = DummyUsers.User.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                return false; // no user with this email
            }

            // Update password
            user.PasswordHash = newPassword;

            // Invalidate token
            PasswordResetStore.ResetTokens.Remove(email);

            return true; 
        }

    }
}
