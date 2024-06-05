namespace TransactionalBox.End2EndTests.SeedWork.Outbox
{
    internal sealed class SendableMessageDefinition : OutboxDefinition<SendableMessage>
    {
        public SendableMessageDefinition() 
        {
            Receiver = "INBOX";
        }
    }
}
