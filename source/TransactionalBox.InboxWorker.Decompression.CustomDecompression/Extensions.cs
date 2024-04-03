using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.InboxWorker.Configurators;
using TransactionalBox.InboxWorker.Decompression;

namespace TransactionalBox.InboxWorker.Decompression.CustomDecompression
{
    public static class Extensions
    {
        public static void UseCustomDecompression<TCustomDecompression>(
            this IInboxWorkerDecompressionAlgorithmConfigurator configurator)
            where TCustomDecompression : class, IDecompressionAlgorithm
        {
            var services = configurator.Services;

            services.AddSingleton<IDecompressionAlgorithm, TCustomDecompression>();
        }
    }
}
