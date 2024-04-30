namespace TransactionalBox.Outbox.Internals.Compression
{
    internal interface ICompressionAlgorithm
    {
        Task<byte[]> Compress(byte[] data);
    }
}
