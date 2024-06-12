using System.Reflection;
using TransactionalBox.Configurators;

namespace TransactionalBox.Internals.InternalPackages.AssemblyConfigurator
{
    internal sealed class AssemblyConfigurator : IAssemblyConfigurator
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
