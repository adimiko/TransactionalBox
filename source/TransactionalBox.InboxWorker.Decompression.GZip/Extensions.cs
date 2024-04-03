using Microsoft.Extensions.DependencyInjection;
using Microsoft.IO;
using TransactionalBox.InboxWorker.Configurators;

namespace TransactionalBox.InboxWorker.Decompression.GZip
{
    public static class Extensions
    {
        public static void UseGZipDecompression(
            this IInboxWorkerDecompressionAlgorithmConfigurator configurator)
        {
            var services = configurator.Services;

            services.AddSingleton(new RecyclableMemoryStreamManager());
            services.AddSingleton<IDecompressionAlgorithm, Internals.GZipDecompression>();
        }
    }
}
