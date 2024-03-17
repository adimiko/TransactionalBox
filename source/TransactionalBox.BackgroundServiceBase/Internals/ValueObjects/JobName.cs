namespace TransactionalBox.BackgroundServiceBase.Internals.ValueObjects
{
    public sealed record JobName(string Value)
    {
        public override string ToString() => Value;
    }
}
