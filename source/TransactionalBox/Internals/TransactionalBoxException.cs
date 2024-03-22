namespace TransactionalBox.Internals
{
    internal abstract class TransactionalBoxException : Exception
    {
        protected TransactionalBoxException()
            : base() { }

        protected TransactionalBoxException(string message) 
            : base(message) { }
    }
}
