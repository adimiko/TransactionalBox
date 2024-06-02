namespace TransactionalBox.BankAccounts.Models
{
    public class BankAccount
    {
        public Guid Id { get; set; }

        public Guid CustomerId { get; set; }

        public decimal Balance { get; set; } = 100;
    }
}
