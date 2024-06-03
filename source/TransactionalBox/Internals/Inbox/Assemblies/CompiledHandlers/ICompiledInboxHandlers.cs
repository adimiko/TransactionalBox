namespace TransactionalBox.Internals.Inbox.Assemblies.CompiledHandlers
{
    internal interface ICompiledInboxHandlers
    {
        Func<object, object, IExecutionContext, Task>? GetCompiledInboxHandler(Type messageType);
    }
}
