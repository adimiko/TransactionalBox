using Microsoft.Extensions.DependencyInjection;
using Microsoft.IO;
using TransactionalBox.Inbox.Configurators;
using TransactionalBox.Inbox.Internals.Decompression;

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
