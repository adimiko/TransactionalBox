using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.OutboxWorker.Compression;
using TransactionalBox.OutboxWorker.Configurators;
using TransactionalBox.OutboxWorker.GZipCompression.Internals;
using TransactionalBox.OutboxWorker.GZipCompression.Settings;

namespace TransactionalBox.OutboxWorker.GZipCompression
{
    public static class Extensions
    {
        public static void UseGZipCompression(
            this IOutboxWorkerCompressionAlgorithmConfigurator configurator,
            Action<GZipCompressionSettings>? configureSettings = null)
        {
            var services = configurator.Services;

            var settings = new GZipCompressionSettings();

            if (configureSettings is not null)
            {
                configureSettings(settings);
            }

            services.AddSingleton<IGZipCompressionSettings>(settings);

            services.AddSingleton<ICompressionAlgorithm, Internals.GZipCompression>();
        }
    }
}
