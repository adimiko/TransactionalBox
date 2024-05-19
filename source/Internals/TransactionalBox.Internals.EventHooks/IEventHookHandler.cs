using TransactionalBox.Internals.EventHooks.Contexts;

namespace TransactionalBox.Internals.EventHooks
{
    internal interface IEventHookHandler<T>
        where T : EventHook, new()
    {
        Task HandleAsync(IHookExecutionContext context, CancellationToken cancellationToken);
    }
}
