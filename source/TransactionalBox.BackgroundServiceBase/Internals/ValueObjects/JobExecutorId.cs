namespace TransactionalBox.BackgroundServiceBase.Internals.ValueObjects
{
    public sealed record JobExecutorId(Guid Value)
    {
        public override string ToString() => Value.ToString();
    }
}
