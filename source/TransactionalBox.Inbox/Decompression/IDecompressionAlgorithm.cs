namespace TransactionalBox.Inbox.Decompression
{
    public interface IDecompressionAlgorithm
    {
        Task<byte[]> Decompress(byte[] data);
    }
}
