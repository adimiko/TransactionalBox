namespace TransactionalBox.Internals
{
    public interface ITransactionalBoxLogger
    {
        void Critical(string? message, params object?[] args);

        void Critical(Exception exception, string? message, params object?[] args);

        void Error(string? message, params object?[] args);

        void Error(Exception exception, string? message, params object?[] args);

        void Warning(string? message, params object?[] args);

        void Warning(Exception exception, string? message, params object?[] args);

        void Information(string? message, params object?[] args);

        void Information(Exception exception, string? message, params object?[] args);

        void Debug(string? message, params object?[] args);

        void Debug(Exception exception, string? message, params object?[] args);

        void Trace(string? message, params object?[] args);

        void Trace(Exception exception, string? message, params object?[] args);
    }
}
