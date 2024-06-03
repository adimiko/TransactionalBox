using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Configurators.Outbox;
using TransactionalBox.Internals.Outbox.Compression;
using TransactionalBox.Internals.Outbox.Compression.NoCompression;

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
