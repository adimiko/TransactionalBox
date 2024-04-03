using TransactionalBox.Outbox.Configurators;
using TransactionalBox.Base.Outbox.Storage.InMemory.Internals;

namespace TransactionalBox.Outbox.Storage.InMemory
{
    public static class Extensions
    {
        public static void UseInMemory(this IOutboxStorageConfigurator configurator) => configurator.UseInternalInMemoryStorage();
    }
}
