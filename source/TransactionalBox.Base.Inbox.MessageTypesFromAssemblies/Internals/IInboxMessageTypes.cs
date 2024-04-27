namespace TransactionalBox.Base.Inbox.MessageTypesFromAssemblies.Internals
{
    internal interface IInboxMessageTypes
    {
        IReadOnlyDictionary<string, Type> Types { get; }
    }
}
