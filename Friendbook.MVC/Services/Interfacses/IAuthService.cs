using Friendbook.MVC.ApiResponseMessages;
using Friendbook.MVC.Areas.LoginRegister.ViewModels;
using RestSharp;

namespace Friendbook.MVC.Services.Interfacses
{
    public interface IAuthService
    {
        Task<RestResponse<ApiResponseMessage<LoginResponseVM>>> Login(UserLoginVM vm);
        Task<RestResponse<ApiResponseMessage<object>>> Register(UserRegisterVM vm);

        void Logout();
    }
}
