using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using TransactionalBox.End2EndTests.SeedWork.Outbox;
using Xunit;

namespace TransactionalBox.End2EndTests
{
    public sealed class End2EndTests
    {
        private readonly Assembly _assembly = typeof(End2EndTests).Assembly;

        // TODO Action multiple implementation ef, inmemory, mongodb, transport etc.
        // TODO multiple test container per test
        private readonly IServiceProvider _outboxDependencies;

        private readonly IServiceProvider _inboxDependencies;

        public End2EndTests()
        {
            var outboxDependencies = new ServiceCollection();

            outboxDependencies.AddLogging();

            outboxDependencies.AddTransactionalBox(
                builder => builder.AddOutbox(
                    assemblyConfiguraton: assebmly => assebmly.RegisterFromAssemblies(_assembly)),
                settings => settings.ServiceId = "OUTBOX");

            _outboxDependencies = outboxDependencies.BuildServiceProvider();

            var inboxDependencies  = new ServiceCollection();

            inboxDependencies.AddLogging();

            inboxDependencies.AddTransactionalBox(
                builder => builder.AddInbox(
                    assemblyConfiguraton: assebmly => assebmly.RegisterFromAssemblies(_assembly)),
                settins => settins.ServiceId = "INBOX");

            _inboxDependencies = inboxDependencies.BuildServiceProvider();
        }

        [Fact]
        public async Task Test()
        {
            var outboxHostedServices = _outboxDependencies.GetServices<IHostedService>();

            foreach (var outboxHostedService in outboxHostedServices)
            {
                await outboxHostedService.StartAsync(CancellationToken.None);
            }

            var inboxHostedServices = _inboxDependencies.GetServices<IHostedService>();

            foreach (var inboxHostedService in inboxHostedServices)
            {
                await inboxHostedService.StartAsync(CancellationToken.None);
            }

            using (var scope = _outboxDependencies.CreateScope()) 
            {
                var outboxMessage = new SendableMessage() { Message  = "Hello" };

                var outbox = scope.ServiceProvider.GetRequiredService<IOutbox>();
                // UoW
                await outbox.Add(outboxMessage);
            }

            await Task.Delay(5000);

            // inbox decorate handler end check
        }
    }
}
