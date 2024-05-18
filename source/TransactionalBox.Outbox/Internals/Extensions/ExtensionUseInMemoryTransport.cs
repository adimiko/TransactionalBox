using TransactionalBox.Outbox.Configurators;
using TransactionalBox.Outbox.Internals.Transport.InMemory;
using TransactionalBox.Outbox.Internals.Transport;
using TransactionalBox.Internals.Transport.InMemory.Internals;
using Microsoft.Extensions.DependencyInjection;

namespace TransactionalBox.Outbox.Internals.Extensions
{
    internal static class ExtensionUseInMemoryTransport
    {
        internal static void UseInMemoryTransport(this IOutboxTransportConfigurator configurator)
        {
            var services = configurator.Services;

            services.UseInternalInMemoryTransport();
            services.AddSingleton<IOutboxTransport, InMemoryOutboxTransport>();
            services.AddSingleton<ITransportMessageSizeSettings>(new InMemoryTransportMessageSizeSettings());
        }
    }
}
