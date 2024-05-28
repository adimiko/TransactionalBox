using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.End2EndTests.SeedWork.Inbox;

namespace TransactionalBox.End2EndTests.TestCases
{
    internal class StorageInMemory_TransportInMemory
    {
        private readonly Func<Task<Dependencies>> _init;

        public StorageInMemory_TransportInMemory()
        {
            _init = () =>
            {
                var outboxDependencies = new ServiceCollection();

                outboxDependencies.AddLogging();
                outboxDependencies.AddTransactionalBox(
                    builder => builder.AddOutbox(),
                    settings => settings.ServiceId = "OUTBOX");

                var outbox = outboxDependencies.BuildServiceProvider();

                var inboxDependencies = new ServiceCollection();

                inboxDependencies.AddLogging();
                inboxDependencies.AddTransactionalBox(
                    builder => builder.AddInbox(),
                    settins => settins.ServiceId = "INBOX");

                inboxDependencies.AddSingleton<InboxVerifier>();

                var inbox = inboxDependencies.BuildServiceProvider();

                return Task.FromResult(new Dependencies(outbox, inbox));
            };
        }

        public End2EndTestCase GetEnd2EndTestCase()
        {
            return new End2EndTestCase(_init, "Storage: InMemory | Transport: InMemory");
        }
    }
}
