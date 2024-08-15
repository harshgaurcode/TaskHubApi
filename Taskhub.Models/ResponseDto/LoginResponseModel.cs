using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taskhub.Models.ResponseDto
{
    public class LoginResponseModel
    {
        public UserDto? User { get; set; }

        public string Token { get; set; } = string.Empty;
    }
}
