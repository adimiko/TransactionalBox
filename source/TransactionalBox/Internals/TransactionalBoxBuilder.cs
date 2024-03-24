using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Builders;

namespace TransactionalBox.Internals
{
    internal sealed class TransactionalBoxBuilder : ITransactionalBoxBuilder
    {
        public IServiceCollection Services { get; }

        internal TransactionalBoxBuilder(IServiceCollection services)
        {
            Services = services;
        }
    }
}
