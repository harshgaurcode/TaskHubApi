using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taskhub.Models.ProjectModels
{
    public class CreateTaskModel
    {
        public Guid ProjectId { get; set; }
        public string TaskId { get; set; } = string.Empty;
        public string Task { get; set; } = string.Empty;
        public string TaskDescription { get; set; } = string.Empty;
        public string TaskType { get; set; } = string.Empty;
        public Guid AssignedToId { get; set; }
        public Guid AssignedById { get; set; }
        public int Status { get; set; }
        public string RelatedDocs { get; set; } = string.Empty;
        public DateTime EstimatedTime { get; set; }
        public int Priority { get; set; }
    }
}
