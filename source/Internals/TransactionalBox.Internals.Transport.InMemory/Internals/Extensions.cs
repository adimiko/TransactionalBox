using Microsoft.Extensions.DependencyInjection;

namespace TransactionalBox.Internals.Transport.InMemory.Internals
{
    internal static class Extensions
    {
        internal static void UseInternalInMemoryTransport(this IServiceCollection services)
        {
            services.AddSingleton<IInMemoryTransport, InternalTransport>();
        }
    }
}
