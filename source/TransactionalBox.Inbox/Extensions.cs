using TransactionalBox.Configurators;
using TransactionalBox.Inbox.Configurators;
using TransactionalBox.Inbox.Internals;
using TransactionalBox.InboxBase.DependencyBuilder;

namespace TransactionalBox.Inbox
{
    public static class Extensions
    {
        public static IInboxDependencyBuilder AddInbox(
            this ITransactionalBoxConfigurator configurator,
            Action<IInboxStorageConfigurator> storageConfiguration)
        {
            var services = configurator.Services;

            var storage = new InboxStorageConfigurator(services);

            storageConfiguration(storage);

            return new InboxDependencyBuilder(services);
        }
    }
}
