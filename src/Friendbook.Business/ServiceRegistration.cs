using Friendbook.Business.Services.Implementations;
using Friendbook.Business.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Friendbook.Data.Contexts;
using Friendbook.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Friendbook.Core.Entities;

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
