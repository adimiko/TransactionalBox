namespace TransactionalBox.Internals.Inbox.Decompression
{
    internal interface IDecompression
    {
        string Name { get; }

        Task<byte[]> Decompress(byte[] data);
    }
}
