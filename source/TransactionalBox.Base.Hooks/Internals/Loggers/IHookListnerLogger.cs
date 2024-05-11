namespace TransactionalBox.Base.Hooks.Internals.Loggers
{
    internal interface IHookListnerLogger<THook>
        where THook : EventHook, new()
    {
        void Started(string eventHookHandlerName, Guid hookId);

        void Ended(Guid hookId);

        void UnexpectedError(Exception exception);
    }
}
