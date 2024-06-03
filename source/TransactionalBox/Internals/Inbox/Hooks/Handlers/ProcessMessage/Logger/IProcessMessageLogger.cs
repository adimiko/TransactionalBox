namespace TransactionalBox.Internals.Inbox.Hooks.Handlers.ProcessMessage.Logger
{
    internal interface IProcessMessageLogger
    {
        void Processed(string eventHookHandlerName, Guid hookId, Guid messageId);
    }
}
