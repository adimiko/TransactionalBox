using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using TransactionalBox.End2EndTests.SeedWork.Inbox;
using TransactionalBox.End2EndTests.SeedWork.Outbox;
using TransactionalBox.End2EndTests.TestCases;
using Xunit;
using Xunit.Abstractions;

namespace TransactionalBox.End2EndTests
{
    [Collection("Sequential")]
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

            using (var scope = outboxDependencies.CreateScope()) 
            {
                var outboxMessage = new SeedWork.Outbox.SendableMessage() { Message  = "Hello" };

                var outbox = scope.ServiceProvider.GetRequiredService<IOutbox>();
                var dbContext = scope.ServiceProvider.GetRequiredService<DbContext>();

                await outbox.Add(outboxMessage).ConfigureAwait(false);
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
                await outbox.TransactionCommited().ConfigureAwait(false);
            }

            await Task.Delay(1000).ConfigureAwait(false);

            using (var scope = inboxDependencies.CreateScope())
            {
                var verifier = scope.ServiceProvider.GetRequiredService<InboxVerifier>();

                Assert.True(verifier.IsExecuted);
            }

            await testCase.CleanUp().ConfigureAwait(false);
        }
    }
}
