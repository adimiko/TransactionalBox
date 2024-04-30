using Microsoft.Extensions.Logging;

namespace TransactionalBox.Outbox.Internals.Loggers
{
    internal interface IOutboxWorkerLogger<TCategoryName>
    {
        void FailedToAddMessagesToTransport();
    }
}
