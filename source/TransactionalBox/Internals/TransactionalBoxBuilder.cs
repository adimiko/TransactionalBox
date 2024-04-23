using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Builders;

namespace TransactionalBox.Internals
{
    internal sealed class TransactionalBoxBuilder : ITransactionalBoxBuilder
    {
        public IServiceCollection Services { get; }

        public IConfiguration Configuration { get; }

        internal TransactionalBoxBuilder(
            IServiceCollection services,
            IConfiguration configuration)
        {
            Services = services;
            
            if (configuration is not null)
            {
                Configuration = configuration;
            }
            else
            {
                Configuration = new ConfigurationBuilder().Build();
            }
        }
    }
}
