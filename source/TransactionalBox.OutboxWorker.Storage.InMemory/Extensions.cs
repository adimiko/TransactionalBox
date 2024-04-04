using TransactionalBox.Base.Outbox.Storage.InMemory.Internals;
using TransactionalBox.OutboxWorker.Configurators;

namespace TransactionalBox.OutboxWorker.Storage.InMemory
{
    public static class Extensions
    {
        public static void UseInMemory(this IOutboxWorkerStorageConfigurator configurator) => configurator.UseInternalInMemoryStorage();
    }
}
