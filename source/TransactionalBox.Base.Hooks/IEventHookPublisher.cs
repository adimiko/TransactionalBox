namespace TransactionalBox.Base.Hooks
{
    internal interface IEventHookPublisher
    {
        Task PublishAsync<TEventHook>() where TEventHook : EventHook, new();
    }
}
