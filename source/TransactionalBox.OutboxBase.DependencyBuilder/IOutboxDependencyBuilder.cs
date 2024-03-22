using Microsoft.Extensions.DependencyInjection;

namespace TransactionalBox.OutboxBase.DependencyBuilder
{
    public interface IOutboxDependencyBuilder
    {
        internal IServiceCollection Services { get; }
    }
}
