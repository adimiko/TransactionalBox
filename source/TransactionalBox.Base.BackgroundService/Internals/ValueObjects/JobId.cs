namespace TransactionalBox.Base.BackgroundService.Internals.ValueObjects
{
    public sealed record JobId(string Value)
    {
        public override string ToString() => Value;
    }
}
