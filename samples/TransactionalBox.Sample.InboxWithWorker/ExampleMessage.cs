using TransactionalBox.Inbox;

namespace TransactionalBox.Sample.InboxWithWorker
{
    public class ExampleMessage : IInboxMessage
    {
        public string Name { get; set; }

        public int Age { get; set; }
    }
}
