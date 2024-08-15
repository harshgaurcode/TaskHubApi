using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskhub.Entities;

namespace Taskhub.Common.SharedMethods
{
    public class SharedMethods
    {
        private TaskhubDbContext _context;

        public SharedMethods(TaskhubDbContext context)
        {
            _context = context;
        }
        public  (byte[] PasswordHash, byte[] PasswordSalt) CreatePasswordHash(string password)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
               var passwordSalt = hmac.Key;
               var passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return (passwordSalt, passwordHash);
            }
        }

        public  bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        public string GetRoleNameById(Guid roleId)
        {

            var role = _context.UserRoles.FirstOrDefault(r => r.Id == roleId);
            return role?.RoleName ?? "UnknownRole";
        }
    }
}
