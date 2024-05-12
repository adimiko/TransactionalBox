using TransactionalBox.Outbox.Configurators;
using TransactionalBox.Outbox.Internals.Compression.Brotli;
using TransactionalBox.Outbox.Internals.Compression;
using TransactionalBox.Outbox.Settings.Compression;
using Microsoft.IO;
using Microsoft.Extensions.DependencyInjection;

namespace TransactionalBox.Outbox
{
    public static class ExtensionUseBrotliCompression
    {
        public static void UseBrotliCompression(
            this IOutboxCompressionAlgorithmConfigurator configurator,
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

            services.AddSingleton<ICompressionAlgorithm, BrotliCompression>();
        }
    }
}
