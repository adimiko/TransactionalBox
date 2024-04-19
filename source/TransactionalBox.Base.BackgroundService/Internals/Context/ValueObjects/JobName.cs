namespace TransactionalBox.Base.BackgroundService.Internals.Context.ValueObjects
{
    public sealed record JobName(string Value)
    {
        public override string ToString() => Value;
    }
}
