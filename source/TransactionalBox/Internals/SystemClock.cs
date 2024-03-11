namespace TransactionalBox.Internals
{
    internal sealed class SystemClock : ISystemClock
    {
        public DateTime UtcNow => TimeProvider.GetUtcNow().UtcDateTime;

        public TimeProvider TimeProvider { get; }

        public SystemClock(TimeProvider timeProvider)
        {
            TimeProvider = timeProvider;
        }
    }
}
