using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.InboxWorker.Configurators;
using TransactionalBox.InboxWorker.Decompression;

namespace TransactionalBox.InboxWorker.BrotliDecompression
{
    public static class Extensions
    {
        public static void UseBrotliDecompression(
            this IInboxWorkerDecompressionAlgorithmConfigurator configurator)
        {
            var services = configurator.Services;

            services.AddSingleton<IDecompressionAlgorithm, Internals.BrotliDecompression>();
        }
    }
}
