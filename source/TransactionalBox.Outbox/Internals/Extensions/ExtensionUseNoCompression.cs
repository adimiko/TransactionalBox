using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Outbox.Configurators;
using TransactionalBox.Outbox.Internals.Compression;
using TransactionalBox.Outbox.Internals.Compression.NoCompression;

namespace TransactionalBox.Outbox.Internals.Extensions
{
    internal static class ExtensionUseNoCompression
    {
        internal static void UseNoCompression(this IOutboxCompressionConfigurator configurator)
        {
            var services = configurator.Services;

            services.AddSingleton<ICompression, NoCompression>();
        }
    }
}
