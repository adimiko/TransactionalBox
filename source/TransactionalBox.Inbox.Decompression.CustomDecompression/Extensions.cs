using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Inbox.Configurators;

namespace TransactionalBox.Inbox.Decompression.CustomDecompression
{
    public static class Extensions
    {
        public static void UseCustomDecompression<TCustomDecompression>(
            this IInboxDecompressionAlgorithmConfigurator configurator)
            where TCustomDecompression : class, IDecompressionAlgorithm
        {
            var services = configurator.Services;

            services.AddSingleton<IDecompressionAlgorithm, TCustomDecompression>();
        }
    }
}
