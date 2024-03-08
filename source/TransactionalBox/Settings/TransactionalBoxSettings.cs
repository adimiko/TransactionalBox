using TransactionalBox.Internals;

namespace TransactionalBox.Settings
{
    public sealed class TransactionalBoxSettings : ITransactionalBoxSettings
    {
        public string ServiceName { get; set; }

        internal TransactionalBoxSettings() { }
    }
}
