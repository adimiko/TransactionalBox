namespace TransactionalBox.OutboxWorker.Compression
{
    public interface ICompressionAlgorithm
    {
        Task<byte[]> Compress(byte[] data);
    }
}
