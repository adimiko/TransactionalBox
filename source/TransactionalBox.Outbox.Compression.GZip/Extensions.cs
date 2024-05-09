using Microsoft.Extensions.DependencyInjection;
using Microsoft.IO;
using TransactionalBox.Outbox.Compression.GZip.Internals;
using TransactionalBox.Outbox.Compression.GZip.Settings;
using TransactionalBox.Outbox.Configurators;
using TransactionalBox.Outbox.Internals.Compression;

namespace TransactionalBox.Outbox.Compression.GZip
{
    public static class Extensions
    {
        public static void UseGZipCompression(
            this IOutboxCompressionAlgorithmConfigurator configurator,
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
