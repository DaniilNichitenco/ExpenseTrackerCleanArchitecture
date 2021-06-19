using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using ExpenseTracker.Core.Domain.Entities;

namespace ExpenseTracker.Api.TestsCommon.Data
{
    public class ExpenseData : CommonBaseTestClassFixture
    {
        public static IEnumerable<Expense> GetExpenses()
        {
            var wallets = WalletData.GetWallets().ToList();
            var users = UserData.GetUsers().ToList();
            
            var expenses = _fixture.Build<Expense>()
            .With(x => x.WalletId, () => wallets[new Random().Next(wallets.Count)].Id)
            .Without(x => x.Wallet)
            .Without(x => x.Topic)
            .With(x => x.Date, 
            () => new Random().Next(1,3) == 1 ? new DateTime(2020, 9, 15) 
                : new DateTime(2019, 6, 19))
            .With(x => x.OwnerId, () => users[new Random().Next(users.Count)].Id)
            .CreateMany<Expense>(100);
            
            foreach (var expense in expenses)
            {
                expense.Wallet = wallets.FirstOrDefault(x => x.Id == expense.WalletId);
                if (expense.Wallet.OwnerId != expense.OwnerId)
                {
                    expense.Wallet.OwnerId = expense.OwnerId;
                }
            }

            return expenses;
        }
    }
}