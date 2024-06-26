﻿using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Internals.Outbox.Transport.ContractsToImplement;
using TransactionalBox.Internals.Outbox.Transport.InMemory;
using TransactionalBox.Configurators.Outbox;
using TransactionalBox.Internals.InternalPackages.Transport;

namespace TransactionalBox.Internals.Outbox.Extensions
{
    internal static class OutboxExtensionUseInMemoryTransport
    {
        internal static void UseInMemoryTransport(this IOutboxTransportConfigurator configurator)
        {
            var services = configurator.Services;

            services.UseInternalInMemoryTransport();
            services.AddSingleton<IOutboxTransport, InMemoryOutboxTransport>();
            services.AddSingleton<ITransportMessageSizeSettings>(new InMemoryTransportMessageSizeSettings());
        }
    }
}
