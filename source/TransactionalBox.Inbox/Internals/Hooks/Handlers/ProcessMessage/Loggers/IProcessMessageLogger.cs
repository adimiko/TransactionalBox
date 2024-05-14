namespace TransactionalBox.Inbox.Internals.Hooks.Handlers.ProcessMessage.Loggers
{
    internal interface IProcessMessageLogger
    {
        void Processed(string eventHookHandlerName, Guid hookId, Guid messageId);
    }
}
