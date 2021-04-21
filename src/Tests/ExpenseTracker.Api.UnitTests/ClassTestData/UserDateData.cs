using System;
using System.Collections;
using System.Collections.Generic;

namespace ExpenseTracker.Api.UnitTests.ClassTestData
{
    public class UserDateData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                1, new DateTime(2020, 9, 15)
            };
            yield return new object[]
            {
                2, new DateTime(2020, 9, 15)
            };
            yield return new object[]
            {
                3, new DateTime(2020, 9, 15)
            };
            yield return new object[]
            {
                1, new DateTime(2019, 6, 19)
            };
            yield return new object[]
            {
                2, new DateTime(2019, 6, 19)
            };
            yield return new object[]
            {
                3, new DateTime(2019, 6, 19)
            };
            yield return new object[]
            {
                1, DateTime.Now
            };
            yield return new object[]
            {
                2, DateTime.Now
            };
            yield return new object[]
            {
                3, DateTime.Now
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}