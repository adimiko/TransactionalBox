namespace TransactionalBox.Internals.InternalPackages.EventHooks
{
    internal interface IEventHookPublisher
    {
        Task PublishAsync<TEventHook>() where TEventHook : EventHook, new();
    }
}
