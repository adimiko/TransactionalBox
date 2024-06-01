using TransactionalBox.Customers.Database;
using TransactionalBox.Customers.Models;
using TransactionalBox.Inbox.Contexts;

namespace TransactionalBox.Customers.Messages
{
    public sealed class CreateCustomerCommandMessageHandler : IInboxHandler<CreateCustomerCommandMessage>
    {
        private readonly IOutbox _outbox;

        private readonly CustomersDbContext _customersDbContext;

        private readonly IUnitOfWork _unitOfWork;

        public CreateCustomerCommandMessageHandler(
            IOutbox outbox,
            IUnitOfWork unitOfWork,
            CustomersDbContext customersDbContext) 
        {
            _outbox = outbox;
            _unitOfWork = unitOfWork;
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

            await using(await _unitOfWork.BeginTransactionAsync()) 
            {
                await _customersDbContext.Customers.AddAsync(customer);
                await _outbox.Add(@event, e => e.CorrelationId = executionContext.CorrelationId);
            }
        }
    }
}
