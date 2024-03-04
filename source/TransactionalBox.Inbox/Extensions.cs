using TransactionalBox.Configurators;
using TransactionalBox.Inbox.Configurators;
using TransactionalBox.Inbox.Internals;

namespace TransactionalBox.Inbox
{
    public static class Extensions
    {
        public static ITransactionalBoxConfigurator AddInbox(
            this ITransactionalBoxConfigurator configurator,
            Action<IInboxStorageConfigurator> storageConfiguration)
        {
            var services = configurator.Services;

            var storage = new InboxStorageConfigurator(services);

            storageConfiguration(storage);

            return configurator;
        }
    }
}
