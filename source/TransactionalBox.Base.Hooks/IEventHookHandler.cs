namespace TransactionalBox.Base.Hooks
{
    internal interface IEventHookHandler<T>
        where T : EventHook, new()
    {
        Task HandleAsync(IHookExecutionContext context, CancellationToken cancellationToken);
    }
}
