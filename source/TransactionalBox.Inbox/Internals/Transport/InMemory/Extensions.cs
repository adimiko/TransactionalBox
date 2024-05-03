using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Base.Transport.InMemory.Internals;
using TransactionalBox.Inbox.Configurators;

namespace TransactionalBox.Inbox.Internals.Transport.InMemory
{
    internal static class Extensions
    {
        internal static void UseInternalInMemory(this IInboxTransportConfigurator configurator)
        {
            var services = configurator.Services;

            services.UseInternalInMemoryTransport();
            services.AddSingleton<IInboxWorkerTransport, InMemoryInboxWorkerTransport>();
        }
    }
}
