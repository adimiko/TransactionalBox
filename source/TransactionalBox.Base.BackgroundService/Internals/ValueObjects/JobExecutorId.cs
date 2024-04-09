namespace TransactionalBox.Base.BackgroundService.Internals.ValueObjects
{
    public sealed record JobExecutorId(Guid Value)
    {
        public override string ToString() => Value.ToString();
    }
}
