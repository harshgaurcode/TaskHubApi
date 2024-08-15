using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskhub.Common.SharedMethods;
using Taskhub.Entities;
using Taskhub.Models.UsersModel;
namespace Taskhub.Services.UserService
{
    public class UserService:IUserService
    {
        private readonly TaskhubDbContext _context;
        private readonly SharedMethods commonMethods;

        public UserService(TaskhubDbContext context,SharedMethods CommonMethods)
        {
            _context = context;
            commonMethods = CommonMethods;
        }

        public async Task<object> EditUserProfile(EditUserProfileModel model)
        {
            var userData = await _context.Users.FirstOrDefaultAsync(s => s.Id == model.UserId);
         
            if(userData == null)
            {
                return "No User Found";
            }
            
            userData.UserProfile=model.UserProfile;
            userData.Address=model.Address;
            userData.Email=model.Email;
            userData.Address = model.AlternateContact;
            userData.PhoneNumber=model.PhoneNumber; 
            userData.UserName=model.UserName;
            userData.Signature=model.Signature;

            //var pass=CreatePasswordHash(registerModel.Password, out byte[] passwordHash, out byte[] passwordSalt);
            var (passwordHash, passwordSalt) = commonMethods.CreatePasswordHash(model.Password);

          

            await _context.SaveChangesAsync();

            return "Success";
        }


    }
}
