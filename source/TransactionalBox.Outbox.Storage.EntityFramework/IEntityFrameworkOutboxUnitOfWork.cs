using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionalBox.Outbox.Storage.EntityFramework
{
    public interface IEntityFrameworkOutboxUnitOfWork
    {
        Task<IEntityFrameworkTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    }
}
