using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using TransactionalBox.InboxBase.StorageModel;
using TransactionalBox.InboxWorker.Internals;
using TransactionalBox.Internals;

namespace TransactionalBox.InboxWorker.EntityFramework.Internals
{
    internal sealed class EntityFrameworkInboxStorage : IInboxStorage
    {
        private readonly ITransactionalBoxLogger _logger;

        private readonly DbContext _dbContext;

        private readonly DbSet<InboxMessage> _inboxMessages;

        private readonly DbSet<IdempotentInboxMessage> _idempotentInboxMessages;

        public EntityFrameworkInboxStorage(
            ITransactionalBoxLogger logger,
            DbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
            _inboxMessages = dbContext.Set<InboxMessage>();
            _idempotentInboxMessages = _dbContext.Set<IdempotentInboxMessage>();
        }

        public async Task AddRange(IEnumerable<InboxMessage> messages, DateTime nowUtc) //TODO maybe Result.Success or Failure(sqlProblem or duplicate message)
        {
            //TODO create models with created AddedUtc
            foreach (var message in messages) 
            {
                message.AddedUtc = nowUtc;
            }

            var idempotentMessages = messages.Select(x => IdempotentInboxMessage.CreateBasedOnInboxMessage(x));

            try
            {
                await _idempotentInboxMessages.AddRangeAsync(idempotentMessages);

                await _inboxMessages.AddRangeAsync(messages);

                await _dbContext.SaveChangesAsync();
            }
            //TODO Add a better check
            catch (DbUpdateException dbUpdateException) when (dbUpdateException.InnerException != null && dbUpdateException.InnerException.Message.Contains("duplicate key"))
            {
                _logger.Warning("Detected duplicate message with id: {id}", messages.First().Id); //TODO

                //TODO split into multiple insert with diff transactions
            }
        }
    }
}
