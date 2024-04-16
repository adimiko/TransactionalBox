namespace TransactionalBox.OutboxWorker.Internals.Contracts
{
    internal interface ITransportMessageSizeSettings
    {
        public int OptimalTransportMessageSize { get; }
    }
}
