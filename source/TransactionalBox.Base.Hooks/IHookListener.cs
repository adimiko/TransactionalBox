namespace TransactionalBox.Base.Hooks
{
    internal interface IHookListener<T>
        where T : Hook, new()
    {
        Task ListenAsync(IHookExecutionContext context, CancellationToken cancellationToken);
    }
}
