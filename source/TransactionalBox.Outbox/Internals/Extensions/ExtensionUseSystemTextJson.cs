using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Outbox.Configurators;
using TransactionalBox.Outbox.Internals.Serialization;

namespace TransactionalBox.Outbox.Internals.Extensions
{
    internal static class ExtensionUseSystemTextJson
    {
        internal static void UseSystemTextJson(this IOutboxSerializationConfigurator configurator)
        {
            var services = configurator.Services;

            services.AddSingleton<IOutboxSerializer, OutboxSerializer>();
        }
    }
}
