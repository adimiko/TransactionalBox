using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Internals.Inbox.Transport.ContractsToImplement;
using TransactionalBox.Internals.Inbox.Transport.InMemory;
using TransactionalBox.Configurators.Inbox;

namespace TransactionalBox.Internals.Inbox.Extensions
{
    internal static class InboxExtensionUseInMemoryTransport
    {
        internal static void UseInMemoryTransport(this IInboxTransportConfigurator configurator)
        {
            var services = configurator.Services;

            services.UseInternalInMemoryTransport();
            services.AddSingleton<IInboxTransport, InMemoryInboxWorkerTransport>();
            services.AddSingleton<ITransportTopicsCreator, InMemoryTransportTopicsCreator>();
        }
    }
}
