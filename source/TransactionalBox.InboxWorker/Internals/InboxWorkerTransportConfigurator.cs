﻿using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.InboxWorker.Configurators;

namespace TransactionalBox.InboxWorker.Internals
{
    internal sealed class InboxWorkerTransportConfigurator : IInboxWorkerTransportConfigurator
    {
        public IServiceCollection Services { get; }

        internal InboxWorkerTransportConfigurator(IServiceCollection services)
        {
            Services = services;
        }
    }
}
