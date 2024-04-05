using TransactionalBox.Inbox.Configurators;
using TransactionalBox.Base.Inbox.Storage.InMemory.Internals;

namespace TransactionalBox.Inbox.Storage.InMemory
{
    public static class Extensions
    {
        public static void UseInMemory(this IInboxStorageConfigurator configurator) => configurator.UseInternalInMemory();
    }
}
