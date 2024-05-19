using Microsoft.Extensions.DependencyInjection;

namespace TransactionalBox.Internals.DistributedLock.Configurators
{
    internal interface IDistributedLockStorageConfigurator
    {
        internal IServiceCollection Services { get; }
    }
}
