namespace TransactionalBox.Internals.EventHooks
{
    internal interface IEventHookPublisher
    {
        Task PublishAsync<TEventHook>() where TEventHook : EventHook, new();
    }
}
