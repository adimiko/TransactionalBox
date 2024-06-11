namespace TransactionalBox.Internals.InternalPackages.EventHooks
{
    internal interface IEventHookHandler<T>
        where T : EventHook, new()
    {
        Task HandleAsync(IHookExecutionContext context, CancellationToken cancellationToken);
    }
}
