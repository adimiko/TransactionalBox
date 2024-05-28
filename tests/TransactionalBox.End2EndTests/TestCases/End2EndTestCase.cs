using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionalBox.End2EndTests.TestCases
{
    public sealed class End2EndTestCase
    {
        private readonly Func<Task<Dependencies>> _init;

        private readonly string _testName;

        public End2EndTestCase(
            Func<Task<Dependencies>> init,
            string testName)
        {
            _init = init;
            _testName = testName;
        }

        public Task<Dependencies> Init()
        {
            return _init();
        }

        public override string ToString() => _testName;
    }
}
