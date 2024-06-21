namespace TransactionalBox.Internals.Inbox.Storage.ContractsToImplement
{
    internal interface IAddMessagesToInboxRepository
    {
        Task<IEnumerable<IdempotentInboxKey>> GetExistIdempotentInboxKeysBasedOn(IEnumerable<InboxMessageStorage> messages);

        Task<AddRangeToInboxStorageResult> AddRange(IEnumerable<InboxMessageStorage> messages, IEnumerable<IdempotentInboxKey> idempotentInboxKeys);
    }
}
