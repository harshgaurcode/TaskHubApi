using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskhub.Entities;
using Taskhub.Models.ProjectModels;

namespace Taskhub.Services.ProjectService
{
    public class ProjectService:IProjectService
    {
        private readonly TaskhubDbContext _context;
        private readonly ILogger<ProjectService> _logger;

        public ProjectService(TaskhubDbContext context,ILogger<ProjectService> logger) 
        {
            _context = context;
            _logger= logger;
        }

        public async Task<object> AddProject(AddProjectModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }
            else
            {
                var projectID=Guid.NewGuid();
                var projectdescription = new Project()
                {
                    Id = Guid.NewGuid(),
                    ProjectId = projectID,
                    ProjectName = model.ProjectName,
                    ProjectDocs = model.ProjectDocs,
                    ProjectImage = model.ProjectImage,
                    ProjectProgress = model.ProjectProgress,
                    ProjectStatus = model.ProjectStatus,
                    CreatedTimeStamp = DateTime.Now,
                    ClientId = Guid.NewGuid(),//Client UserId
                    IsActive = true,
                    IsCompleted = false
                };
                //Manager Role 


                //Developer Role 


                //Qa Role


                var ProjectMembers = new ProjectMembers()
                {
                    Id = Guid.NewGuid(),
                    RoleId= Guid.NewGuid(),//Manager Role Id
                    UserId=Guid.NewGuid(), //Manager Id 
                    
                };

                await _context.Projects.AddAsync(projectdescription);
                await _context.SaveChangesAsync();
                return "Success";
            }
        }

        public async Task<object> UpdateProject (UpdateProjectModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }
            else
            {
                //Get Project By Id 
                var Project = _context.Projects.FirstOrDefault(x => x.ProjectId == model.ProjectId);
                if (Project == null)
                {
                    throw new ArgumentException("model");

                }
                else
                {

                    Project.ProjectName = model.ProjectName;
                    Project.ProjectDocs = model.ProjectDocs;
                    Project.ProjectImage = model.ProjectImage;
                    Project.ProjectProgress = model.ProjectProgress;
                    Project.ProjectStatus = model.ProjectStatus;
                    Project.CreatedTimeStamp = DateTime.Now;
                    Project.IsActive = true;
                    Project.IsCompleted = false;

                    await _context.SaveChangesAsync();
                    return "Success";
                }
                    
            }
        }

        public async Task<object> GetProjectList()
        {

            var projectdata = await _context.Projects.ToListAsync(); 
            if (projectdata.Count==0)
            {
                throw new KeyNotFoundException("Project not found.");
            }
            // Map the project data to a list of ProjectListResponse DTOs
            var response = projectdata.Select(project => new ProjectListResponse
            {
                ProjectId = project.ProjectId,
                ProjectName = project.ProjectName,
                ProjectDocs = project.ProjectDocs,
                ProjectImage = project.ProjectImage,
                ProjectProgress = project.ProjectProgress,
                ProjectStatus = project.ProjectStatus,
                Description = project.Description,
                ProjectDate = project.CreatedTimeStamp
            }).ToList();

            return response;
        }
        
        public async Task<object> GetProjectById(Guid projectId)
        {

            var projectTaskData = await (from project in _context.Projects
                                         join task in _context.Tickets
                                         on project.ProjectId equals task.ProjectId
                                         where project.Id == projectId
                                         select new
                                         {
                                             ProjectId = project.Id,
                                             ProjectName = project.ProjectName,
                                             TaskId = task.Id,
                                             TaskName = task.Title
                                         }).ToListAsync();
            if (projectTaskData == null)
            {
                throw new KeyNotFoundException("Project not found.");
            }
            var projectModel = new
            {
                Id = projectTaskData.First().ProjectId,
                ProjectName = projectTaskData.First().ProjectName,
                Tasks = projectTaskData.Select(t => new
                {
                    Id = t.TaskId,
                    TaskName = t.TaskName
                }).ToList()
            };

            return projectTaskData;
        }

        public async Task<object> GetTaskById(Guid taskId)
        {
            var task = await _context.Tickets
                .FirstOrDefaultAsync(t => t.Id == taskId);

            if (task == null)
            {
                _logger.LogWarning($"Record not Found in {nameof(GetTaskById)}");
                throw new KeyNotFoundException("Task not found.");
            }

            return task;
        }

        public async Task<object> AddTask(CreateTaskModel model) {

            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var task = new Ticket
            {
                Id = Guid.NewGuid(),
                ProjectId = model.ProjectId,
                TaskId = model.TaskId,
                Title = model.Task,
                Discription = model.TaskDescription,
                Type = model.TaskType,
                AssigendToId = model.AssignedToId,
                Status = model.Status,
                RelatedDocs = model.RelatedDocs,
                CreatedTime = DateTime.Now,
                EstimatedTime = model.EstimatedTime,
                Priority = model.Priority
            };

            await _context.Tickets.AddAsync(task);
            await _context.SaveChangesAsync();
            return "Task Created Successfully";
        

        }
        public async Task<object> UpdateTask(UpdateTaskModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var task = await _context.Tickets.FirstOrDefaultAsync(x => x.TaskId == model.TaskId);

            if (task == null)
            {
                throw new KeyNotFoundException("Task not found.");
            }

            task.Title = model.Task;
            task.Discription = model.TaskDescription;
            task.Type = model.TaskType;
            task.AssigendToId = model.AssignedToId;
            task.Status = model.Status;
            task.RelatedDocs = model.RelatedDocs;
            task.UpdatedTime = model.UpdatedTime;
            task.EstimatedTime = model.EstimatedTime;
            task.Priority = model.Priority;

            await _context.SaveChangesAsync();
            return "Task Updated Successfully";
        }

        public async Task<object> UpdateTaskStatus(UpdateTaskStatusModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var task = await _context.Tickets.FirstOrDefaultAsync(x => x.TaskId == model.TaskId);

            if (task == null)
            {
                throw new KeyNotFoundException("Task not found.");
            }

            task.Status = model.Status;
            task.UpdatedTime = model.UpdatedTime;

            await _context.SaveChangesAsync();
            return "Task Status Updated Successfully";
        }

        public async Task<object> AssignTask(string TaskId,Guid AssisedTo)
        {
            var Task = await _context.Tickets.FirstOrDefaultAsync(s => s.TaskId == TaskId);

            if(Task==null)
            {
                throw new NullReferenceException(nameof(Task));
            }
            
            Task.AssigendToId= AssisedTo;
            await _context.SaveChangesAsync();
            return new
            {
                Response = "Ok",
                Task=Task

            };

        }

      
    }
}
