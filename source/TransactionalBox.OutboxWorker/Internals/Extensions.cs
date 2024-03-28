using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.OutboxWorker.Compression;
using TransactionalBox.OutboxWorker.Configurators;
using TransactionalBox.OutboxWorker.Internals.Compression;

namespace TransactionalBox.OutboxWorker.Internals
{
    internal static class Extensions
    {
        internal static void UseNoCompression(
            this IOutboxWorkerCompressionAlgorithmConfigurator configurator)
        {
            var services = configurator.Services;

            services.AddSingleton<ICompressionAlgorithm, NoCompression>();
        }
    }
}
