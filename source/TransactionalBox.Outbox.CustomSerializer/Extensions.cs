using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Outbox.Configurators;
using TransactionalBox.Outbox.Serialization;

namespace TransactionalBox.Outbox.CustomSerializer
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