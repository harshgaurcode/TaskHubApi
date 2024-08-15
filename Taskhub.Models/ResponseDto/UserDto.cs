using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taskhub.Models.ResponseDto
{
    public class UserDto
    {
        public string UserName { get; set; } = string.Empty;
       public Guid? RoleId { get; set; }
    }
}
