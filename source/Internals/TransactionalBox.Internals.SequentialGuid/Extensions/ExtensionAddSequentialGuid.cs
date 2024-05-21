using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Internals.SequentialGuid;
using TransactionalBox.Internals.SequentialGuid.Internals;

namespace TransactionalBox
{
    internal static class ExtensionAddSequentialGuid
    {
        internal static void AddSequentialGuid(this IServiceCollection services, SequentialGuidType sequentialGuidType)
        {
            services.AddSingleton<ISequentialGuidGenerator>(new SequentialGuid(sequentialGuidType));
        }
    }
}
