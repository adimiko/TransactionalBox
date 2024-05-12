namespace TransactionalBox.Inbox.Internals.Decompression
{
    internal interface IDecompression
    {
        Task<byte[]> Decompress(byte[] data);
    }
}
