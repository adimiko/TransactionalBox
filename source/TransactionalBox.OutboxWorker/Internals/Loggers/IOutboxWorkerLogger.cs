using Microsoft.Extensions.Logging;

namespace TransactionalBox.OutboxWorker.Internals.Loggers
{
    internal interface IOutboxWorkerLogger<TCategoryName>
    {
        void FailedToAddMessagesToTransport();
    }
}
