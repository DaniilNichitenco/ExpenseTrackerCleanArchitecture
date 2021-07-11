using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ExpenseTracker.Api.TestsCommon.Data;

namespace ExpenseTracker.Api.UnitTests.ClassTestData
{
    public class UserDateData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            var users = UserData.GetUsers().ToList();
            
            yield return new object[]
            {
                users[0].Id, new DateTime(2020, 9, 15)
            };
            yield return new object[]
            {
                users[1].Id, new DateTime(2020, 9, 15)
            };
            yield return new object[]
            {
                users[2].Id, new DateTime(2020, 9, 15)
            };
            yield return new object[]
            {
                users[0].Id, new DateTime(2019, 6, 19)
            };
            yield return new object[]
            {
                users[1].Id, new DateTime(2019, 6, 19)
            };
            yield return new object[]
            {
                users[2].Id, new DateTime(2019, 6, 19)
            };
            yield return new object[]
            {
                users[0].Id, DateTime.Now
            };
            yield return new object[]
            {
                users[1].Id, DateTime.Now
            };
            yield return new object[]
            {
                users[2].Id, DateTime.Now
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}