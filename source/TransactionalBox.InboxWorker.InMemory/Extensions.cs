using TransactionalBox.InboxWorker.Configurators;
using TransactionalBox.TransportBase.InMemory.Internals;

namespace TransactionalBox.InboxWorker.InMemory
{
    public static class Extensions
    {
        public static void UseInMemory(this IInboxWorkerTransportConfigurator configurator)
        {
            configurator.UseInternalInMemory();
        }
    }
}
