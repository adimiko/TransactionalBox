namespace TransactionalBox.Internals.InternalPackages.EventHooks
{
    internal interface IInternalHookListenersLauncher
    {
        Task LaunchAsync(CancellationToken cancellationToken);
    }
}
