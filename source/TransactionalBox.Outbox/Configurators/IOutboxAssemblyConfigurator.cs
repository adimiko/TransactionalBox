using System.Reflection;

namespace TransactionalBox.Outbox.Configurators
{
    public interface IOutboxAssemblyConfigurator
    {
        void RegisterFromAssemblies(Assembly assembly);

        void RegisterFromAssemblies(params Assembly[] assemblies);
    }
}
