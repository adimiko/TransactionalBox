using TransactionalBox.Outbox.Configurators;
using TransactionalBox.Outbox.Internals.Compression.GZip;
using TransactionalBox.Outbox.Internals.Compression;
using TransactionalBox.Outbox.Settings.Compression;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IO;

namespace TransactionalBox.Outbox
{
    public static class ExtensionUseGZipCompression
    {
        public static void UseGZipCompression(
            this IOutboxCompressionConfigurator configurator,
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

            services.AddSingleton<ICompression, GZipCompression>();
        }
    }
}
