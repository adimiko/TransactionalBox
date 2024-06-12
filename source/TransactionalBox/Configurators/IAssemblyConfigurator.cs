using System.Reflection;

namespace TransactionalBox.Configurators
{
    public interface IAssemblyConfigurator
    {
        void RegisterFromAssemblies(Assembly assembly);

        void RegisterFromAssemblies(params Assembly[] assemblies);
    }
}
