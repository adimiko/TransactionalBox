using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.InboxBase.DependencyBuilder;

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
