using Friendbook.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Friendbook.Core.IRepositories
{
    public interface IAppUserRepository
    {
        public DbSet<AppUser> Table { get; }
        Task CreateAsync(AppUser entity);
        void Delete(AppUser entity);
        IQueryable<AppUser> GetByExpression(bool asNoTracking = false, Expression<Func<AppUser, bool>>? expression = null, params string[] includes);
        Task<AppUser> GetByIdAsync(string id);

        Task<int> CommitAsync();
    }
}
