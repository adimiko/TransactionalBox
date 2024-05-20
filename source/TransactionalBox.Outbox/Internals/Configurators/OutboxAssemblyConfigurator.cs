using System.Reflection;
using TransactionalBox.Outbox.Configurators;

namespace TransactionalBox.Outbox.Internals.Configurators
{
    //TODO the same configurator is in Inbox (maybe base class)
    internal sealed class OutboxAssemblyConfigurator : IOutboxAssemblyConfigurator
    {
        private readonly ISet<Assembly> _assemblies = new HashSet<Assembly>();

        internal IEnumerable<Assembly> Assemblies
        {
            get
            {
                if (_assemblies.Any())
                {
                    return _assemblies;
                }

                var assemblies = AppDomain.CurrentDomain.GetAssemblies();

                foreach (var assembly in assemblies)
                {
                    _assemblies.Add(assembly);
                }

                return _assemblies;
            }
        }

        public void RegisterFromAssemblies(Assembly assembly)
        {
            _assemblies.Add(assembly);
        }

        public void RegisterFromAssemblies(params Assembly[] assemblies)
        {
            foreach (Assembly assembly in assemblies)
            {
                _assemblies.Add(assembly);
            }
        }
    }
}
