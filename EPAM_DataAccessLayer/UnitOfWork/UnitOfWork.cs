using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EPAM_DataAccessLayer.EF;
using EPAM_DataAccessLayer.Entities;
using EPAM_DataAccessLayer.Interfaces;
using EPAM_DataAccessLayer.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EPAM_DataAccessLayer.UnitOfWork
{
    public class UnitOfWork: IUnitOfWork
    {
        private Hashtable _repositories;

        public UnitOfWork(AuctionContext context)
        {
            Context = context;
            SeedRoles();
        }

        public AuctionContext Context { get; }

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

        private void SeedRoles()
        {
            string[] roles = { "Owner", "Administrator", "Moderator", "User", "Premium", "Plus" };
            var roleStore = new RoleStore<IdentityRole>(Context);
            roleStore.AutoSaveChanges = true;

            foreach (var role in roles)
            {
                if (!Context.Roles.Any(r => r.Name == role))
                {
                    roleStore.CreateAsync(new IdentityRole(role));
                }
            }
        }

        private static string FormatError(DbEntityValidationException ex)
        {
            var build = new StringBuilder();
            foreach (var error in ex.EntityValidationErrors)
            {
                var errorBuilder = new StringBuilder();

                foreach (var validationError in error.ValidationErrors)
                {
                    errorBuilder.AppendLine(string.Format("Property '{0}' errored:{1}", validationError.PropertyName, validationError.ErrorMessage));
                }

                build.AppendLine(errorBuilder.ToString());
            }
            return build.ToString();
        }

        public IRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            if (_repositories == null)
                _repositories = new Hashtable();

            var type = typeof(TEntity).Name;

            if (_repositories.ContainsKey(type)) return (IRepository<TEntity>)_repositories[type];

            var repositoryType = typeof(Repository<>);

            var repositoryInstance =
                Activator.CreateInstance(repositoryType
                    .MakeGenericType(typeof(TEntity)), Context);

            _repositories.Add(type, repositoryInstance);

            return (IRepository<TEntity>)_repositories[type];
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
            await CommitAsync(null, cancellationToken);
        }

        public void Commit(IDbContextTransaction? transaction)
        {
            try
            {
                Context.SaveChanges();
                transaction?.Commit();
            }
            catch (DbEntityValidationException ex)
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
            catch (DbEntityValidationException ex)
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

        public TEntity Delete<TEntity>(TEntity entity) where TEntity : class
        {
            return Repository<TEntity>().Delete(entity);
        }

        public void DeleteRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            Repository<TEntity>().DeleteRange(entities);
        }

        public IEnumerable<TEntity> Find<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            return Repository<TEntity>().Find(predicate);
        }

        public IQueryable<TEntity> GetAll<TEntity>() where TEntity : class
        {
            return Repository<TEntity>().GetAll();
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
    }
}
