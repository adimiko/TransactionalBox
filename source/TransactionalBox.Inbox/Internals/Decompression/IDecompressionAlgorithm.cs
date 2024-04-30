namespace TransactionalBox.Inbox.Internals.Decompression
{
    internal interface IDecompressionAlgorithm
    {
        Task<byte[]> Decompress(byte[] data);
    }
}
