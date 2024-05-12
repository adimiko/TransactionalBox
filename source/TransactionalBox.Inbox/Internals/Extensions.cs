using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Inbox.Configurators;
using TransactionalBox.Inbox.Internals.Decompression;
using TransactionalBox.Inbox.Internals.Deserialization;

namespace TransactionalBox.Inbox.Internals
{
    internal static class Extensions
    {
        internal static void UseNoDecompression(
            this IInboxDecompressionConfigurator configurator)
        {
            var services = configurator.Services;

            services.AddSingleton<IDecompression, NoDecompression>();
        }

        internal static void UseSystemTextJson(this IInboxDeserializationConfigurator configurator)
        {
            var services = configurator.Services;

            services.AddSingleton<IInboxDeserializer, InboxDeserializer>();
        }
    }
}
