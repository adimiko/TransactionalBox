namespace TransactionalBox.Internals.Outbox.Transport.ContractsToImplement
{
    internal interface ITransportMessageSizeSettings
    {
        public int OptimalTransportMessageSize { get; }
    }
}
