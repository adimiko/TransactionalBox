using TransactionalBox.InboxBase.StorageModel;

namespace TransactionalBox.InboxWorker.Internals
{
    public interface IInboxStorage
    {
        Task AddRange(IEnumerable<InboxMessage> messages, DateTime nowUtc);
    }
}
