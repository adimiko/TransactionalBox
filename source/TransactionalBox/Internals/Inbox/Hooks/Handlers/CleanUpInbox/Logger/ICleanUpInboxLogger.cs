namespace TransactionalBox.Internals.Inbox.Hooks.Handlers.CleanUpInbox.Logger
{
    internal interface ICleanUpInboxLogger
    {
        void CleanedUp(string eventHookHandlerName, Guid hookId, long iteration, int numberOfMessages);
    }
}
