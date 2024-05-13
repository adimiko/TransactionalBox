using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Inbox.Configurators;
using TransactionalBox.Inbox.Internals.Decompression;

namespace TransactionalBox.Inbox.Internals.Extensions
{
    internal static class ExtensionUseNoDecompression
    {
        internal static void UseNoDecompression(
            this IInboxDecompressionConfigurator configurator)
        {
            var services = configurator.Services;

            services.AddSingleton<IDecompression, NoDecompression>();
        }
    }
}
