namespace TransactionalBox.Sample.InboxWithWorker
{
    public sealed class PublishableMessageDefinition : InboxMessageDefinition<PublishableMessage>
    {
        public PublishableMessageDefinition() 
        {
            PublishedBy = "ServiceWithOutbox";
        }
    }
}
