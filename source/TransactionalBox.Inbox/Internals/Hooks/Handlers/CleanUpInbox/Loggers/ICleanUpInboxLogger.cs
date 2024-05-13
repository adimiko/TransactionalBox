namespace TransactionalBox.Inbox.Internals.Hooks.Handlers.CleanUpInbox.Loggers
{
    internal interface ICleanUpInboxLogger
    {
        void CleanedUp(string eventHookHandlerName, Guid hookId, long iteration, int numberOfMessages);
    }
}
