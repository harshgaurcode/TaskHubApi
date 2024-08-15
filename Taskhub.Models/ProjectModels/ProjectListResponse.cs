using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taskhub.Models.ProjectModels
{
    public class ProjectListResponse
    {
        public Guid ProjectId { get; set; }

        public string? ProjectName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string ProjectImage { get; set; } = string.Empty;

        public string? ProjectDocs { get; set; } = string.Empty;
        public string ProjectStatus { get; set; } = string.Empty;

        public int ProjectProgress { get; set; }
        public DateTime ProjectDate { get; set; }
    }
}
