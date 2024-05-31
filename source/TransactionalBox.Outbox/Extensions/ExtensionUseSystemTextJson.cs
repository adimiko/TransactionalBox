using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Outbox.Configurators;
using TransactionalBox.Outbox.Internals.Serialization;

namespace TransactionalBox
{
    public static class ExtensionUseSystemTextJson
    {
        public static void UseSystemTextJson(this IOutboxSerializationConfigurator configurator)
        {
            var services = configurator.Services;

            services.AddSingleton<IOutboxSerializer, OutboxSerializer>();
        }
    }
}
