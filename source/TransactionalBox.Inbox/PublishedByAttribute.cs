namespace TransactionalBox.Inbox
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class PublishedByAttribute : Attribute
    {
        public string PublishedBy { get; }

        public PublishedByAttribute(string publishedBy)
        {
            //TODO validation
            PublishedBy = publishedBy;
        }
    }
}
