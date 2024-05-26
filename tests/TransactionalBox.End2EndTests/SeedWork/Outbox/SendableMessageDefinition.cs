namespace TransactionalBox.End2EndTests.SeedWork.Outbox
{
    internal sealed class SendableMessageDefinition : OutboxMessageDefinition<SendableMessage>
    {
        public SendableMessageDefinition() 
        {
            Receiver = "INBOX";
        }
    }
}
