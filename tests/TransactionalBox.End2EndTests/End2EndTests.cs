using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using TransactionalBox.End2EndTests.SeedWork.Inbox;
using TransactionalBox.End2EndTests.SeedWork.Outbox;
using TransactionalBox.End2EndTests.TestCases;
using Xunit;
using Xunit.Abstractions;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace TransactionalBox.End2EndTests
{
    public sealed class End2EndTests
    {
        private readonly ITestOutputHelper _output;

        public End2EndTests(ITestOutputHelper output) 
        {
            _output = output;
        }
        // TODO Action multiple implementation ef, inmemory, mongodb, transport etc.
        // TODO multiple test container per test
        //TODO xunit logger


        [Theory]
        [ClassData(typeof(Tests))]
        public async Task Test(End2EndTestCase testCase)
        {
            var dependencies = await testCase.Init(_output).ConfigureAwait(false);

            var outboxDependencies = dependencies.OutboxDependecies;
            var inboxDependencies = dependencies.InboxDependecies;

            var outboxHostedServices = outboxDependencies.GetServices<IHostedService>();

            foreach (var outboxHostedService in outboxHostedServices)
            {
                await outboxHostedService.StartAsync(CancellationToken.None).ConfigureAwait(false); ;
            }

            var inboxHostedServices = inboxDependencies.GetServices<IHostedService>();

            foreach (var inboxHostedService in inboxHostedServices)
            {
                await inboxHostedService.StartAsync(CancellationToken.None).ConfigureAwait(false); ;
            }

            using (var scope = outboxDependencies.CreateScope()) 
            {
                var outboxMessage = new SeedWork.Outbox.SendableMessage() { Message  = "Hello" };

                var outbox = scope.ServiceProvider.GetRequiredService<IOutbox>();
                var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                await using (await uow.BeginTransactionAsync().ConfigureAwait(false))
                {
                    await outbox.Add(outboxMessage).ConfigureAwait(false);
                }
            }

            await Task.Delay(5000).ConfigureAwait(false);

            using (var scope = inboxDependencies.CreateScope())
            {
                var verifier = scope.ServiceProvider.GetRequiredService<InboxVerifier>();

                Assert.True(verifier.IsExecuted);
            }
            /*
            foreach (var outboxHostedService in outboxHostedServices)
            {
                await outboxHostedService.StopAsync(CancellationToken.None).ConfigureAwait(false);
            }

            foreach (var inboxHostedService in inboxHostedServices)
            {
                await inboxHostedService.StopAsync(CancellationToken.None).ConfigureAwait(false);
            }
            */
            await testCase.CleanUp();
        }
    }
}
