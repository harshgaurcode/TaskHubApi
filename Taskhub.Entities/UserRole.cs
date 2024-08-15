using k8s.KubeConfigModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taskhub.Entities
{
    public class UserRole
    {

        public Guid Id { get; set; }
        public string? RoleName { get; set; }

        public ICollection<User>? Users { get; set; }
    }

}
