﻿using Microsoft.EntityFrameworkCore;
using System.Threading;
using TransactionalBox.Outbox.Internals.Storage;

namespace TransactionalBox.Outbox.EntityFrameworkCore.Internals
{
    internal sealed class EntityFrameworkUnitOfWork : IUnitOfWork
    {
        private readonly DbContext _dbContext;

        private readonly ITranactionCommited _tranactionCommited;

        public EntityFrameworkUnitOfWork(
            DbContext dbContext,
            ITranactionCommited tranactionCommited) 
        {
            _dbContext = dbContext;
            _tranactionCommited = tranactionCommited;
        }

        public IUnitOfWorkTransaction BeginTransaction()
        {
            var transaction = _dbContext.Database.BeginTransaction();

            return new EntityFrameworkTransaction(_dbContext, transaction, _tranactionCommited);
        }

        public async Task<IUnitOfWorkTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);

            return new EntityFrameworkTransaction(_dbContext, transaction, _tranactionCommited);
        }
    }
}
