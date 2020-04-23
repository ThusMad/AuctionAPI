﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EPAM_DataAccessLayer.EF;
using EPAM_DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace EPAM_DataAccessLayer.UnitOfWork
{
    class UnitOfWorkTransaction : IUnitOfWorkTransaction
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDbContextTransaction _transaction;

        public UnitOfWorkTransaction(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _transaction = unitOfWork.Context.Database.BeginTransaction();
            Context = unitOfWork.Context;
        }

        public AuctionContext Context { get; set; }

        public void Dispose()
        {
            _unitOfWork.Commit(_transaction);
        }

        public void Commit()
        {
            _unitOfWork.Commit();
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            await _unitOfWork.CommitAsync(cancellationToken);
        }

        public void Rollback()
        {
            _transaction.Rollback();
        }

        public async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            await _transaction.RollbackAsync(cancellationToken);
        }

        public TEntity Insert<TEntity>(TEntity entity) where TEntity : class
        {
            return _unitOfWork.Insert(entity);
        }

        public Task<TEntity> InsertAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : class
        {
            return _unitOfWork.InsertAsync(entity, cancellationToken);
        }

        public void InsertRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            _unitOfWork.InsertRange(entities);
        }

        public async Task InsertRangeAsync<TEntity>(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default) where TEntity : class
        {
            await _unitOfWork.InsertRangeAsync(entities, cancellationToken);
        }

        public TEntity Update<TEntity>(TEntity entity) where TEntity : class
        {
            return _unitOfWork.Update(entity);
        }

        public void UpdateRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            _unitOfWork.UpdateRange(entities);
        }
       
        public TEntity Delete<TEntity>(TEntity entity) where TEntity : class
        {
            return _unitOfWork.Delete(entity);
        }

        public void DeleteRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            _unitOfWork.DeleteRange(entities);
        }

        public IEnumerable<TEntity> Find<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class { 
            return _unitOfWork.Find(predicate);
        }

        public IQueryable<TEntity> GetAll<TEntity>() where TEntity : class
        {
            return _unitOfWork.GetAll<TEntity>();
        }

        public TEntity GetById<TEntity>(Guid id) where TEntity : class
        {
            return _unitOfWork.GetById<TEntity>(id);
        }

        public async Task<TEntity> GetByIdAsync<TEntity>(Guid id) where TEntity : class
        {
            return await _unitOfWork.GetByIdAsync<TEntity>(id);
        }

        public IQueryable<TEntity> ExecuteQueryCommand<TEntity>(string sql, params object[] parameters) where TEntity : class
        {
            return _unitOfWork.ExecuteQueryCommand<TEntity>(sql, parameters);
        }
    }
}