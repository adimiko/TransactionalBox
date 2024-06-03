namespace TransactionalBox.Internals.Outbox.Compression
{
    internal interface ICompression
    {
        string Name { get; }

        Task<byte[]> Compress(byte[] data);
    }
}
