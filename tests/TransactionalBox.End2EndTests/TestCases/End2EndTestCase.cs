using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionalBox.End2EndTests.TestCases
{
    public sealed class End2EndTestCase
    {
        public IServiceProvider OutboxDependecies { get; }

        public IServiceProvider InboxDependecies { get; }

        private readonly string _testName;

        public End2EndTestCase(
            IServiceProvider outboxDependecies, 
            IServiceProvider inboxDependecies,
            string testName)
        {
            OutboxDependecies = outboxDependecies;
            InboxDependecies = inboxDependecies;
            _testName = testName;
        }

        public override string ToString() => _testName;
    }
}
