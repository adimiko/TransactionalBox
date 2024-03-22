using Microsoft.Extensions.DependencyInjection;

namespace TransactionalBox.InboxBase.DependencyBuilder
{
    public interface IInboxDependencyBuilder
    {
        internal IServiceCollection Services { get; }
    }
}
