using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Inbox.Configurators;
using TransactionalBox.Inbox.Internals.Deserialization;

namespace TransactionalBox
{
    internal static class ExtensionUseSystemTextJson
    {
        internal static void UseSystemTextJson(this IInboxDeserializationConfigurator configurator)
        {
            var services = configurator.Services;

            services.AddSingleton<IInboxDeserializer, InboxDeserializer>();
        }
    }
}
