﻿using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Inbox.Configurators;
using TransactionalBox.InboxWorker.Internals.Contracts;
using TransactionalBox.OutboxWorker.Configurators;
using TransactionalBox.OutboxWorker.Internals.Contracts;

namespace TransactionalBox.Base.Transport.InMemory.Internals
{
    internal static class Extensions
    {
        internal static void UseInternalInMemory(this IOutboxWorkerTransportConfigurator configurator)
        {
            var services = configurator.Services;

            services.AddSingleton<IOutboxWorkerTransport, InternalTransport>();
            services.AddSingleton<ITransportMessageSizeSettings>(new InMemoryTransportMessageSizeSettings());
        }

        internal static void UseInternalInMemory(this IInboxTransportConfigurator configurator)
        {
            var services = configurator.Services;

            services.AddSingleton<IInboxWorkerTransport, InternalTransport>();
            services.AddSingleton<ITransportTopicWithWildCard, InMemoryTransportTopicWithWildCard>();
        }
    }
}
