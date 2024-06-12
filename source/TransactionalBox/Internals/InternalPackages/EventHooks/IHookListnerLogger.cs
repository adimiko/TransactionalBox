namespace TransactionalBox.Internals.InternalPackages.EventHooks
{
    internal interface IHookListnerLogger<THook>
        where THook : EventHook, new()
    {
        void Started(string eventHookHandlerName, Guid hookId);

        void Ended(string eventHookHandlerName, Guid hookId);

        void UnexpectedException(string eventHookHandlerName, Guid hookId, long attempt, long msDelay, Exception exception);

        void UnexpectedException(Exception exception);
    }
}
