using Friendbook.MVC.Services.Implementations;
using Friendbook.MVC.Services.Interfacses;

namespace Friendbook.MVC.Services
{
    public static class ServiceRegistration
    {
        public static void AddRegisterService(this IServiceCollection services)
        {
            services.AddScoped<ICrudService, CrudService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<TokenAuthorizationFilter>();
        }
    }
}
