namespace TransactionalBox.Sample.InboxWithWorker
{
    public class PublishableMessage : InboxMessage
    {
        public string Name { get; init; }

        public int Age { get; init; }
    }
}
