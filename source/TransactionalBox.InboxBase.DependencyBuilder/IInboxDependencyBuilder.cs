using Microsoft.Extensions.DependencyInjection;

namespace TransactionalBox.InboxBase.DependencyBuilder
{
    public interface IInboxDependencyBuilder
    {
        IServiceCollection Services { get; }
    }
}
