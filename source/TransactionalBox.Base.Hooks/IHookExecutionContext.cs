namespace TransactionalBox.Base.Hooks
{
    internal interface IHookExecutionContext
    {
        Guid Id { get; }

        string Name { get; }

        DateTime LastOccurredUtc { get; }
    }
}
