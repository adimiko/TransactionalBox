using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionalBox.End2EndTests.TestCases
{
    public sealed record Dependencies(IServiceProvider OutboxDependecies, IServiceProvider InboxDependecies);
}
