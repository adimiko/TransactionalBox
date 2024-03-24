using Microsoft.Extensions.DependencyInjection;

namespace TransactionalBox.Builders
{
    public interface ITransactionalBoxBuilder
    {
        internal IServiceCollection Services { get; }
    }
}
