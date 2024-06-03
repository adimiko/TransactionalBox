using Microsoft.IO;
using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Settings.Outbox.Compression;
using TransactionalBox.Internals.Outbox.Compression;
using TransactionalBox.Internals.Outbox.Compression.Brotli;
using TransactionalBox.Configurators.Outbox;

namespace TransactionalBox
{
    public static class ExtensionUseBrotli
    {
        public static void UseBrotli(
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
