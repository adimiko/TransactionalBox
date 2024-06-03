namespace TransactionalBox.Internals.Inbox.Decompression
{
    internal interface IDecompressionFactory
    {
        IDecompression GetDecompression(string compressionName);
    }
}
