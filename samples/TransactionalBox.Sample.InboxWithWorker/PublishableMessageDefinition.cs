namespace TransactionalBox.Sample.InboxWithWorker
{
    public sealed class PublishableMessageDefinition : InboxDefinition<PublishableMessage>
    {
        public PublishableMessageDefinition() 
        {
            PublishedBy = "ServiceWithOutbox";
        }
    }
}
