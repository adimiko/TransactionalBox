namespace TransactionalBox.Internals
{
    internal interface ITopicFactory
    {
        string Create(string serviceName, string messageName);
    }
}
