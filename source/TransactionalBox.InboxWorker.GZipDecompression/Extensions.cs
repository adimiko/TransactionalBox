using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.InboxWorker.Configurators;
using TransactionalBox.InboxWorker.Decompression;

namespace TransactionalBox.InboxWorker.GZipDecompression
{
    public static class Extensions
    {
        public static void UseGZipDecompression(
            this IInboxWorkerDecompressionAlgorithmConfigurator configurator)
        {
            var services = configurator.Services;

            services.AddSingleton<IDecompressionAlgorithm, Internals.GZipDecompression>();
        }
    }
}
