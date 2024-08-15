using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Taskhub.Common;
using Taskhub.Models.ProjectModels;
using Taskhub.Services.ProjectService;

namespace TaskHubApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _service;
        private readonly ILogger<ProjectController> _logger;
        private readonly APIResponse<object> _response;

        public ProjectController(IProjectService service,ILogger<ProjectController> logger,APIResponse<object> response)
        {
            _service=service;
            _logger = logger;
            _response = response;
        }


        [HttpPost("AddProject")]
        public async Task<ActionResult<APIResponse<object>>> AddProject([FromBody] AddProjectModel model)
        {
            _logger.LogInformation($"Project Creating {model}");
            try
            {
                var Result = await _service.AddProject(model);
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = Result;
                return _response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(AddProject)}");
                return Problem($"Something went wrong in the {nameof(AddProject)}",statusCode:500);
            }
        }

        [HttpPost("UpdateProject")]
        public async Task<ActionResult<APIResponse<object>>> UpdateProject([FromBody] UpdateProjectModel model)
        {
            try
            {
                var result = await _service.UpdateProject(model);
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = result;
                return _response;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/Project/GetProjectById
        [HttpGet("GetProjectById")]
        public async Task<ActionResult<APIResponse<object>>> GetProjectById(Guid projectId)
        {
            try
            {
                var result = await _service.GetProjectById(projectId);
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = result;
                return _response;
            }
            catch (KeyNotFoundException ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };

                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };

                return _response;
            }
        }

        // GET: api/Project/GetProjects
        [HttpGet("GetProjects")]
        public async Task<ActionResult<APIResponse<object>>> GetProjects()
        {
            try
            {
                var result = await _service.GetProjectList();
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = result;
                return _response;
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST: api/Task/Create
        [HttpPost("CreateTask")]
        public async Task<ActionResult<APIResponse<object>>> CreateTask([FromBody] CreateTaskModel model)
        {
            try
            {
                var result = await _service.AddTask(model);
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = result;
                return _response;
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT: api/Task/Update
        [HttpPut("UpdateTask")]
        public async Task<ActionResult<APIResponse<object>>> UpdateTask([FromBody] UpdateTaskModel model)
        {
            try
            {
                var result = await _service.UpdateTask(model);
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = result;
                return _response;
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PATCH: api/Task/UpdateStatus
        [HttpPatch("UpdateStatus")]
        public async Task<ActionResult<APIResponse<object>>> UpdateTaskStatus([FromBody] UpdateTaskStatusModel model)
        {
            try
            {
                var result = await _service.UpdateTaskStatus(model);
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = result;
                return _response;
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/Task/GetTaskById/{taskId}
        [HttpGet("GetTaskById/{taskId}")]
        public async Task<ActionResult<APIResponse<object>>> GetTaskById(Guid taskId)
        {
            try
            {
                var result = await _service.GetTaskById(taskId);
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = result;
                return _response;
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
