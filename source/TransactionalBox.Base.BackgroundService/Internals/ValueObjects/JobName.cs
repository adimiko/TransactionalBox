namespace TransactionalBox.Base.BackgroundService.Internals.ValueObjects
{
    public sealed record JobName(string Value)
    {
        public override string ToString() => Value;
    }
}
