using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Inbox.Configurators;
using TransactionalBox.Inbox.Internals.Configurators;
using TransactionalBox.Inbox.Internals.Extensions;

namespace TransactionalBox.Inbox.Settings
{
    public sealed class InboxSettings
    {
        public AddMessagesToInboxSettings AddMessagesToInboxSettings { get; } = new AddMessagesToInboxSettings();

        public CleanUpInboxSettings CleanUpInboxSettings { get; } = new CleanUpInboxSettings();

        public CleanUpIdempotencyKeysSettings CleanUpIdempotencyKeysSettings { get; } = new CleanUpIdempotencyKeysSettings();

        public Action<IInboxDeserializationConfigurator> ConfigureDeserialization { get; set; } = x => x.UseSystemTextJson();

        internal InboxSettings() { }

        internal void ConfigureDelegates(IServiceCollection services)
        {
            ConfigureDeserialization(new InboxDeserializationConfigurator(services));
        }
    }
}
