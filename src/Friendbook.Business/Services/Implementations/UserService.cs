using Friendbook.Business.Services.Interfaces;
using Friendbook.Core.Entities;
using Friendbook.Core.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Friendbook.Business.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IAppUserRepository repo;

        public UserService(IAppUserRepository repo)
        {
            this.repo = repo;
        }
        public async Task<AppUser> GetByIdAsync(string id)
        {
            if(id == null) throw new ArgumentNullException("id");
            var user = await  repo.GetByIdAsync(id);
            if (user == null) throw new ArgumentNullException("User not found!");
            return user;
        }
    }
}
