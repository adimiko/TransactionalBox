﻿namespace TransactionalBox.Sample.WebApi.InMemory.ServiceWithInbox
{
    internal sealed class PublishableMessageHandler : IInboxHandler<PublishableMessage>
    {
        public Task Handle(PublishableMessage message, IExecutionContext executionContext)
        {
            //Logic
            //throw new NotImplementedException();
            return Task.CompletedTask;
        }
    }
}
