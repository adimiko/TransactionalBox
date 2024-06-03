namespace TransactionalBox.Internals.Outbox.Hooks.Handlers.AddMessagesToTransport.Logger
{
    internal interface IAddMessagesToTransportLogger
    {
        void Added(string eventHookHandlerName, Guid hookId, long iteration, int numberOfMessages, int maxBatchSize);
    }
}
