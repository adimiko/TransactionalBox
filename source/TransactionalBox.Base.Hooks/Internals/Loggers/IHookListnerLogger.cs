namespace TransactionalBox.Base.Hooks.Internals.Loggers
{
    internal interface IHookListnerLogger
    {
        void Started(string hookName, Guid hookId);

        void Ended(Guid hookId);
    }
}
