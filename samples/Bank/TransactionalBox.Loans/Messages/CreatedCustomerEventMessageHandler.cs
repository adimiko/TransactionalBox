using TransactionalBox.Loans.Database;
using TransactionalBox.Loans.Models;

namespace TransactionalBox.Loans.Messages
{
    public sealed class CreatedCustomerEventMessageHandler : IInboxHandler<CreatedCustomerEventMessage>
    {
        private readonly LoansDbContext _loansDbContext;

        public CreatedCustomerEventMessageHandler(LoansDbContext loansDbContext)
        {
            _loansDbContext = loansDbContext;
        }

        public async Task Handle(CreatedCustomerEventMessage message, IExecutionContext executionContext)
        {
            var loan = new Loan()
            {
                Id = Guid.NewGuid(),
                CustomerId = message.Id,
                Amount = 0,
            };

            await _loansDbContext.Loans.AddAsync(loan);
            await _loansDbContext.SaveChangesAsync();
        }
    }
}
