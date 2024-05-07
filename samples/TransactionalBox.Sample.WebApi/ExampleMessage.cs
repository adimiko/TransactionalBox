﻿using TransactionalBox.Inbox;
using TransactionalBox.Outbox;

namespace TransactionalBox.Sample.WebApi
{
    public sealed class ExampleMessage : OutboxMessage
    {
        public string Name { get; set; }

        public int Age { get; set; }
    }
}
