using Microsoft.Extensions.DependencyInjection;

namespace TransactionalBox.Internals.InternalPackages.DistributedLock
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
