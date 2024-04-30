using Microsoft.Extensions.DependencyInjection;

namespace TransactionalBox.Inbox.Builders
{
    public interface IInboxDependencyBuilder
    {
        internal IServiceCollection Services { get; }
    }
}
