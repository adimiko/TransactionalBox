using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Base.Transport.InMemory.Internals;
using TransactionalBox.Inbox.Configurators;
using TransactionalBox.Inbox.Internals.Contracts;

namespace TransactionalBox.Inbox.Internals.Transport.InMemory
{
    internal static class Extensions
    {
        internal static void UseInternalInMemory(this IInboxTransportConfigurator configurator)
        {
            var services = configurator.Services;

            services.UseInternalInMemoryTransport();
            services.AddSingleton<ITransportTopicWithWildCard, InMemoryTransportTopicWithWildCard>();
            services.AddSingleton<IInboxWorkerTransport, InMemoryInboxWorkerTransport>();
        }
    }
}
