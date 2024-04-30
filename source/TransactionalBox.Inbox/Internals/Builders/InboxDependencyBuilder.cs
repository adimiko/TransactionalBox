using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Inbox.Builders;

namespace TransactionalBox.Inbox.Internals.Builders
{
    internal sealed class InboxDependencyBuilder : IInboxDependencyBuilder
    {
        public IServiceCollection Services { get; }

        internal InboxDependencyBuilder(IServiceCollection services)
        {
            Services = services;
        }
    }
}
