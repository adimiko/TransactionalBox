using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Outbox.Configurators;
using TransactionalBox.Outbox.Internals.Transport.ContractsToImplement;
using TransactionalBox.Outbox.RabbitMQ.Internals.ImplementedContracts;
using TransactionalBox.Outbox.RabbitMQ.Settings;

namespace TransactionalBox
{
    public static class ExtensionUseRabbitMQ
    {
        public static void UseRabbitMQ(
            this IOutboxTransportConfigurator outboxWorkerTransportConfigurator,
            Action<OutboxRabbitMqSettings> settingsConfiguration)
        {
            var services = outboxWorkerTransportConfigurator.Services;
            var settings = new OutboxRabbitMqSettings();

            settingsConfiguration(settings);

            services.AddSingleton<ITransportMessageSizeSettings, RabbitMqTransportMessageSizeSettings>();

            services.AddSingleton(settings.ConnectionFactory);
            services.AddScoped<IOutboxTransport, RabbiMqOutboxTransport>();
        }
    }
}
