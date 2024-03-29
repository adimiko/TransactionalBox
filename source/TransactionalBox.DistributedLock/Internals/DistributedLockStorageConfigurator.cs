﻿using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.DistributedLock.Configurators;

namespace TransactionalBox.DistributedLock.Internals
{
    internal sealed class DistributedLockStorageConfigurator : IDistributedLockStorageConfigurator
    {
        public IServiceCollection Services { get; }

        public DistributedLockStorageConfigurator(IServiceCollection services) 
        {
            Services = services;
        }
    }
}
