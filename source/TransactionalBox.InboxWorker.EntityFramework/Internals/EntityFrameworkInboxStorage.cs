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

        private readonly DbSet<InboxMessage> _inbox;

        public EntityFrameworkInboxStorage(
            ITransactionalBoxLogger logger,
            DbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
            _inbox = dbContext.Set<InboxMessage>();
        }

        public async Task Add(InboxMessage message) //TODO maybe Result.Success or Failure(sqlProblem or duplicate message)
        {
            try
            {
                await _inbox.AddAsync(message).AsTask();

                await _dbContext.SaveChangesAsync();
            }
            //TODO Add a better check
            catch (DbUpdateException dbUpdateException) when (dbUpdateException.InnerException != null && dbUpdateException.InnerException.Message.Contains("duplicate key"))
            {
                _logger.Warning("Detected duplicate message with id: {id}", message.Id);
            }
        }
    }
}
