using Microsoft.Extensions.DependencyInjection;
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

            outboxDependencies.AddTransactionalBox(
                builder => builder.AddOutbox(
                    assemblyConfiguraton: assebmly => assebmly.RegisterFromAssemblies(_assembly)),
                settings => settings.ServiceId = "OUTBOX");

            _outboxDependencies = outboxDependencies.BuildServiceProvider();

            var inboxDependencies  = new ServiceCollection();

            inboxDependencies.AddTransactionalBox(
                builder => builder.AddInbox(
                    assemblyConfiguraton: assebmly => assebmly.RegisterFromAssemblies(_assembly)),
                settins => settins.ServiceId = "INBOX");

            _inboxDependencies = inboxDependencies.BuildServiceProvider();
        }

        [Fact]
        public async Task Test()
        {
            using (var scope = _outboxDependencies.CreateScope()) 
            {
                var outboxMessage = new SendableMessage() { Message  = "Hello" };

                var outbox = scope.ServiceProvider.GetRequiredService<IOutbox>();
                // UoW
                await outbox.Add(outboxMessage);
            }

            await Task.Delay(5000);

            // add to outbox

            // inbox decorate handler end check
        }
    }
}
