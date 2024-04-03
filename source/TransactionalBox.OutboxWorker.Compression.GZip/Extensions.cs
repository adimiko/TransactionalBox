using Microsoft.Extensions.DependencyInjection;
using Microsoft.IO;
using TransactionalBox.OutboxWorker.Compression;
using TransactionalBox.OutboxWorker.Configurators;
using TransactionalBox.OutboxWorker.Compression.GZip.Internals;
using TransactionalBox.OutboxWorker.Compression.GZip.Settings;

namespace TransactionalBox.OutboxWorker.Compression.GZip
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

            services.AddSingleton(new RecyclableMemoryStreamManager());

            services.AddSingleton<IGZipCompressionSettings>(settings);

            services.AddSingleton<ICompressionAlgorithm, Internals.GZipCompression>();
        }
    }
}
