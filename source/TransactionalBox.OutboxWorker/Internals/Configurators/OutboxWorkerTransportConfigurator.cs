﻿using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.OutboxWorker.Configurators;

namespace TransactionalBox.OutboxWorker.Internals.Configurators
{
    internal sealed class OutboxWorkerTransportConfigurator : IOutboxWorkerTransportConfigurator
    {
        public IServiceCollection Services { get; }

        internal OutboxWorkerTransportConfigurator(IServiceCollection services)
        {
            Services = services;
        }
    }
}
