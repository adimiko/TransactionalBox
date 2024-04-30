using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Outbox.Configurators;
using TransactionalBox.Outbox.Internals.Compression;
using TransactionalBox.Outbox.Internals.Serialization;

namespace TransactionalBox.Outbox.Internals
{
    internal static class Extensions
    {
        internal static void UseNoCompression(
            this IOutboxWorkerCompressionAlgorithmConfigurator configurator)
        {
            var services = configurator.Services;

            services.AddSingleton<ICompressionAlgorithm, NoCompression>();
        }

        internal static void UseSystemTextJson(
            this IOutboxSerializationConfigurator configurator)
        {
            var services = configurator.Services;

            services.AddSingleton<IOutboxSerializer, OutboxSerializer>();
        }
    }
}
