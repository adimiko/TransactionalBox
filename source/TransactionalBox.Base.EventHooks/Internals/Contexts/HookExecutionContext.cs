namespace TransactionalBox.Base.EventHooks.Internals.Contexts
{
    internal sealed record HookExecutionContext(Guid Id, string Name, DateTime LastOccurredUtc) : IHookExecutionContext;
}
