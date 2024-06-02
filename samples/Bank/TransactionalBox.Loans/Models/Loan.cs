namespace TransactionalBox.Loans.Models
{
    public class Loan
    {
        public Guid Id { get; set; }

        public Guid CustomerId { get; set; }

        public decimal Amount { get; set; } = 0;
    }
}
