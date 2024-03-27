namespace TransactionalBox.InboxWorker.Decompression
{
    public interface IDecompressionAlgorithm
    {
        byte[] Decompress(byte[] data);
    }
}
