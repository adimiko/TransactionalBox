﻿using System.Collections;
using TransactionalBox.End2EndTests.TestCases;

namespace TransactionalBox.End2EndTests
{
    public class Tests : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { new StorageInMemory_TransportInMemory().GetEnd2EndTestCase() };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
