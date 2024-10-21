using Friendbook.MVC.ApiResponseMessages;
using Friendbook.MVC.Services.Interfacses;
using RestSharp;

namespace Friendbook.MVC.Services.Implementations
{
    public class CrudService : ICrudService
    {
        private readonly RestClient _restClient;
        private readonly IConfiguration configuration;
        private readonly IHttpContextAccessor contextAccessor;

        public CrudService(IConfiguration configuration, IHttpContextAccessor contextAccessor)
        {
            this.configuration = configuration;
            this.contextAccessor = contextAccessor;
            _restClient = new RestClient(configuration.GetSection("API:Base_Url").Value);

            var token = contextAccessor.HttpContext.Request.Cookies["token"];

            if (token != null)
            {
                _restClient.AddDefaultHeader("Authorization", "Bearer " + token);
            }
        }
        public async Task<RestResponse<ApiResponseMessage<T>>> Create<T>(string endpoint, T entity) where T : class
        {
            var request = new RestRequest(endpoint, Method.Post);
            request.AddJsonBody(entity);
            var response = await _restClient.ExecuteAsync<ApiResponseMessage<T>>(request);

            return response;
        }

        public  async Task<RestResponse<ApiResponseMessage<T>>> Delete<T>(string endpoint, int id)
        {
            var request = new RestRequest(endpoint, Method.Delete);
            var response = await _restClient.ExecuteAsync<ApiResponseMessage<T>>(request);

            return response;
        }

        public async Task<RestResponse<ApiResponseMessage<T>>> GetAllAsync<T>(string endpoint)
        {
            var request = new RestRequest(endpoint, Method.Get);
            var response = await _restClient.ExecuteAsync<ApiResponseMessage<T>>(request);

            return response;
        }

        public async Task<RestResponse<ApiResponseMessage<T>>> GetByIdAsync<T>(string endpoint, int id)
        {
            if (id < 1) throw new Exception();
            var request = new RestRequest(endpoint, Method.Get);
            var response = await _restClient.ExecuteAsync<ApiResponseMessage<T>>(request);

            return response;
        }

        public async  Task<RestResponse<ApiResponseMessage<T>>> Update<T>(string endpoint, T entity) where T : class
        {
            var request = new RestRequest(endpoint, Method.Put);
            request.AddJsonBody(entity);
            var response = await _restClient.ExecuteAsync<ApiResponseMessage<T>>(request);

            return response;
        }
    }
}
