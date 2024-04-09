using Microsoft.Extensions.DependencyInjection;

namespace TransactionalBox.Base.Outbox.DependencyBuilder
{
    public interface IOutboxDependencyBuilder
    {
        internal IServiceCollection Services { get; }
    }
}
