using Microsoft.Extensions.DependencyInjection;

namespace TransactionalBox.Internals.DistributedLock.Configurators
{
    public interface IDistributedLockStorageConfigurator
    {
        internal IServiceCollection Services { get; }
    }
}
