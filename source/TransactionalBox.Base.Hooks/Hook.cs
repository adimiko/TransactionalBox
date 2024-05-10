namespace TransactionalBox.Base.Hooks
{
    internal abstract class Hook
    {
        protected internal abstract Task StartAsync(CancellationToken cancellationToken);
    }
}
