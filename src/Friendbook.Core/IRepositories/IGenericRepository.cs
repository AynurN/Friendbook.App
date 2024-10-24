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
    public interface IGenericRepository<TEntity> where TEntity : BaseClass, new()
    {
        public DbSet<TEntity> Table { get; }
        Task CreateAsync(TEntity entity);
        void Delete(TEntity entity);
        IQueryable<TEntity> GetByExpression(bool asNoTracking = false, Expression<Func<TEntity, bool>>? expression = null, params string[] includes);
        Task<TEntity> GetByIdAsync(int id);
        void Update(TEntity entity);

        Task<int> CommitAsync();
    }
}
