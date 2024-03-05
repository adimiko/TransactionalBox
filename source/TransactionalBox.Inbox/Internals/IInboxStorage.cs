namespace TransactionalBox.Inbox.Internals
{
    public interface IInboxStorage
    {
        Task<string> GetMessage();
    }
}
