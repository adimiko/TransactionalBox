namespace TransactionalBox.BackgroundServiceBase.Internals.ValueObjects
{
    public record JobId
    {
        public string Value { get; }

        public JobId(string value)
        {
            Value = value;
        }

        public override string ToString() => Value;
    }
}
