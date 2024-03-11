namespace TransactionalBox.Internals
{
    public interface ISystemClock
    {
        DateTime UtcNow { get; }

        TimeProvider TimeProvider { get; }
    }
}
