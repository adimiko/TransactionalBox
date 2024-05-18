using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Internals.DistributedLock.Configurators;

namespace TransactionalBox.Internals.DistributedLock.Internals.Configurators
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
