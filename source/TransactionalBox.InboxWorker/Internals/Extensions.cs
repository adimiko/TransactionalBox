using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.InboxWorker.Configurators;
using TransactionalBox.InboxWorker.Decompression;
using TransactionalBox.InboxWorker.Internals.Decompression;

namespace TransactionalBox.InboxWorker.Internals
{
    internal static class Extensions
    {
        internal static void UseNoDecompression(
            this IInboxWorkerDecompressionAlgorithmConfigurator configurator)
        {
            var services = configurator.Services;

            services.AddSingleton<IDecompressionAlgorithm, NoDecompression>();
        }
    }
}
