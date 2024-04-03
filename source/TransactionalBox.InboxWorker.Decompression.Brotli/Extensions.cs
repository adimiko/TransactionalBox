using Microsoft.Extensions.DependencyInjection;
using Microsoft.IO;
using TransactionalBox.InboxWorker.Configurators;
using TransactionalBox.InboxWorker.Decompression;

namespace TransactionalBox.InboxWorker.Decompression.Brotli
{
    public static class Extensions
    {
        public static void UseBrotliDecompression(
            this IInboxWorkerDecompressionAlgorithmConfigurator configurator)
        {
            var services = configurator.Services;

            services.AddSingleton(new RecyclableMemoryStreamManager());
            services.AddSingleton<IDecompressionAlgorithm, Internals.BrotliDecompression>();
        }
    }
}
