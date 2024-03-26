using Microsoft.Extensions.DependencyInjection;

namespace TransactionalBox.Inbox.Configurators
{
    public interface IInboxDeserializationConfigurator
    {
        internal IServiceCollection Services { get; }
    }
}
