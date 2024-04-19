namespace TransactionalBox.Base.BackgroundService.Internals.Context.ValueObjects
{
    public sealed record JobExecutorId(Guid Value)
    {
        public override string ToString() => Value.ToString();
    }
}
