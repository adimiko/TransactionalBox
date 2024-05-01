using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Base.Transport.InMemory.Internals;
using TransactionalBox.Outbox.Configurators;
using TransactionalBox.Outbox.Internals.Contracts;

namespace TransactionalBox.Outbox.Internals.Transport.InMemory
{
    internal static class Extensions
    {
        internal static void UseInternalInMemory(this IOutboxWorkerTransportConfigurator configurator)
        {
            var services = configurator.Services;

            services.UseInternalInMemoryTransport();
            services.AddSingleton<IOutboxWorkerTransport, InMemoryOutboxWorkerTransport>();
            services.AddSingleton<ITransportMessageSizeSettings>(new InMemoryTransportMessageSizeSettings());
        }
    }
}
