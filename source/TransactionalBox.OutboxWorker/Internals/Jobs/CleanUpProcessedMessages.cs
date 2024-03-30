using TransactionalBox.BackgroundServiceBase.Internals;
using TransactionalBox.OutboxWorker.Internals.Contracts;

namespace TransactionalBox.OutboxWorker.Internals.Jobs
{
    internal sealed class CleanUpProcessedMessages : Job
    {
        private readonly IOutboxStorage _outboxStorage;

        public CleanUpProcessedMessages(IOutboxStorage outboxStorage) 
        {
            _outboxStorage = outboxStorage;
        }

        protected override async Task Execute(CancellationToken stoppingToken)
        {
            //TODOD batsize, delay when empty, when not full
            await _outboxStorage.RemoveProcessedMessages(5000); //TODO package size
        }
    }
}
