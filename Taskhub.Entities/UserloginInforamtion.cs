using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taskhub.Entities
{
    public class UserloginInforamtion
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public Guid UserId { get; set; }

        public byte[] PasswordHash { get; set; } = new byte[0];
        public byte[] PasswordSalt { get; set; } = new byte[0];

        public DateTime CreatedTimeStamp { get; set; }

        public DateTime UpdatedTimeStamp { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }
    }
}
