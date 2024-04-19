namespace TransactionalBox.Base.BackgroundService.Internals.Contexts.JobExecution.ValueObjects
{
    public sealed record JobId(string Value)
    {
        public override string ToString() => Value;
    }
}
