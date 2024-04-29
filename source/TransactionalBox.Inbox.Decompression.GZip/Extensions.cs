using Microsoft.Extensions.DependencyInjection;
using Microsoft.IO;
using TransactionalBox.Inbox.Configurators;

namespace TransactionalBox.Inbox.Decompression.GZip
{
    public static class Extensions
    {
        public static void UseGZipDecompression(
            this IInboxDecompressionAlgorithmConfigurator configurator)
        {
            var services = configurator.Services;

            services.AddSingleton(new RecyclableMemoryStreamManager());
            services.AddSingleton<IDecompressionAlgorithm, Internals.GZipDecompression>();
        }
    }
}
