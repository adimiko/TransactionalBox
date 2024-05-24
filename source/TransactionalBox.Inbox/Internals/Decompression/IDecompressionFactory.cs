namespace TransactionalBox.Inbox.Internals.Decompression
{
    internal interface IDecompressionFactory
    {
        IDecompression GetDecompression(string compressionName);
    }
}
