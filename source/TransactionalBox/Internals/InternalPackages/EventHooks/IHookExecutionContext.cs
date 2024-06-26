﻿namespace TransactionalBox.Internals.InternalPackages.EventHooks
{
    internal interface IHookExecutionContext
    {
        Guid Id { get; }

        string Name { get; }

        DateTime LastOccurredUtc { get; }

        bool IsError { get; }

        long Attempt { get; }
    }
}
