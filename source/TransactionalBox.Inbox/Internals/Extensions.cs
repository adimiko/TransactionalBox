using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Inbox.Configurators;
using TransactionalBox.Inbox.Decompression;
using TransactionalBox.Inbox.Deserialization;
using TransactionalBox.Inbox.Internals.Decompression;
using TransactionalBox.Inbox.Internals.Deserializers;

namespace TransactionalBox.Inbox.Internals
{
    internal static class Extensions
    {
        internal static void UseNoDecompression(
            this IInboxDecompressionAlgorithmConfigurator configurator)
        {
            var services = configurator.Services;

            services.AddSingleton<IDecompressionAlgorithm, NoDecompression>();
        }

        internal static void UseSystemTextJson(this IInboxDeserializationConfigurator configurator)
        {
            var services = configurator.Services;

            services.AddSingleton<IInboxDeserializer, InboxDeserializer>();
        }
    }
}
