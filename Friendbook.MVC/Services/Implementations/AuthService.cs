using Friendbook.MVC.ApiResponseMessages;
using Friendbook.MVC.Areas.LoginRegister.ViewModels;
using Friendbook.MVC.Services.Interfacses;
using RestSharp;

namespace Friendbook.MVC.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly RestClient _restClient;
        private readonly IConfiguration configuration;
        private readonly IHttpContextAccessor contextAccessor;

        public AuthService(IConfiguration configuration, IHttpContextAccessor contextAccessor)
        {
            this.configuration = configuration;
            this.contextAccessor = contextAccessor;
           _restClient = new RestClient(configuration.GetSection("API:Base_Url").Value);
        }
        public async Task<RestResponse<ApiResponseMessage<LoginResponseVM>>> Login(UserLoginVM vm)
        {
            var request = new RestRequest("/auths/login", Method.Post);
            request.AddJsonBody(vm);

            var response = await _restClient.ExecuteAsync<ApiResponseMessage<LoginResponseVM>>(request);

            return response;
        }

        public void Logout()
        {
            contextAccessor.HttpContext.Response.Cookies.Delete("token");
        }

        public async  Task<RestResponse<ApiResponseMessage<object>>> Register(UserRegisterVM vm)
        {
            var request = new RestRequest("/auths/register", Method.Post);
            request.AddJsonBody(vm);
            var response = await _restClient.ExecuteAsync<ApiResponseMessage<object>>(request);

            return response;
        }
    }
}
