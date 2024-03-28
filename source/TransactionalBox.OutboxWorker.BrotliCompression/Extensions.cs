using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.OutboxWorker.BrotliCompression.Internals;
using TransactionalBox.OutboxWorker.BrotliCompression.Settings;
using TransactionalBox.OutboxWorker.Compression;
using TransactionalBox.OutboxWorker.Configurators;

namespace TransactionalBox.OutboxWorker.BrotliCompression
{
    public static class Extensions
    {
        public static void UseBrotliCompression(
            this IOutboxWorkerCompressionAlgorithmConfigurator configurator,
            Action<BrotliCompressionSettings>? configureCompressionSettings = null)
        {
            var services = configurator.Services;

            var settings = new BrotliCompressionSettings();

            if (configureCompressionSettings is not null)
            {
                configureCompressionSettings(settings);
            }

            services.AddSingleton<IBrotliCompressionSettings>(settings);

            services.AddSingleton<ICompressionAlgorithm, Internals.BrotliCompression>();
        }
    }
}
