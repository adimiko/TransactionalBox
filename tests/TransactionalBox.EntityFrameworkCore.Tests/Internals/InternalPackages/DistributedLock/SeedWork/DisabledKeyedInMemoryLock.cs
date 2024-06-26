﻿using TransactionalBox.Internals.InternalPackages.KeyedInMemoryLock;

namespace TransactionalBox.EntityFrameworkCore.Tests.Internals.InternalPackages.DistributedLock.SeedWork
{
    internal sealed class DisabledKeyedInMemoryLock : IKeyedInMemoryLock
    {
        public Task<ILockInstance> Acquire(string key, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<ILockInstance>(new DisabledLockInstance());
        }
    }
}
