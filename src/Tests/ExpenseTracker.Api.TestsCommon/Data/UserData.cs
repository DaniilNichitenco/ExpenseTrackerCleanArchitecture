using System;
using System.Collections;
using System.Collections.Generic;
using ExpenseTracker.Core.Domain.Auth;

namespace ExpenseTracker.Api.TestsCommon.Data
{
    public class UserData
    {
        public static IEnumerable<User> GetUsers()
        {
            var users = new List<User>
            {
                new User
                {
                    Id = new Guid("9ECDC3AB-FAB3-4563-A54B-4CF59808765E")
                },
                new User
                {
                    Id = new Guid("D9DE987F-D581-435B-A038-DB6EA01D7C67")
                },
                new User
                {
                    Id = new Guid("8A6453AA-04F6-430B-BA1E-B3271449A5EF")
                }
            };

            return users;
        }
    }
}