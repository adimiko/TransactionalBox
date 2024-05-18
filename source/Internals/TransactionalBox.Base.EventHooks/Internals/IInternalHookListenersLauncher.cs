namespace TransactionalBox.Base.EventHooks.Internals
{
    internal interface IInternalHookListenersLauncher
    {
        Task LaunchAsync(CancellationToken cancellationToken);
    }
}
