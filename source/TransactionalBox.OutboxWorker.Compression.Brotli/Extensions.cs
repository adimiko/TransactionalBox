using Microsoft.Extensions.DependencyInjection;
using Microsoft.IO;
using TransactionalBox.OutboxWorker.Compression.Brotli.Internals;
using TransactionalBox.OutboxWorker.Compression.Brotli.Settings;
using TransactionalBox.OutboxWorker.Compression;
using TransactionalBox.OutboxWorker.Configurators;

namespace TransactionalBox.OutboxWorker.Compression.Brotli
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

            services.AddSingleton(new RecyclableMemoryStreamManager());

            services.AddSingleton<IBrotliCompressionSettings>(settings);

            services.AddSingleton<ICompressionAlgorithm, Internals.BrotliCompression>();
        }
    }
}
