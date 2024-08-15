using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taskhub.Models.ProjectModels
{
    public class AddProjectModel
    {
        public string ProjectName { get; set; } =string.Empty;

        public string Description { get; set; } = string.Empty;

        public string ProjectImage { get; set; } = string.Empty;

        public string ProjectDocs { get; set; } = string.Empty;
        public string ProjectStatus { get; set; } = string.Empty;

        public int ProjectProgress { get; set; }

        public string ClientName { get ; set; } = string.Empty; 

        public Guid ManagedBy { get; set; }

        public List<Guid> DeveloperIds { get; set; } = new List<Guid>();

        public Guid QaId { get; set; }
    }
}
