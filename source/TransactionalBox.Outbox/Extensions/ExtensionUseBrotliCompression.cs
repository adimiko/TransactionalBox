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
            this IOutboxCompressionConfigurator configurator,
            Action<BrotliCompressionSettings>? settingsConfiguration = null)
        {
            var services = configurator.Services;

            var settings = new BrotliCompressionSettings();

            if (settingsConfiguration is not null)
            {
                settingsConfiguration(settings);
            }

            services.AddSingleton(new RecyclableMemoryStreamManager());

            services.AddSingleton<IBrotliCompressionSettings>(settings);

            services.AddSingleton<ICompression, BrotliCompression>();
        }
    }
}
