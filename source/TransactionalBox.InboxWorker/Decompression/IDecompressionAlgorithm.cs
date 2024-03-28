namespace TransactionalBox.InboxWorker.Decompression
{
    public interface IDecompressionAlgorithm
    {
        Task<byte[]> Decompress(byte[] data);
    }
}
