using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Outbox.Configurators;
using TransactionalBox.Outbox.Internals.Compression;
using TransactionalBox.Outbox.Internals.Compression.NoCompression;

namespace TransactionalBox
{
    public static class ExtensionUseNoCompression
    {
        public static void UseNoCompression(this IOutboxCompressionConfigurator configurator)
        {
            var services = configurator.Services;

            services.AddSingleton<ICompression, NoCompression>();
        }
    }
}
