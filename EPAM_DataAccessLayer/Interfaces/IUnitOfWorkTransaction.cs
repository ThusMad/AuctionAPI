using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace EPAM_DataAccessLayer.Interfaces
{
    public interface IUnitOfWorkTransaction : IDisposable
    {
        void Commit();
        Task CommitAsync(CancellationToken cancellationToken);
        void Rollback();
        Task RollbackAsync(CancellationToken cancellationToken);
        TEntity Insert<TEntity>(TEntity entity) where TEntity : class;
        Task<TEntity> InsertAsync<TEntity>(TEntity entity, CancellationToken cancellationToken) where TEntity : class;
        void InsertRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
        Task InsertRangeAsync<TEntity>(IEnumerable<TEntity> entities, CancellationToken cancellationToken) where TEntity : class;
        TEntity Update<TEntity>(TEntity entity) where TEntity : class;
        void UpdateRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
        TEntity Delete<TEntity>(TEntity entity) where TEntity : class;
        void DeleteRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
        IEnumerable<TEntity> Find<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;
        IQueryable<TEntity> GetAll<TEntity>() where TEntity : class;
        TEntity GetById<TEntity>(Guid id) where TEntity : class;
        Task<TEntity> GetByIdAsync<TEntity>(Guid id) where TEntity : class;
        IQueryable<TEntity> ExecuteQueryCommand<TEntity>(string sql, params object[] parameters) where TEntity : class;
    }
}