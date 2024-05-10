using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionalBox.Outbox.Internals.Hooks
{
    internal interface ITranactionCommited
    {
        Task Commited();
    }
}
