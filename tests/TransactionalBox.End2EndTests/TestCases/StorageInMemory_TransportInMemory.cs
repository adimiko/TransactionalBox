using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.End2EndTests.SeedWork.Inbox;

namespace TransactionalBox.End2EndTests.TestCases
{
    internal class StorageInMemory_TransportInMemory
    {
        private readonly IServiceProvider _outboxDependencies;

        private readonly IServiceProvider _inboxDependencies;

        public StorageInMemory_TransportInMemory()
        {
            var outboxDependencies = new ServiceCollection();

            outboxDependencies.AddLogging();
            outboxDependencies.AddTransactionalBox(
                builder => builder.AddOutbox(),
                settings => settings.ServiceId = "OUTBOX");

            _outboxDependencies = outboxDependencies.BuildServiceProvider();

            var inboxDependencies = new ServiceCollection();

            inboxDependencies.AddLogging();
            inboxDependencies.AddTransactionalBox(
                builder => builder.AddInbox(),
                settins => settins.ServiceId = "INBOX");

            inboxDependencies.AddSingleton<InboxVerifier>();

            _inboxDependencies = inboxDependencies.BuildServiceProvider();
        }

        public End2EndTestCase GetEnd2EndTestCase()
        {
            return new End2EndTestCase(_outboxDependencies, _inboxDependencies, "Storage: InMemory | Transport: InMemory");
        }
    }
}
