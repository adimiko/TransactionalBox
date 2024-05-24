namespace TransactionalBox.Inbox.Internals.Decompression
{
    internal interface IDecompression
    {
        string Name { get; }

        Task<byte[]> Decompress(byte[] data);
    }
}
