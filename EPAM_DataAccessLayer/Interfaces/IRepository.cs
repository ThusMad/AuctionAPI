using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace EPAM_DataAccessLayer.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetAll();
        TEntity GetById(Guid id);
        Task<TEntity> GetByIdAsync(Guid id);
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        TEntity Update(TEntity entity);
        void UpdateRange(IEnumerable<TEntity> entities);
        TEntity Insert(TEntity entity);
        Task<TEntity> InsertAsync(TEntity item, CancellationToken cancellationToken);
        void InsertRange(IEnumerable<TEntity> entities);
        Task InsertRangeAsync(IEnumerable<TEntity> entity, CancellationToken cancellationToken);
        TEntity Delete(TEntity entity);
        void DeleteRange(IEnumerable<TEntity> entities);
        IQueryable<TEntity> ExecuteQueryCommand(string sql, params object[] parameters);
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}