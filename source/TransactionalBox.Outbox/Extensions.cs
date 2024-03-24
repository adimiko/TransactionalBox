using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Builders;
using TransactionalBox.Outbox.Configurators;
using TransactionalBox.Outbox.Internals;
using TransactionalBox.OutboxBase.DependencyBuilder;

namespace TransactionalBox.Outbox
{
    public static class Extensions
    {
        public static IOutboxDependencyBuilder AddOutbox(
            this ITransactionalBoxBuilder builder,
            Action<IOutboxStorageConfigurator> storageConfiguration)
        {
            var services = builder.Services;

            var outboxStorageConfigurator = new OutboxStorageConfigurator(services);

            storageConfiguration(outboxStorageConfigurator);

            services.AddSingleton<TopicFactory>();
            services.AddScoped<IOutbox, InternalOutbox>();

            return new OutboxDependencyBuilder(services);
        }
    }
}
