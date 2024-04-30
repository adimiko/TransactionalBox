using TransactionalBox.OutboxWorker.Configurators;
using TransactionalBox.Base.Transport.InMemory.Internals;

namespace TransactionalBox.Outbox.Transport.InMemory
{
    public static class Extensions
    {
        public static void UseInMemory(this IOutboxWorkerTransportConfigurator configurator)
        {
            configurator.UseInternalInMemory();
        }
    }
}
