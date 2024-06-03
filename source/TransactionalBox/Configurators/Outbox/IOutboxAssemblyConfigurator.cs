using System.Reflection;

namespace TransactionalBox.Configurators.Outbox
{
    public interface IOutboxAssemblyConfigurator
    {
        void RegisterFromAssemblies(Assembly assembly);

        void RegisterFromAssemblies(params Assembly[] assemblies);
    }
}
