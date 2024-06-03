using System.Reflection;

namespace TransactionalBox.Configurators.Inbox
{
    public interface IInboxAssemblyConfigurator
    {
        void RegisterFromAssemblies(Assembly assembly);

        void RegisterFromAssemblies(params Assembly[] assemblies);
    }
}
