using Microsoft.Extensions.DependencyInjection;
using TransactionalBox;
using TransactionalBox.Configurators.Inbox;
using TransactionalBox.Internals.Inbox.Configurators;

namespace TransactionalBox.Settings.Inbox
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
