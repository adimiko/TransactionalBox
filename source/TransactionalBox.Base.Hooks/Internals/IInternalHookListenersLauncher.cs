namespace TransactionalBox.Base.Hooks.Internals
{
    internal interface IInternalHookListenersLauncher
    {
        Task LaunchAsync(CancellationToken cancellationToken);
    }
}
