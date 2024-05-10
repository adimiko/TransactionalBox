namespace TransactionalBox.Outbox.Internals.Hooks
{
    internal interface IHookCaller<T> where T : Hook
    {
        ValueTask CallAsync();
    }
}
