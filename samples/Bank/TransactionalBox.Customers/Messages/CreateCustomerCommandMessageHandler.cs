using TransactionalBox.Customers.Database;
using TransactionalBox.Customers.Models;

namespace TransactionalBox.Customers.Messages
{
    public sealed class CreateCustomerCommandMessageHandler : IInboxHandler<CreateCustomerCommandMessage>
    {
        private readonly IOutbox _outbox;

        private readonly CustomersDbContext _customersDbContext;

        public CreateCustomerCommandMessageHandler(
            IOutbox outbox,
            CustomersDbContext customersDbContext) 
        {
            _outbox = outbox;
            _customersDbContext = customersDbContext;
        }

        public async Task Handle(CreateCustomerCommandMessage message, IExecutionContext executionContext)
        {
            var customer = new Customer()
            {
                Id = message.Id,
                FirstName = message.FirstName,
                LastName = message.LastName,
                Age = message.Age,
                CreatedAtUtc = DateTime.UtcNow,
            };

            var @event = new CreatedCustomerEventMessage(){ Id = message.Id };

            await _customersDbContext.Customers.AddAsync(customer);
            await _outbox.Add(@event, e => e.CorrelationId = executionContext.CorrelationId);

            await _customersDbContext.SaveChangesAsync();
            await _outbox.TransactionCommited();
        }
    }
}
