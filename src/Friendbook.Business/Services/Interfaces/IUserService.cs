using Friendbook.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Friendbook.Business.Services.Interfaces
{
    public interface IUserService
    {
        Task<AppUser> GetByIdAsync(string id);
    }
}
