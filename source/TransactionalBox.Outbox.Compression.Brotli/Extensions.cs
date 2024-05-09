using Microsoft.Extensions.DependencyInjection;
using Microsoft.IO;
using TransactionalBox.Outbox.Compression.Brotli.Internals;
using TransactionalBox.Outbox.Compression.Brotli.Settings;
using TransactionalBox.Outbox.Configurators;
using TransactionalBox.Outbox.Internals.Compression;

namespace TransactionalBox.Outbox.Compression.Brotli
{
    public static class Extensions
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

            services.AddSingleton<ICompressionAlgorithm, Internals.BrotliCompression>();
        }
    }
}
