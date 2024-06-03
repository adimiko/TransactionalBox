using Microsoft.Extensions.DependencyInjection;

namespace TransactionalBox.Configurators.Inbox
{
    public interface IInboxDeserializationConfigurator
    {
        internal IServiceCollection Services { get; }
    }
}
