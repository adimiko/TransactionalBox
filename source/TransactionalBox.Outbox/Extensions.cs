using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Configurators;
using TransactionalBox.Outbox.Configurators;
using TransactionalBox.Outbox.Internals;

namespace TransactionalBox.Outbox
{
    public static class Extensions
    {
        public static ITransactionalBoxConfigurator UseOutbox(
            this ITransactionalBoxConfigurator transactionalBoxConfigurator,
            Action<IOutboxStorageConfigurator> storageConfiguration)
        {
            var services = transactionalBoxConfigurator.Services;

            var outboxStorageConfigurator = new OutboxStorageConfigurator(services);

            storageConfiguration(outboxStorageConfigurator);

            services.AddSingleton<TopicFactory>();
            services.AddScoped<IOutboxSender, OutboxSender>();

            return transactionalBoxConfigurator;
        }
    }
}
