using System;
using System.Collections.Generic;
using System.Linq;
using ExpenseTracker.Core.Domain.Entities;

namespace ExpenseTracker.Api.TestsCommon.Data
{
    public class WalletData : CommonBaseTestClassFixture
    {
        public static IEnumerable<Wallet> GetWallets()
        {
            var users = UserData.GetUsers().ToList();
            
            var wallets = new List<Wallet>
            {
                new Wallet
                {
                    Id = new Guid("ECE0A896-39AC-4B3E-846F-5DB24B2FAAEF"),
                    CurrencyCode = "mdl",
                    Bill = 10000
                },
                new Wallet
                {
                    Id = new Guid("D0B1CDBA-B336-49B7-8E66-C8DF376BEAFD"),
                    CurrencyCode = "eur",
                    Bill = 5000
                }
            };

            return wallets;
        }
    }
}