using Microsoft.Extensions.DependencyInjection;

namespace TransactionalBox.Base.Inbox.DependencyBuilder
{
    public interface IInboxDependencyBuilder
    {
        internal IServiceCollection Services { get; }
    }
}
