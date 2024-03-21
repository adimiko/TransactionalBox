namespace TransactionalBox.Internals
{
    internal interface ISystemClock
    {
        DateTime UtcNow { get; }

        TimeProvider TimeProvider { get; }
    }
}
