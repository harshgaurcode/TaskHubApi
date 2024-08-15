using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taskhub.Entities
{
    public class Project
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }

        public string? ProjectName { get; set; }

        public string Description { get; set; }=string.Empty;

        public string ProjectImage { get; set; }=string.Empty;

        public string? ProjectDocs { get; set; } 
        public string ProjectStatus { get; set; }= string.Empty;

        public int ProjectProgress { get; set; }

        [ForeignKey("Client")]
        public Guid ClientId { get; set; }

        public Users? Client { get; set; }

        public ICollection<ProjectMembers>? ProjectMembers { get; set; }

        public DateTime CreatedTimeStamp { get; set; }

        public DateTime LastUpdatedTimeStamp { get; set;}

        public bool IsActive { get; set; }

        public bool IsCompleted  { get; set; }


    }
}
