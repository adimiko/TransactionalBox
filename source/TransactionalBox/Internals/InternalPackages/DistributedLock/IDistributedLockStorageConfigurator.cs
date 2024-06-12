using Microsoft.Extensions.DependencyInjection;

namespace TransactionalBox.Internals.InternalPackages.DistributedLock
{
    internal interface IDistributedLockStorageConfigurator
    {
        internal IServiceCollection Services { get; }
    }
}
