using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactionalBox.OutboxWorker.Configurators;

namespace TransactionalBox.OutboxWorker.Internals
{
    internal sealed class OutboxWorkerStorageConfigurator : IOutboxWorkerStorageConfigurator
    {
        public IServiceCollection Services { get; }

        internal OutboxWorkerStorageConfigurator(IServiceCollection services)
        {
            Services = services;
        }
    }
}
