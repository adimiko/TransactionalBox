namespace TransactionalBox.Inbox.Internals
{
    internal interface IInboxMessageTypes
    {
        IReadOnlyDictionary<string, Type> Types { get; }
    }
}
