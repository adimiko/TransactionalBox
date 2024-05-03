namespace TransactionalBox.Inbox.Internals.Assemblies.MessageTypes
{
    internal interface IInboxMessageTypes
    {
        IReadOnlyDictionary<string, Type> DictionaryMessageTypes { get; }

        IEnumerable<Type> MessageTypes { get; }
    }
}
