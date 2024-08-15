using k8s.KubeConfigModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taskhub.Entities
{
    public class ProjectMembers
    {
        public Guid Id { get; set; }

        [ForeignKey("Project")]
        public Guid ProjectId { get; set; }
        public Project? Project { get; set; }

        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public User? User { get; set; }

        [ForeignKey("Role")]
        public Guid? RoleId { get; set; }
        public UserRole? Role { get; set; }


    }
}
