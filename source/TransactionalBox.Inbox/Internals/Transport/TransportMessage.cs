﻿namespace TransactionalBox.Inbox.Internals.Transport
{
    internal sealed class TransportMessage
    {
        internal required byte[] Payload { get; init; }

        internal required string Compression { get; init; }
    }
}
