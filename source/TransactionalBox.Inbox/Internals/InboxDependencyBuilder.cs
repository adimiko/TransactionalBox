using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Base.Inbox.DependencyBuilder;

namespace TransactionalBox.Inbox.Internals
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
