using TransactionalBox.Base.Inbox.Storage.InMemory.Internals;
using TransactionalBox.InboxWorker.Configurators;

namespace TransactionalBox.InboxWorker.Storage.InMemory
{
    public static class Extensions
    {
        public static void UseInMemory(this IInboxWorkerStorageConfigurator configurator) => configurator.UseInternalInMemory();
    }
}
