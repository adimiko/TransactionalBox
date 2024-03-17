namespace TransactionalBox.BackgroundServiceBase.Internals.ValueObjects
{
    public sealed record JobId(string Value)
    {
        public override string ToString() => Value;
    }
}
