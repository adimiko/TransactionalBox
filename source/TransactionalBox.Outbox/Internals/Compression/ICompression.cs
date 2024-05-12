namespace TransactionalBox.Outbox.Internals.Compression
{
    internal interface ICompression
    {
        Task<byte[]> Compress(byte[] data);
    }
}
