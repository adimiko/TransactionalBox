using TransactionalBox.Outbox;

namespace TransactionalBox.Sample.WebApi.InMemory.ServiceWithOutbox
{
    public sealed class ExampleServiceWithOutbox
    {
        private readonly IOutbox _outbox;

        public ExampleServiceWithOutbox(IOutbox outbox)
        {
            _outbox = outbox;
        }

        public async Task Execute()
        {
            var message = new ExampleMessage()
            {
                Name = "Name",
                Age = 25,
            };

            await _outbox.Add(message, envelope => envelope.Receiver = "ServiceName");
        }
    }
}
