namespace TransactionalBox.Outbox.Internals.Contracts
{
    internal interface ITransportMessageSizeSettings
    {
        public int OptimalTransportMessageSize { get; }
    }
}
