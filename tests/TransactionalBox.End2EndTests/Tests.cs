using System.Collections;
using TransactionalBox.End2EndTests.TestCases.Storage.EntityFrameworkCore;

namespace TransactionalBox.End2EndTests
{
    public class Tests : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            //yield return new object[] { new StorageInMemory_TransportInMemory().GetEnd2EndTestCase() };
            yield return new object[] { new EntityFrameworkCorePostgresSql().GetEnd2EndTestCase() };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
