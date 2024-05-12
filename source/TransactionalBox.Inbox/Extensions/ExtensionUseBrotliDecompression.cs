using Microsoft.Extensions.DependencyInjection;
using Microsoft.IO;
using TransactionalBox.Inbox.Configurators;
using TransactionalBox.Inbox.Internals.Decompression;

namespace TransactionalBox.Inbox
{
    public static class ExtensionUseBrotliDecompression
    {
        public static void UseBrotliDecompression(
            this IInboxDecompressionConfigurator configurator)
        {
            var services = configurator.Services;

            services.AddSingleton(new RecyclableMemoryStreamManager());
            services.AddSingleton<IDecompression, BrotliDecompression>();
        }
    }
}
