using TransactionalBox.Inbox;

namespace TransactionalBox.Sample.InboxWithWorker
{
    [PublishedBy("ServiceWithOutbox")]
    public class PublishableMessage : IInboxMessage
    {
        public string Name { get; init; }

        public int Age { get; init; }
    }
}
