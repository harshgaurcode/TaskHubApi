using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taskhub.Entities
{
    public class Ticket
    {
        public Guid Id { get; set; }

        [ForeignKey("Project")]
        public Guid ProjectId { get ; set; }

        [NotMapped]
        public Project? Project { get; set; }

        public string? TaskId { get; set; }=string.Empty;

        public string? Title { get; set; } = string.Empty;

        public string Discription { get; set; } = string.Empty; 

        public string Type { get; set; } = string.Empty;

        [ForeignKey("AssignedTo")]
        public Guid AssigendToId { get; set; }


        [NotMapped]
        public Users? AssignedTo { get; set; }
        
        public int Status { get; set; } 

        public string RelatedDocs { get; set; }= string.Empty;

        public DateTime? CreatedTime{ get; set; }

        public DateTime? UpdatedTime { get; set;}

        public DateTime EstimatedTime { get; set; }

        public int Priority { get; set; }


    }
}
