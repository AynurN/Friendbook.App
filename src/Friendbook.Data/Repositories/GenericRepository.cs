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
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseClass, new()
    {
        private readonly AppDbContext context;

        public GenericRepository(AppDbContext context)
        {
            this.context = context;
        }
        public DbSet<TEntity> Table =>context.Set<TEntity>();

        public async Task<int> CommitAsync()
         => await context.SaveChangesAsync();

        public async Task CreateAsync(TEntity entity)
         => await Table.AddAsync(entity);

        public void Delete(TEntity entity)
          => Table.Remove(entity);

        public async Task<TEntity> GetByIdAsync(int id)
            => await Table.FindAsync(id);

        public IQueryable<TEntity> GetByExpression(bool asNoTracking = false, Expression<Func<TEntity, bool>>? expression = null, params string[] includes)
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
