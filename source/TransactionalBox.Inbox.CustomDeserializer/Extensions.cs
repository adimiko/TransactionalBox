using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Inbox.Configurators;
using TransactionalBox.Inbox.Deserialization;

namespace TransactionalBox.Inbox.CustomDeserializer
{
    public static class Extensions
    {
        public static void UseCustomDeserializer<TCustomDeserializer>(
            this IInboxDeserializationConfigurator configurator)
            where TCustomDeserializer : class, IInboxDeserializer
        {
            var services = configurator.Services;

            services.AddSingleton<IInboxDeserializer, TCustomDeserializer>();
        }
    }
}
