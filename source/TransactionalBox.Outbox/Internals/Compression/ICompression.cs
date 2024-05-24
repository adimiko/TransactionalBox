namespace TransactionalBox.Outbox.Internals.Compression
{
    internal interface ICompression
    {
        string Name { get; }

        Task<byte[]> Compress(byte[] data);
    }
}
