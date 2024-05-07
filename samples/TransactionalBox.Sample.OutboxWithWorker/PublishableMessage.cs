using TransactionalBox.Outbox;

namespace TransactionalBox.Sample.OutboxWithWorker
{
    public class PublishableMessage : OutboxMessage
    {
        public string Name { get; set; } = "Adrian";

        public int Age { get; set; } = 25;
    }
}
