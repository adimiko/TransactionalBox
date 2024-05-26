namespace TransactionalBox.End2EndTests.SeedWork.Inbox
{
    internal sealed class SendableMessage : InboxMessage
    {
        public string Message { get; init; }
    }
}
