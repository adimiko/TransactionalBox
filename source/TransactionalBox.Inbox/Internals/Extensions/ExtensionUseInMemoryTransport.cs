using TransactionalBox.Inbox.Configurators;
using TransactionalBox.Inbox.Internals.Transport.InMemory;
using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Inbox.Internals.Transport.ContractsToImplement;

namespace TransactionalBox.Inbox.Internals.Extensions
{
    internal static class ExtensionUseInMemoryTransport
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
