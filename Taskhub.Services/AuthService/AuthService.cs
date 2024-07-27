using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Taskhub.Entities;
using Taskhub.Models;

namespace Taskhub.Services.AuthService
{
    public class AuthService:IAuthService
    {
        private readonly TaskhubDbContext _context;
        private IConfiguration _configuration;

        public AuthService(TaskhubDbContext DbContext,IConfiguration configuration)
        {
            DbContext = _context;
            configuration = _configuration;
        }

        public async Task<object> Login(UserLoginModel loginModel)
        {
            var user = await _context.UserloginInforamtions
                .FirstOrDefaultAsync(u => u.UserName.ToLower().Equals(loginModel.UserName.ToLower()));
            if (user is null)
            {
                return "User Not Found";
            }
            else if (!VerifyPasswordHash(loginModel.Password, user.PasswordHash, user.PasswordSalt))
            {
                return "Wrong password.";
            }
            else
            {
                var Token= CreateToken(user);
            }

            return response;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };

            var appSettingsToken = _configuration.GetSection("AppSettings:Token").Value;
            if (appSettingsToken is null)
                throw new Exception("AppSettings Token is null!");

            SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8
                .GetBytes(appSettingsToken));

            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
