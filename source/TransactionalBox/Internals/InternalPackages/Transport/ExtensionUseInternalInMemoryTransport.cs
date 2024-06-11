using Microsoft.Extensions.DependencyInjection;

namespace TransactionalBox.Internals.InternalPackages.Transport
{
    internal static class ExtensionUseInternalInMemoryTransport
    {
        internal static void UseInternalInMemoryTransport(this IServiceCollection services)
        {
            services.AddSingleton<IInMemoryTransport, InternalTransport>();
        }
    }
}
