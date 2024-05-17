﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using TransactionalBox.Outbox.Internals.Storage;

namespace TransactionalBox.Outbox.EntityFrameworkCore.Internals
{
    internal sealed class EntityFrameworkUnitOfWork : IEntityFrameworkOutboxUnitOfWork
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

        public async Task<IEntityFrameworkTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);

            return new EntityFrameworkTransaction(_dbContext, transaction, _tranactionCommited);
        }
    }
}