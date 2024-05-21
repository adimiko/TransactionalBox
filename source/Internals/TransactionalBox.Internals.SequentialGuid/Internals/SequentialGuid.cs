using System.Security.Cryptography;

namespace TransactionalBox.Internals.SequentialGuid.Internals
{
    // Based on code from https://github.com/jhtodd/SequentialGuid/tree/master

    internal sealed class SequentialGuid : ISequentialGuidGenerator
    {
        private static readonly RandomNumberGenerator _random = RandomNumberGenerator.Create();

        private readonly SequentialGuidType _sequentialGuidType;

        public SequentialGuid(SequentialGuidType sequentialGuidType)
        {
            _sequentialGuidType = sequentialGuidType;
        }

        public Guid Create()
        {
            var randomBytes = new byte[10];
            _random.GetBytes(randomBytes);

            long timestamp = DateTime.UtcNow.Ticks / 10000L;

            byte[] timestampBytes = BitConverter.GetBytes(timestamp);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(timestampBytes);
            }

            byte[] guidBytes = new byte[16];

            switch (_sequentialGuidType)
            {
                case SequentialGuidType.SequentialAsString:
                case SequentialGuidType.SequentialAsBinary:

                    Buffer.BlockCopy(timestampBytes, 2, guidBytes, 0, 6);
                    Buffer.BlockCopy(randomBytes, 0, guidBytes, 6, 10);

                    if (_sequentialGuidType == SequentialGuidType.SequentialAsString && BitConverter.IsLittleEndian)
                    {
                        Array.Reverse(guidBytes, 0, 4);
                        Array.Reverse(guidBytes, 4, 2);
                    }

                    break;

                case SequentialGuidType.SequentialAtEnd:

                    Buffer.BlockCopy(randomBytes, 0, guidBytes, 0, 10);
                    Buffer.BlockCopy(timestampBytes, 2, guidBytes, 10, 6);

                    break;
            }

            return new Guid(guidBytes);
        }
    }
}
