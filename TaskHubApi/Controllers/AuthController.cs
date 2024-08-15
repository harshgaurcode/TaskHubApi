using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Taskhub.Common;
using Taskhub.Models.ResponseDto;
using Taskhub.Models.UsersModel;
using Taskhub.Services.AuthService;

namespace TaskHubApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    [Authorize]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly APIResponse<object> _response;

        public AuthController(IAuthService authService,APIResponse<object> response)
        {
            _authService = authService;
            _response= response;
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<ActionResult<APIResponse<object>>> RegisterUser (UserRigisterModel registerModel)
        {
            try{ 
            var Response =await _authService.Register(registerModel);
            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            _response.Result = registerModel;

                return Ok(_response);
            }
            catch (Exception ex){ 
            
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };

                return _response;
            }
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult<APIResponse<LoginResponseModel>>> Login(UserLoginModel loginModel)
        {
            var loginResponse =await _authService.Login(loginModel);
            if(loginResponse== null && loginResponse.Token == "User Not Found" )
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.Result = loginResponse;
                return NotFound(_response);
            }
            else if(loginResponse.Token== "Password is Incorrect" && loginResponse.User==null)
            {
                _response.StatusCode = HttpStatusCode.Unauthorized;
                _response.IsSuccess = false;
                _response.Result=loginResponse;
                return Unauthorized(_response);

            }
            else
            {
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = loginResponse;
                return Ok(_response);
            }

        }

        [HttpGet("AuthecticationRequestList")]
        [ResponseCache(Duration=10,Location=ResponseCacheLocation.Any,NoStore =false)]
        public async Task<ActionResult<APIResponse<object>>> AuthecticationRequestList()
        {
            try
            {
                var Result = await _authService.ListOfUserAuthenticationRequest();
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = Result;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
                return _response;
            }
        }

        [HttpPost("AuthenticateUserForLogin")]
        public async Task<ActionResult<APIResponse<object>>> AuthenticateUserForLogin(Guid Id,Guid RoleId)
        {
            try
            {
                var response=await _authService.AuthenticateUsers(Id,RoleId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess=false;
                _response.ErrorMessages=new List<string> { ex.ToString() };
                return _response;
            }
        }
    }

}
