using TransactionalBox.Internals.EventHooks.Contexts;

namespace TransactionalBox.Internals.EventHooks.Internals.Contexts
{
    internal sealed record HookExecutionContext(Guid Id, string Name, DateTime LastOccurredUtc, bool IsError, long Attempt) : IHookExecutionContext;
}
