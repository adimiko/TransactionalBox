using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.InboxWorker.Configurators;
using TransactionalBox.InboxWorker.Internals.Contracts;
using TransactionalBox.OutboxWorker.Configurators;
using TransactionalBox.OutboxWorker.Internals.Contracts;

namespace TransactionalBox.Base.Transport.InMemory.Internals
{
    internal static class Extensions
    {
        internal static void UseInternalInMemory(this IOutboxWorkerTransportConfigurator configurator)
        {
            var services = configurator.Services;

            services.AddSingleton<IOutboxWorkerTransport, InternalTransport>();
        }

        internal static void UseInternalInMemory(this IInboxWorkerTransportConfigurator configurator)
        {
            var services = configurator.Services;

            services.AddSingleton<IInboxWorkerTransport, InternalTransport>();
            services.AddSingleton<ITransportTopicWithWildCard, InMemoryTransportTopicWithWildCard>();
        }
    }
}
