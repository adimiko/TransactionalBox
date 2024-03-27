namespace TransactionalBox.OutboxWorker.Compression
{
    public interface ICompressionAlgorithm
    {
        byte[] Compress(byte[] input);
    }
}
