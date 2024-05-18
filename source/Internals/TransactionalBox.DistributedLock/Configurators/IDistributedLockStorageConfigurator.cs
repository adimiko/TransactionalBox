using Microsoft.Extensions.DependencyInjection;

namespace TransactionalBox.DistributedLock.Configurators
{
    public interface IDistributedLockStorageConfigurator
    {
        internal IServiceCollection Services { get; }
    }
}
