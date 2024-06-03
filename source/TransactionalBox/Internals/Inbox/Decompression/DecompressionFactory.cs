namespace TransactionalBox.Internals.Inbox.Decompression
{
    internal sealed class DecompressionFactory : IDecompressionFactory
    {
        private readonly IEnumerable<IDecompression> _decompressions;

        public DecompressionFactory(IEnumerable<IDecompression> decompressions)
        {
            _decompressions = decompressions;
        }

        public IDecompression GetDecompression(string compressionName)
        {
            //TODO validation
            var decompression = _decompressions.Single(x => x.Name == compressionName);

            return decompression;
        }
    }
}
