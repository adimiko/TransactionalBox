using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace TransactionalBox.End2EndTests.TestCases
{
    public class End2EndTestCase
    {
        private readonly Func<ITestOutputHelper, Task<Dependencies>> _init;

        private readonly Func<Task> _cleanUp;

        private readonly string _testName;

        public End2EndTestCase(
            Func<ITestOutputHelper, Task<Dependencies>> init,
            Func<Task> cleanUp,
            string testName)
        {
            _init = init;
            _cleanUp = cleanUp;
            _testName = testName;
        }

        public Task<Dependencies> Init(ITestOutputHelper testOutputHelper)
        {
            return _init(testOutputHelper);
        }

        public Task CleanUp()
        {
            return _cleanUp();
        }

        public override string ToString() => _testName;
    }
}
