using TransactionalBox.OutboxWorker.Configurators;
using TransactionalBox.TransportBase.InMemory.Internals;

namespace TransactionalBox.OutboxWorker.InMemory
{
    public static class Extensions
    {
        public static void UseInMemory(this IOutboxWorkerTransportConfigurator configurator)
        {
            configurator.UseInternalInMemory();
        }
    }
}
