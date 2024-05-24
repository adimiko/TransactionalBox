namespace TransactionalBox.Inbox.Internals.Decompression
{
    internal sealed class DecompressionPolicy : IDecompressionPolicy
    {
        private readonly IEnumerable<IDecompression> _decompressions;

        public DecompressionPolicy(IEnumerable<IDecompression> decompressions)
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
