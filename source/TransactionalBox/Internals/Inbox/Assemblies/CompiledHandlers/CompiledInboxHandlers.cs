using System.Linq.Expressions;
using TransactionalBox.Internals.Inbox.Assemblies.MessageTypes;

namespace TransactionalBox.Internals.Inbox.Assemblies.CompiledHandlers
{
    internal sealed class CompiledInboxHandlers : ICompiledInboxHandlers
    {
        private static readonly Dictionary<Type, Func<object, object, IExecutionContext, Task>> _compiledInboxHandlers = new();

        public CompiledInboxHandlers(IInboxMessageTypes inboxMessageTypes)
        {
            var messageTypes = inboxMessageTypes.MessageTypes;

            foreach (var messageType in messageTypes)
            {
                var compliedHandler = CompileInboxHandler(messageType);

                _compiledInboxHandlers.Add(messageType, compliedHandler);
            }
        }

        public Func<object, object, IExecutionContext, Task>? GetCompiledInboxHandler(Type messageType)
        {
            //TODO not found compiled handler
            return _compiledInboxHandlers.GetValueOrDefault(messageType);
        }

        private static Func<object, object, IExecutionContext, Task> CompileInboxHandler(Type messageType)
        {
            var messageObjectTypeParameter = Expression.Parameter(typeof(object));
            var executionContextObjectTypeParameter = Expression.Parameter(typeof(IExecutionContext));
            var inboxHandlerObjectTypeParameter = Expression.Parameter(typeof(object));

            var messageTypeExpression = Expression.Convert(messageObjectTypeParameter, messageType);
            var inboxHandlerTypeExpression = Expression.Convert(inboxHandlerObjectTypeParameter, typeof(IInboxHandler<>).MakeGenericType(messageType));

            var body = Expression.Call(inboxHandlerTypeExpression, "Handle", new Type[0], messageTypeExpression, executionContextObjectTypeParameter);

            return Expression.Lambda<Func<object, object, IExecutionContext, Task>>(body, inboxHandlerObjectTypeParameter, messageObjectTypeParameter, executionContextObjectTypeParameter).Compile();
        }
    }
}
