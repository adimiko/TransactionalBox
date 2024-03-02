namespace TransactionalBox.Internals
{
    public abstract class TransactionalBoxException : Exception
    {
        protected TransactionalBoxException()
            : base() { }

        protected TransactionalBoxException(string message) 
            : base(message) { }
    }
}
