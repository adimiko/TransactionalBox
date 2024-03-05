using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.OutboxBase.DependencyBuilder;

namespace TransactionalBox.Outbox.Internals
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
