namespace TransactionalBox.Outbox.Internals.Transport
{
    internal interface ITransportMessageSizeSettings
    {
        public int OptimalTransportMessageSize { get; }
    }
}
