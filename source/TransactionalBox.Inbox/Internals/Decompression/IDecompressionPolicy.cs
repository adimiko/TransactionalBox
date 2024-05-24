namespace TransactionalBox.Inbox.Internals.Decompression
{
    internal interface IDecompressionPolicy
    {
        IDecompression GetDecompression(string compressionName);
    }
}
