using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Configurators.Inbox;
using TransactionalBox.Internals.Inbox.Deserialization;

namespace TransactionalBox
{
    internal static class InboxExtensionUseSystemTextJson
    {
        internal static void UseSystemTextJson(this IInboxDeserializationConfigurator configurator)
        {
            var services = configurator.Services;

            services.AddSingleton<IInboxDeserializer, InboxDeserializer>();
        }
    }
}
