namespace TransactionalBox.OutboxWorker.Compression
{
    internal interface ICompressionAlgorithm
    {
        Task<byte[]> Compress(byte[] data);
    }
}
