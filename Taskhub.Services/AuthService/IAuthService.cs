using Taskhub.Models.ResponseDto;
using Taskhub.Models.UsersModel;

namespace Taskhub.Services.AuthService
{
    public interface IAuthService
    {
        Task<object> Register(UserRigisterModel registerModel);

        Task<LoginResponseModel> Login(UserLoginModel loginModel);

        Task<object> ListOfUserAuthenticationRequest();

        Task<object> AuthenticateUsers(Guid UserId,Guid RoleId);
    }
}