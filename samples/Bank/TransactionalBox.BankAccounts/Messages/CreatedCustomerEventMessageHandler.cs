using TransactionalBox.BankAccounts.Database;
using TransactionalBox.BankAccounts.Models;

namespace TransactionalBox.BankAccounts.Messages
{
    public sealed class CreatedCustomerEventMessageHandler : IInboxHandler<CreatedCustomerEventMessage>
    {
        private readonly BankAccountsDbContext _bankAccountsDbContext;

        public CreatedCustomerEventMessageHandler(BankAccountsDbContext bankAccountsDbContext) 
        {
            _bankAccountsDbContext = bankAccountsDbContext;
        }

        public async Task Handle(CreatedCustomerEventMessage message, IExecutionContext executionContext)
        {
            var bankAccount = new BankAccount()
            {
                Id = Guid.NewGuid(),
                CustomerId = message.Id,
                Balance = 100,
            };

            await _bankAccountsDbContext.BankAccounts.AddAsync(bankAccount);
            await _bankAccountsDbContext.SaveChangesAsync();
        }
    }
}
