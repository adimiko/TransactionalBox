using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Outbox.Configurators;

namespace TransactionalBox.Outbox.Serialization.CustomSerializer
{
    public static class Extensions
    {
        public static void UseCustomSerializer<TCustomSerializer>(
            this IOutboxSerializationConfigurator configurator)
            where TCustomSerializer : class, IOutboxSerializer
        {
            var services = configurator.Services;

            services.AddSingleton<IOutboxSerializer, TCustomSerializer>();
        }
    }
}