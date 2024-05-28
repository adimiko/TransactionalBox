namespace TransactionalBox.Outbox.Internals.Transport.ContractsToImplement
{
    internal interface ITransportMessageSizeSettings
    {
        public int OptimalTransportMessageSize { get; }
    }
}
