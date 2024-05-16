using TransactionalBox.Inbox.Configurators;
using TransactionalBox.Inbox.Internals.Transport.InMemory;
using TransactionalBox.Inbox.Internals.Transport;
using TransactionalBox.Base.Transport.InMemory.Internals;
using Microsoft.Extensions.DependencyInjection;

namespace TransactionalBox.Inbox.Internals.Extensions
{
    internal static class ExtensionUseInMemoryTransport
    {
        internal static void UseInMemoryTransport(this IInboxTransportConfigurator configurator)
        {
            var services = configurator.Services;

            services.UseInternalInMemoryTransport();
            services.AddSingleton<IInboxTransport, InMemoryInboxWorkerTransport>();
        }
    }
}
