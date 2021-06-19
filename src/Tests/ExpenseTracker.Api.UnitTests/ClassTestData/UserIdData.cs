using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ExpenseTracker.Api.TestsCommon.Data;

namespace ExpenseTracker.Api.UnitTests.ClassTestData
{
    public class UserIdData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            var users = UserData.GetUsers().ToList();
            
            yield return new object[]
            {
                users[0].Id
            };
            yield return new object[]
            {
                users[1].Id
            };
            yield return new object[]
            {
                users[2].Id
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}