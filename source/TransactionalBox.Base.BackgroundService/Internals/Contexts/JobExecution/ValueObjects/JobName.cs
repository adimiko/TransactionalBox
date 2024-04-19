namespace TransactionalBox.Base.BackgroundService.Internals.Contexts.JobExecution.ValueObjects
{
    public sealed record JobName(string Value)
    {
        public override string ToString() => Value;
    }
}
