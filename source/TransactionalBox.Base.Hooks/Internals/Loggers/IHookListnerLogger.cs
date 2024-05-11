namespace TransactionalBox.Base.Hooks.Internals.Loggers
{
    internal interface IHookListnerLogger<THook>
        where THook : Hook, new()
    {
        void Started(string hookName, Guid hookId);

        void Ended(Guid hookId);

        void UnexpectedError(Exception exception);
    }
}
