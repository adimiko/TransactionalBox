using System.Reflection;

namespace TransactionalBox.Inbox.Configurators
{
    public interface IInboxAssemblyConfigurator
    {
        void RegisterFromAssemblies(Assembly assembly);

        void RegisterFromAssemblies(params Assembly[] assemblies);
    }
}
