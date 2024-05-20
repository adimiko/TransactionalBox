using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Internals.Transport.InMemory.Internals;

namespace TransactionalBox
{
    internal static class ExtensionUseInternalInMemoryTransport
    {
        internal static void UseInternalInMemoryTransport(this IServiceCollection services)
        {
            services.AddSingleton<IInMemoryTransport, InternalTransport>();
        }
    }
}
