using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskhub.Models.ProjectModels;

namespace Taskhub.Services.ProjectService
{
    public interface IProjectService
    {
        public Task<object> AddProject(AddProjectModel model);
        public Task<object> UpdateProject(UpdateProjectModel model);
        public Task<object> AddTask(CreateTaskModel model);
        public Task<object> UpdateTask(UpdateTaskModel model);
        public Task<object> UpdateTaskStatus(UpdateTaskStatusModel model);

        public Task<object> GetProjectList();
        public Task<object> GetProjectById(Guid projectId);

        public Task<object> GetTaskById(Guid taskId);
    }
}
