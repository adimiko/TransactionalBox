namespace TransactionalBox.Base.Hooks
{
    internal interface IHookCaller<T> 
        where T : Hook, new()
    {
        ValueTask CallAsync();
    }
}
