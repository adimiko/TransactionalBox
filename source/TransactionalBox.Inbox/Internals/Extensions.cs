using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Inbox.Configurators;
using TransactionalBox.Inbox.Decompression;
using TransactionalBox.Inbox.Internals.Decompression;

namespace TransactionalBox.Inbox.Internals
{
    internal static class Extensions
    {
        internal static void UseNoDecompression(
            this IInboxDecompressionAlgorithmConfigurator configurator)
        {
            var services = configurator.Services;

            services.AddSingleton<IDecompressionAlgorithm, NoDecompression>();
        }
    }
}
