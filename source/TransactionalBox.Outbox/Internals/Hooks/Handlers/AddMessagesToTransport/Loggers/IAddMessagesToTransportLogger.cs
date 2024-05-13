namespace TransactionalBox.Outbox.Internals.Hooks.Handlers.AddMessagesToTransport.Loggers
{
    internal interface IAddMessagesToTransportLogger
    {
        void Added(string eventHookHandlerName, Guid hookId, long iteration, int numberOfMessages, int maxBatchSize);
    }
}
