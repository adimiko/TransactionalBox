namespace TransactionalBox.Base.Hooks
{
    internal interface IHookCaller<T> where T : Hook
    {
        ValueTask CallAsync();
    }
}
