namespace TransactionalBox.Base.BackgroundService.Internals.Contexts.JobExecution.ValueObjects
{
    public sealed record JobExecutorId(Guid Value)
    {
        public override string ToString() => Value.ToString();
    }
}
