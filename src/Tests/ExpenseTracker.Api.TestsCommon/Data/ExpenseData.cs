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
            var wallets = GetWallets();
            
            var expenses = _fixture.Build<Expense>()
            .With(x => x.WalletId, () => new Random().Next(1,3))
            .Without(x => x.Wallet)
            .Without(x => x.Topic)
            .With(x => x.Date, 
            () => new Random().Next(1,3) == 1 ? new DateTime(2020, 9, 15) 
                : new DateTime(2019, 6, 19))
            .With(x => x.OwnerId, () => new Random().Next(1,4))
            .CreateMany<Expense>(100);
            
            foreach (var expense in expenses)
            {
                expense.Wallet = wallets.FirstOrDefault(x => x.Id == expense.WalletId);
            }

            return expenses;
        }
        
        public static IEnumerable<Wallet> GetWallets()
        {
            var wallets = new List<Wallet>
            {
                new Wallet
                {
                    Id = 1,
                    CurrencyCode = "mdl",
                    Bill = 10000
                },
                new Wallet
                {
                    Id = 2,
                    CurrencyCode = "eur",
                    Bill = 5000
                }
            };

            return wallets;
        }
    }
}