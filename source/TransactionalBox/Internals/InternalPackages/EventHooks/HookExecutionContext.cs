namespace TransactionalBox.Internals.InternalPackages.EventHooks
{
    internal sealed record HookExecutionContext(Guid Id, string Name, DateTime LastOccurredUtc, bool IsError, long Attempt) : IHookExecutionContext;
}
