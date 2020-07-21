using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using EPAM_DataAccessLayer.Repositories.Interfaces;

namespace EPAM_DataAccessLayer.UnitOfWork.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUnitOfWorkTransaction : IDisposable
    {
        /// <summary>
        /// Commits changes
        /// </summary>
        void Commit();
        /// <summary>
        /// Commits changes asynchronous
        /// </summary>
        /// <param name="cancellationToken">cancellation token</param>
        /// <returns></returns>
        Task CommitAsync(CancellationToken cancellationToken = default);
        /// <summary>
        /// Rollback changes
        /// </summary>
        void Rollback();
        /// <summary>
        /// Rollback changes asynchronous
        /// </summary>
        /// <param name="cancellationToken">cancellation token</param>
        /// <returns></returns>
        Task RollbackAsync(CancellationToken cancellationToken);
        /// <summary>
        /// Inserts entity to <see cref="IRepository{TEntity}"/>
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity">instance of <seealso cref="TEntity"/></param>
        /// <returns><seealso cref="TEntity"/> with db generated fields</returns>
        TEntity Insert<TEntity>(TEntity entity) where TEntity : class;
        /// <summary>
        /// Inserts entity to <see cref="IRepository{TEntity}"/> asynchronous
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity">instance of <seealso cref="TEntity"/></param>
        /// <param name="cancellationToken">cancellation token</param>
        /// <returns><seealso cref="TEntity"/> with db generated fields</returns>
        Task<TEntity> InsertAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : class;
        /// <summary>
        /// Inserts collection of entities to <see cref="IRepository{TEntity}"/>
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities">Collection of <seealso cref="TEntity"/></param>
        void InsertRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
        /// <summary>
        /// Inserts collection of entities to <see cref="IRepository{TEntity}"/> asynchronous
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities">Collection of <seealso cref="TEntity"/></param>
        /// <param name="cancellationToken">cancellation token</param>
        Task InsertRangeAsync<TEntity>(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default) where TEntity : class;
        /// <summary>
        /// Updates entity in <see cref="IRepository{TEntity}"/>
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity">instance of <seealso cref="TEntity"/></param>
        /// <returns>Update entity</returns>
        TEntity Update<TEntity>(TEntity entity) where TEntity : class;
        /// <summary>
        /// Updates collection of entities in <see cref="IRepository{TEntity}"/>
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities">Collection of <seealso cref="TEntity"/></param>
        void UpdateRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
        /// <summary>
        /// Removes entity from <see cref="IRepository{TEntity}"/>
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity">instance of <seealso cref="TEntity"/></param>
        /// <returns>instance of removed entity</returns>
        TEntity Remove<TEntity>(TEntity entity) where TEntity : class;
        /// <summary>
        /// Removes collection of entities from <see cref="IRepository{TEntity}"/>
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities">Collection of <seealso cref="TEntity"/></param>
        void RemoveRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
        /// <summary>
        /// Finds collection of <see cref="IRepository{TEntity}"/> which satisfy the condition 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="predicate">Search term</param>
        /// <returns></returns>
        IEnumerable<TEntity> Find<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;
        /// <summary>
        /// Selecting all items from <see cref="IRepository{TEntity}"/>
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns>Collection of <seealso cref="TEntity"/></returns>
        IQueryable<TEntity> GetAll<TEntity>() where TEntity : class;
        /// <summary>
        /// Selecting entity with specific id 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="id">Unique entity id</param>
        /// <returns>instance of <seealso cref="TEntity"/></returns>
        TEntity GetById<TEntity>(Guid id) where TEntity : class;
        /// <summary>
        /// Selecting entity with specific id asynchronous
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="id">Unique entity id</param>
        /// <returns>instance of <seealso cref="TEntity"/></returns>
        Task<TEntity> GetByIdAsync<TEntity>(Guid id) where TEntity : class;
        /// <summary>
        /// Executes custom MS SQL query
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql">MS SQL query</param>
        /// <param name="parameters">query parameters</param>
        /// <returns>Collection of <seealso cref="TEntity"/></returns>
        IQueryable<TEntity> ExecuteQueryCommand<TEntity>(string sql, params object[] parameters) where TEntity : class;
    }
}