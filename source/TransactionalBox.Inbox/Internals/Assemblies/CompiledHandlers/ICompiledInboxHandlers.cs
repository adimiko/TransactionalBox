namespace TransactionalBox.Inbox.Internals.Assemblies.CompiledHandlers
{
    internal interface ICompiledInboxHandlers
    {
        Func<object, object, IExecutionContext, Task>? GetCompiledInboxHandler(Type messageType);
    }
}
