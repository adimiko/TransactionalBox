namespace TransactionalBox.Base.BackgroundService.Internals.Context.ValueObjects
{
    public sealed record JobId(string Value)
    {
        public override string ToString() => Value;
    }
}
