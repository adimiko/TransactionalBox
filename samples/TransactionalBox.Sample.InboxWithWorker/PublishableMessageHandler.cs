﻿using TransactionalBox.Inbox;
using TransactionalBox.Inbox.Contexts;

namespace TransactionalBox.Sample.InboxWithWorker
{
    internal sealed class PublishableMessageHandler : IInboxMessageHandler<PublishableMessage>
    {
        public Task Handle(PublishableMessage message, IExecutionContext executionContext)
        {
            //Logic
            //throw new NotImplementedException();
            return Task.CompletedTask;
        }
    }
}
