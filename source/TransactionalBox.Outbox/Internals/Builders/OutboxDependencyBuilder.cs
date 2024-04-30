using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Outbox.Builders;

namespace TransactionalBox.Outbox.Internals.Builders
{
    internal sealed class OutboxDependencyBuilder : IOutboxDependencyBuilder
    {
        public IServiceCollection Services { get; }

        internal OutboxDependencyBuilder(IServiceCollection services)
        {
            Services = services;
        }
    }
}
