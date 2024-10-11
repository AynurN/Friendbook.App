using Friendbook.Business.Services.Implementations;
using Friendbook.Business.Services.Interfaces;
using Friendbook.Core.IRepositories;
using Friendbook.Data.Contexts;
using Friendbook.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Friendbook.Business
{
    public static class ServiceRegistration
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>(); 
        }
    }
}
