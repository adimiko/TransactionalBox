namespace TransactionalBox.Outbox.Internals.Hooks.Handlers.CleanUpOutbox.Loggers
{
    internal interface ICleanUpOutboxLogger
    {
        void CleanedUp(string eventHookHandlerName, Guid hookId, long iteration, int numberOfMessages);
    }
}
