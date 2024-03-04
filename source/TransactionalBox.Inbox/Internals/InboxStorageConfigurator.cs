﻿using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Inbox.Configurators;

namespace TransactionalBox.Inbox.Internals
{
    internal sealed class InboxStorageConfigurator : IInboxStorageConfigurator
    {
        public IServiceCollection Services { get; }

        internal InboxStorageConfigurator(IServiceCollection services) 
        {
            Services = services;
        }
    }
}
