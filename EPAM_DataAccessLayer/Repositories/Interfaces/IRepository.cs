using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace EPAM_DataAccessLayer.Repositories.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> GetAll(int limit, int offset);
        TEntity GetById(Guid id);
        Task<TEntity> GetByIdAsync(Guid id);
        IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        TEntity Update(TEntity entity);
        void UpdateRange(IEnumerable<TEntity> entities);
        TEntity Insert(TEntity entity);
        Task<TEntity> InsertAsync(TEntity item, CancellationToken cancellationToken);
        void InsertRange(IEnumerable<TEntity> entities);
        Task InsertRangeAsync(IEnumerable<TEntity> entity, CancellationToken cancellationToken);
        TEntity Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);
        IQueryable<TEntity> ExecuteQueryCommand(string sql, params object[] parameters);
        bool Any(Expression<Func<TEntity, bool>> predicate);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default);
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}