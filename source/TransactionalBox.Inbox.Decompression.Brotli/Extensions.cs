using Microsoft.Extensions.DependencyInjection;
using Microsoft.IO;
using TransactionalBox.Inbox.Configurators;

namespace TransactionalBox.Inbox.Decompression.Brotli
{
    public static class Extensions
    {
        public static void UseBrotliDecompression(
            this IInboxDecompressionAlgorithmConfigurator configurator)
        {
            var services = configurator.Services;

            services.AddSingleton(new RecyclableMemoryStreamManager());
            services.AddSingleton<IDecompressionAlgorithm, Internals.BrotliDecompression>();
        }
    }
}
