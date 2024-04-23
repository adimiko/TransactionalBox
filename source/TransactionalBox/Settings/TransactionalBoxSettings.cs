using System.ComponentModel.DataAnnotations;

namespace TransactionalBox.Settings
{
    public sealed class TransactionalBoxSettings
    {
        [Required]
        public string ServiceId { get; set; }

        internal TransactionalBoxSettings() { }
    }
}
