﻿namespace TransactionalBox.Inbox.Internals.Jobs
{
    internal interface ICleanUpProcessedInboxMessagesJobSettings
    {
        int BatchSize { get; }

        TimeSpan DelayWhenBatchIsEmpty { get; }

        TimeSpan DelayWhenBatchIsNotFull { get; }
    }
}