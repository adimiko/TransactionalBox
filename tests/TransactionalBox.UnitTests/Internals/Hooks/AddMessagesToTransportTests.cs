using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactionalBox.Internals.InternalPackages.EventHooks;
using TransactionalBox.Internals.Outbox.Hooks.Events;
using TransactionalBox.Internals.Outbox.Hooks.Handlers.AddMessagesToTransport;
using Xunit;

namespace TransactionalBox.UnitTests.Internals.Hooks
{
    public sealed class AddMessagesToTransportTests
    {
        private readonly CancellationToken _cancellationToken = CancellationToken.None;

        private readonly IServiceProvider _serviceProvider;

        public AddMessagesToTransportTests() 
        {
            IServiceCollection services = new ServiceCollection();

            services.AddTransactionalBox(builder =>
            {
                builder.AddOutbox();
            }, settings => settings.ServiceId = "TEST");

            //TODO replace implementation mock for transport nad storage

            _serviceProvider = services.BuildServiceProvider();
        }

        [Fact]
        public async Task Test()
        {
            var addMessagesToTransport = _serviceProvider.GetRequiredService<AddMessagesToTransport>();

            var hookContext = new HookExecutionContext(Guid.NewGuid(), "AddMessagesToTransport", DateTime.UtcNow, false, 0);

            await addMessagesToTransport.HandleAsync(hookContext, _cancellationToken);
        }
    }
}
