using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using TransactionalBox.End2EndTests.SeedWork.Inbox;
using TransactionalBox.End2EndTests.SeedWork.Outbox;
using TransactionalBox.End2EndTests.TestCases;
using Xunit;

namespace TransactionalBox.End2EndTests
{
    public sealed class End2EndTests
    {
        private readonly Assembly _assembly = typeof(End2EndTests).Assembly;

        // TODO Action multiple implementation ef, inmemory, mongodb, transport etc.
        // TODO multiple test container per test
        //TODO xunit logger


        [Theory]
        [ClassData(typeof(Tests))]
        public async Task Test(End2EndTestCase testCase)
        {
            var dependencies = await testCase.Init();

            var outboxDependencies = dependencies.OutboxDependecies;
            var inboxDependencies = dependencies.InboxDependecies;

            var outboxHostedServices = outboxDependencies.GetServices<IHostedService>();

            foreach (var outboxHostedService in outboxHostedServices)
            {
                await outboxHostedService.StartAsync(CancellationToken.None);
            }

            var inboxHostedServices = inboxDependencies.GetServices<IHostedService>();

            foreach (var inboxHostedService in inboxHostedServices)
            {
                await inboxHostedService.StartAsync(CancellationToken.None);
            }

            using (var scope = outboxDependencies.CreateScope()) 
            {
                var outboxMessage = new SeedWork.Outbox.SendableMessage() { Message  = "Hello" };

                var outbox = scope.ServiceProvider.GetRequiredService<IOutbox>();
                // UoW
                await outbox.Add(outboxMessage);
            }

            await Task.Delay(500);

            var verifier = inboxDependencies.GetRequiredService<InboxVerifier>();

            Assert.True(verifier.IsExecuted);
        }
    }
}
