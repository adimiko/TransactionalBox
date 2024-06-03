namespace TransactionalBox.Internals.Outbox.Hooks.Handlers.CleanUpOutbox.Logger
{
    internal interface ICleanUpOutboxLogger
    {
        void CleanedUp(string eventHookHandlerName, Guid hookId, long iteration, int numberOfMessages);
    }
}
