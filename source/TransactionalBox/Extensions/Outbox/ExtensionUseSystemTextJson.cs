using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Internals.Outbox.Serialization;
using TransactionalBox.Configurators.Outbox;

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
