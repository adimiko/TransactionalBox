using Microsoft.Extensions.DependencyInjection;
using Microsoft.IO;
using TransactionalBox.Settings.Outbox.Compression;
using TransactionalBox.Internals.Outbox.Compression;
using TransactionalBox.Internals.Outbox.Compression.GZip;
using TransactionalBox.Configurators.Outbox;

namespace TransactionalBox
{
    public static class OutboxExtensionUseGZip
    {
        public static void UseGZip(
            this IOutboxCompressionConfigurator configurator,
            Action<GZipCompressionSettings>? settingsConfiguration = null)
        {
            var services = configurator.Services;

            var settings = new GZipCompressionSettings();

            if (settingsConfiguration is not null)
            {
                settingsConfiguration(settings);
            }

            services.AddSingleton(new RecyclableMemoryStreamManager());

            services.AddSingleton<IGZipCompressionSettings>(settings);

            services.AddSingleton<ICompression, GZipCompression>();
        }
    }
}
