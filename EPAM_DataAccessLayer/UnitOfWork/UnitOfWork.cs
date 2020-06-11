using EPAM_DataAccessLayer.Contexts;
using EPAM_DataAccessLayer.Entities;
using EPAM_DataAccessLayer.Infrastructure;
using EPAM_DataAccessLayer.Repositories;
using EPAM_DataAccessLayer.Repositories.Interfaces;
using EPAM_DataAccessLayer.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EPAM_DataAccessLayer.UnitOfWork
{
    ///<inheritdoc cref="IUnitOfWork"/>
    public class UnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// Collection of TEntity/<see cref="IRepository{TEntity}"/> pairs
        /// </summary>
        private Hashtable _repositories;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
        /// </summary>
        /// <param name="context"></param>
        public UnitOfWork(AuctionContext context)
        {
            Context = context;
            //Task.Run(async () => await SeedRoles());
        }

        public IdentityDbContext<ApplicationUser> Context { get; }

        public void Dispose()
        {
            try
            {
                Commit();
            }
            finally
            {
                //Context.Dispose();
            }
        }

        // TODO: Remove
        private async Task SeedRoles()
        {
            string[] roles = { "Owner", "Administrator", "Moderator", "User", "Premium", "Plus" };
            var roleStore = new RoleStore<IdentityRole>(Context)
            {
                AutoSaveChanges = true
            };

            foreach (var role in roles)
            {
                if (Context.Roles.Any(r => r.Name == role))
                {
                    continue;
                }

                var nr = new IdentityRole(role)
                {
                    NormalizedName = role.ToUpper()
                };
                await roleStore.CreateAsync(nr);
            }
        }

        /// <summary>
        /// Method that formats exceptions
        /// </summary>
        /// <param name="ex"><seealso cref="ValidationException"/> exceptions</param>
        /// <returns>String formatted exception</returns>
        private static string FormatError(ValidationException ex)
        {
            var errorBuilder = new StringBuilder();

            foreach (var error in ex.EntityValidationErrors)
            {
                errorBuilder.AppendLine(string.Format($"Property '{error.MemberNames}' errored:{error.ErrorMessage}"));
            }
            return errorBuilder.ToString();
        }

        /// <summary>
        /// Function that provide direct access to <seealso cref="IRepository{TEntity}"/>
        /// </summary>
        /// <typeparam name="TEntity">Entity stored in Db</typeparam>
        /// <returns>Instance of <seealso cref="IRepository{TEntity}"/></returns>
        public IRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            if (_repositories == null)
                _repositories = new Hashtable();

            var type = typeof(TEntity).Name;

            if (_repositories.ContainsKey(type)) return _repositories[type] as IRepository<TEntity>;

            var repositoryType = typeof(Repository<>);

            var repositoryInstance =
                Activator.CreateInstance(repositoryType
                    .MakeGenericType(typeof(TEntity)), Context);

            _repositories.Add(type, repositoryInstance);

            return _repositories[type] as IRepository<TEntity>;
        }

        public IUnitOfWorkTransaction BeginTransaction()
        {
            return new UnitOfWorkTransaction(this);
        }

        public void Commit()
        {
            Commit(null);
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            await CommitAsync(null, cancellationToken).ConfigureAwait(false);
        }

        public void Commit(IDbContextTransaction? transaction)
        {
            try
            {
                Context.SaveChanges();
                transaction?.Commit();
            }
            catch (ValidationException ex)
            {
                var errors = FormatError(ex);
                throw new Exception(errors, ex);
            }
            catch
            {
                transaction?.Rollback();
                throw;
            }
        }

        public async Task CommitAsync(IDbContextTransaction? transaction, CancellationToken cancellationToken = default)
        {
            try
            {
                await Context.SaveChangesAsync(cancellationToken);
                if (transaction != null)
                {
                    await transaction.CommitAsync(cancellationToken);
                }
            }
            catch (ValidationException ex)
            {
                var errors = FormatError(ex);
                throw new Exception(errors, ex);
            }
            catch
            {
                transaction?.Rollback();
                throw;
            }
        }

        public TEntity Insert<TEntity>(TEntity entity) where TEntity : class
        {
            return Repository<TEntity>().Insert(entity);
        }

        public void InsertRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            Repository<TEntity>().InsertRange(entities);
        }

        public async Task<TEntity> InsertAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : class
        {
            return await Repository<TEntity>().InsertAsync(entity, cancellationToken);
        }

        public async Task InsertRangeAsync<TEntity>(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default) where TEntity : class
        {
            await Repository<TEntity>().InsertRangeAsync(entities, cancellationToken);
        }

        public TEntity Update<TEntity>(TEntity entity) where TEntity : class
        {
            return Repository<TEntity>().Update(entity);
        }

        public void UpdateRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            Repository<TEntity>().UpdateRange(entities);
        }

        public TEntity Remove<TEntity>(TEntity entity) where TEntity : class
        {
            return Repository<TEntity>().Remove(entity);
        }

        public void RemoveRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            Repository<TEntity>().RemoveRange(entities);
        }

        public IQueryable<TEntity> Find<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            return Repository<TEntity>().Find(predicate);
        }

        public IQueryable<TEntity> GetAll<TEntity>() where TEntity : class
        {
            return Repository<TEntity>().GetAll();
        }

        public IQueryable<TEntity> GetAll<TEntity>(int limit, int offset) where TEntity : class
        {
            return Repository<TEntity>().GetAll(limit, offset);
        }

        public TEntity GetById<TEntity>(Guid id) where TEntity : class
        {
            return Repository<TEntity>().GetById(id);
        }

        public async Task<TEntity> GetByIdAsync<TEntity>(Guid id) where TEntity : class
        {
            return await Repository<TEntity>().GetByIdAsync(id);
        }

        public IQueryable<TEntity> ExecuteQueryCommand<TEntity>(string sql, params object[] parameters) where TEntity : class
        {
            return Repository<TEntity>().ExecuteQueryCommand(sql, parameters);
        }

        public bool Any<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            return Repository<TEntity>().Any(predicate);
        }

        public async Task<bool>? AnyAsync<TEntity>(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default) where TEntity : class
        {
            return await Repository<TEntity>().AnyAsync(predicate, cancellationToken);
        }
    }
}
