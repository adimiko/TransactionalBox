using Microsoft.Extensions.DependencyInjection;

namespace TransactionalBox.Outbox.Builders
{
    public interface IOutboxDependencyBuilder
    {
        internal IServiceCollection Services { get; }
    }
}
