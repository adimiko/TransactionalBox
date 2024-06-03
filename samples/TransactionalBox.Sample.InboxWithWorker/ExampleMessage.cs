
namespace TransactionalBox.Sample.InboxWithWorker
{
    public class ExampleMessage : InboxMessage
    {
        public string Name { get; set; }

        public int Age { get; set; }
    }
}
