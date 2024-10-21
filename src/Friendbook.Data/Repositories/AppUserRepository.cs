using Friendbook.Core.Entities;
using Friendbook.Core.IRepositories;
using Friendbook.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Friendbook.Data.Repositories
{
    public class AppUserRepository : IAppUserRepository
    {
        private readonly AppDbContext context;

        public AppUserRepository(AppDbContext context)
        {
            this.context = context;
        }
        public DbSet<AppUser> Table => context.Set<AppUser>();

        public async Task<int> CommitAsync()
         => await context.SaveChangesAsync();

        public async Task CreateAsync(AppUser entity)
         => await Table.AddAsync(entity);

        public void Delete(AppUser entity)
          => Table.Remove(entity);

        public async Task<AppUser> GetByIdAsync(string id)
            => await Table.FindAsync(id);

        public IQueryable<AppUser> GetByExpression(bool asNoTracking = false, Expression<Func<AppUser, bool>>? expression = null, params string[] includes)
        {
            var query = Table.AsQueryable();
            if (includes.Length > 0)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            query = asNoTracking == true
                ? query.AsNoTracking()
                : query;

            return expression is not null
                ? query.Where(expression)
                : query;
        }
    }
}
