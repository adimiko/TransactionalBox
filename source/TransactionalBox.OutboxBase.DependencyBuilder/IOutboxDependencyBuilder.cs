using Microsoft.Extensions.DependencyInjection;

namespace TransactionalBox.OutboxBase.DependencyBuilder
{
    public interface IOutboxDependencyBuilder
    {
        IServiceCollection Services { get; }
    }
}
