namespace TransactionalBox.Internals.EventHooks.Internals
{
    internal interface IInternalHookListenersLauncher
    {
        Task LaunchAsync(CancellationToken cancellationToken);
    }
}
