using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taskhub.Models.ProjectModels
{
    public class UpdateTaskStatusModel
    {
        public string TaskId { get; set; } = string.Empty;
        public int Status { get; set; }
        public DateTime UpdatedTime { get; set; } = DateTime.Now;
    }
}
