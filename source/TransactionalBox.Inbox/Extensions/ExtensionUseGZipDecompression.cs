using Microsoft.Extensions.DependencyInjection;
using Microsoft.IO;
using TransactionalBox.Inbox.Configurators;
using TransactionalBox.Inbox.Internals.Decompression;

namespace TransactionalBox.Inbox
{
    public static class ExtensionUseGZipDecompression
    {
        public static void UseGZipDecompression(
            this IInboxDecompressionAlgorithmConfigurator configurator)
        {
            var services = configurator.Services;

            services.AddSingleton(new RecyclableMemoryStreamManager());
            services.AddSingleton<IDecompressionAlgorithm, GZipDecompression>();
        }
    }
}
