namespace TransactionalBox.Inbox.Internals
{
    internal interface IInboxMessageTypes
    {
        IReadOnlyDictionary<string, Type> DictionaryMessageTypes { get; }

        IEnumerable<Type> MessageTypes { get; }
    }
}
