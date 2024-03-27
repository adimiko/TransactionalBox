using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.OutboxWorker.Compression;
using TransactionalBox.OutboxWorker.Configurators;

namespace TransactionalBox.OutboxWorker.CustomCompression
{
    public static class Extensions
    {
        public static void UseCustomCompression<TCustomCompression>(
            this IOutboxWorkerCompressionAlgorithmConfigurator configurator)
            where TCustomCompression : class, ICompressionAlgorithm
        {
            var services = configurator.Services;

            services.AddSingleton<ICompressionAlgorithm, TCustomCompression>();
        }
    }
}
