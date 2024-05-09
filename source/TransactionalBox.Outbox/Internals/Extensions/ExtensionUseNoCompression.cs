using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Outbox.Configurators;
using TransactionalBox.Outbox.Internals.Compression;

namespace TransactionalBox.Outbox.Internals.Extensions
{
    internal static class ExtensionUseNoCompression
    {
        internal static void UseNoCompression(
            this IOutboxCompressionAlgorithmConfigurator configurator)
        {
            var services = configurator.Services;

            services.AddSingleton<ICompressionAlgorithm, NoCompression>();
        }
    }
}
