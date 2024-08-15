using IdentityModel.OidcClient;
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
using Taskhub.Common.SharedMethods;
using Taskhub.Entities;
using Taskhub.Models.ResponseDto;
using Taskhub.Models.UsersModel;


namespace Taskhub.Services.AuthService
{
    public class AuthService:IAuthService
    {
        private readonly TaskhubDbContext _context;
        private IConfiguration _configuration;
        private readonly SharedMethods commonMethods;

        public AuthService(TaskhubDbContext context,IConfiguration configuration,SharedMethods CommonMethods)
        {
            _context = context;
            _configuration = configuration;
            commonMethods = CommonMethods;
        }

        public async Task<object> Register(UserRigisterModel registerModel)
        {
           if (registerModel == null)
            {

                return "Invalid Input";
            }

            var username = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName.ToLower().Equals(registerModel.UserName.ToLower()));
            if (username != null)
            {
                return "User Name Already Exists";
            }
            else
            {
                Guid UserId= Guid.NewGuid();
            

                //Adding Data in Users Table 
                var user = new Users()
                {
                    Id = UserId,
                    Name = registerModel.Name,
                    UserProfile = registerModel.UserProfile ?? string.Empty,
                    Address = registerModel.Address??string.Empty,
                    AlternateContact = registerModel.AlternateContact ?? string.Empty,
                    Signature = registerModel.Signature ?? string.Empty,
                    CreatedTimeStamp = DateTime.UtcNow,
                    IsActive = true,
                    IsDeleted = false,
                    Email=registerModel.Email ?? string.Empty,
                    PhoneNumber=registerModel.PhoneNumber ?? string.Empty,
                    IsAuthenticated=false,
                };


                //CreatePasswordHash(registerModel.Password, out byte[] passwordHash, out byte[] passwordSalt);
                var (passwordHash, passwordSalt) = commonMethods.CreatePasswordHash(registerModel.Password);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;

                await _context.Users.AddAsync(user);


                //Saving Data In Database
                await _context.SaveChangesAsync();

                var response = new { res = "ok",model= registerModel };

                return response;    

            }


        }

        public async Task<LoginResponseModel> Login(UserLoginModel loginModel)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName.ToLower().Equals(loginModel.UserName.ToLower()));
            if (user is null)
            {

                return new LoginResponseModel()
                {
                    Token = "User Not Found",
                    User = null
                };
            }
            else if (!commonMethods.VerifyPasswordHash(loginModel.Password, user.PasswordHash, user.PasswordSalt))
            {
                return new LoginResponseModel()
                {
                    Token = "Password is Incorrect",
                    User = null
                };
            }
            else if(user.IsAuthenticated==false)
            {
                return new LoginResponseModel()
                {
                    Token = "You are not Authenticated for login ",
                    User = null
                };
            }
            else
            {
                var UserResponse = new UserDto()
                {
        
                    UserName=user.UserName,
                    RoleId=user.RoleId
                };
                var Token= CreateToken(user);
                return new LoginResponseModel()
                {
                    Token = Token,
                    User = UserResponse
                };
            }

            
        }

        public async Task<object> ListOfUserAuthenticationRequest()
        {
            //&& s.RoleId == ""
            var AuthRequests=await _context.Users.Where(s=>s.IsAuthenticated==false ).ToListAsync();
     
            return AuthRequests;
        }

        public async Task<object> AuthenticateUsers(Guid UserId,Guid RoleId)
        {
            var user=await _context.Users.FirstOrDefaultAsync(s=>s.Id==UserId);
            if(user==null)
            {
                return "Not Found";
            }
            user.IsAuthenticated = true;
            user.RoleId=RoleId;
            await _context.SaveChangesAsync();
            return new
            {
                Result= "Success"
            };
        }


        private string CreateToken(Users user)
        {
            string? role = commonMethods.GetRoleNameById(user.RoleId);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role,role)
            };

            var appSettingsToken = _configuration.GetSection("Authentication:SecretKey").Value;
            if (appSettingsToken is null)
                throw new Exception("AppSettings Token is null!");

            SymmetricSecurityKey secretKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8
                .GetBytes(appSettingsToken));

            SigningCredentials sigingCreds = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha512Signature);

        

            var token = new JwtSecurityToken(
                _configuration.GetValue<string>(key: "Authentication:Issuer"),
                _configuration.GetValue<string>(key: "Authentication:Audience"),
                claims,
                DateTime.UtcNow, // when this token becomes valid 
                DateTime.UtcNow.AddDays(1),
                sigingCreds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        //private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        //{
        //    using (var hmac = new System.Security.Cryptography.HMACSHA512())
        //    {
        //        passwordSalt = hmac.Key;
        //        passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        //    }
        //}
    }
}
