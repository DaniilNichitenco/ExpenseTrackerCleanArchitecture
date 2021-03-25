using ExpenseTracker.Core.Application.Interfaces;
using ExpenseTracker.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Infrastructure.API
{
    public static class Seed
    {
        public static async Task SeedWallets(IEFRepository<Wallet> repository)
        {
            if (!repository.Read().Any())
            {
                List<Wallet> Wallets = new List<Wallet>()
                {
                    new Wallet() { OwnerId = 1, Bill = 1000, CurrencyCode = "USD" },
                    new Wallet() { OwnerId = 1, Bill = 2000, CurrencyCode = "EUR" },
                };

                await repository.AddRangeAsync(Wallets);
                await repository.SaveChangesAsync();
            }
        }

        public static async Task SeedTopics(IEFRepository<Topic> repository)
        {
            if (!repository.Read().Any())
            {
                List<Topic> topics = new List<Topic>()
                {
                    new Topic(){ Name = "Food", OwnerId = 1 },
                    new Topic(){ Name = "Transport", OwnerId = 1 },
                    new Topic(){ Name = "Health", OwnerId = 1 },
                    new Topic(){ Name = "Sport", OwnerId = 1 }
                };

                await repository.AddRangeAsync(topics);
                await repository.SaveChangesAsync();
            }
        }

        public static async Task SeedExpenses(IEFRepository<Expense> repository)
        {
            if (!repository.Read().Any())
            {
                List<Expense> expenses = new List<Expense>()
                {
                    new Expense() { OwnerId = 1, Title="First expense",
                        Date=new DateTime(2020, 1, 1), Money=900, WalletId=1, TopicId = 1 },
                    new Expense() { OwnerId = 1, Title="Second expense",
                        Date=new DateTime(2020, 1, 2), Money=700, WalletId=1, TopicId = 1 },
                    new Expense() { OwnerId = 1, Title="3 expense",
                        Date=new DateTime(2020, 2, 3), Money=1400, WalletId=1, TopicId = 1 },
                    new Expense() { OwnerId = 1, Title="4 expense",
                        Date=new DateTime(2020, 2, 4), Money=800, WalletId=1, TopicId = 2 },
                    new Expense() { OwnerId = 1, Title="5 expense",
                        Date=new DateTime(2020, 3, 5), Money=1200, WalletId=1, TopicId = 2 },
                    new Expense() { OwnerId = 1, Title="6 expense",
                        Date=new DateTime(2020, 3, 6), Money=1100, WalletId=1, TopicId = 2 },
                    new Expense() { OwnerId = 1, Title="7 expense",
                        Date=new DateTime(2020, 1, 7), Money=2400, WalletId=2, TopicId = 3 },
                    new Expense() { OwnerId = 1, Title="8 expense",
                        Date=new DateTime(2020, 1, 8), Money=1400, WalletId=2, TopicId = 3 },
                    new Expense() { OwnerId = 1, Title="9 expense",
                        Date=new DateTime(2020, 2, 9), Money=2200, WalletId=2, TopicId = 3 },
                    new Expense() { OwnerId = 1, Title="10 expense",
                        Date=new DateTime(2020, 2, 10), Money=1200, WalletId=2, TopicId = 4 },
                    new Expense() { OwnerId = 1, Title="11 expense",
                        Date=new DateTime(2020, 3, 11), Money=2100, WalletId=2, TopicId = 4 },
                    new Expense() { OwnerId = 1, Title="12 expense",
                        Date=new DateTime(2020, 3, 12), Money=1100, WalletId=2, TopicId = 4 },
                    new Expense() { OwnerId = 1, Title="13 expense",
                        Date=new DateTime(2020, 4, 12), Money=3100, WalletId=2, TopicId = 4 },
                    new Expense() { OwnerId = 1, Title="13 expense",
                        Date=new DateTime(2020, 4, 13), Money=1100, WalletId=2, TopicId = 4 },
                    new Expense() { OwnerId = 1, Title="14 expense",
                        Date=new DateTime(2020, 4, 14), Money=1400, WalletId=2, TopicId = 4 },
                    new Expense() { OwnerId = 1, Title="15 expense",
                        Date=new DateTime(2020, 5, 12), Money=1100, WalletId=2, TopicId = 4 },
                    new Expense() { OwnerId = 1, Title="16 expense",
                        Date=new DateTime(2020, 5, 13), Money=1100, WalletId=2, TopicId = 4 },
                    new Expense() { OwnerId = 1, Title="17 expense",
                        Date=new DateTime(2020, 5, 14), Money=600, WalletId=2, TopicId = 4 },
                    new Expense() { OwnerId = 1, Title="18 expense",
                        Date=new DateTime(2020, 6, 13), Money=600, WalletId=2, TopicId = 4 },
                    new Expense() { OwnerId = 1, Title="19 expense",
                        Date=new DateTime(2020, 6, 14), Money=600, WalletId=2, TopicId = 4 },
                    new Expense() { OwnerId = 1, Title="20 expense",
                        Date=new DateTime(2020, 6, 15), Money=400, WalletId=2, TopicId = 4 },
                    new Expense() { OwnerId = 1, Title="21 expense",
                        Date=new DateTime(2020, 6, 16), Money=270, WalletId=2, TopicId = 4 },
                     new Expense() { OwnerId = 1, Title="22 expense",
                        Date=new DateTime(2020, 12, 8), Money=340, WalletId=1, TopicId = 4 },
                    new Expense() { OwnerId = 1, Title="23 expense",
                        Date=new DateTime(2020, 12, 9), Money=90, WalletId=2, TopicId = 1 },
                    new Expense() { OwnerId = 1, Title="24 expense",
                        Date=new DateTime(2020, 12, 10), Money=40, WalletId=2, TopicId = 4 },
                    new Expense() { OwnerId = 1, Title="10 expense",
                        Date=new DateTime(2020, 12, 10), Money=100, WalletId=2, TopicId = 4 },
                    new Expense() { OwnerId = 1, Title="10 expense",
                        Date=new DateTime(2020, 12, 11), Money=90, WalletId=2, TopicId = 4 },
                    new Expense() { OwnerId = 1, Title="10 expense",
                        Date=new DateTime(2020, 12, 12), Money=150, WalletId=2, TopicId = 4 },
                    new Expense() { OwnerId = 1, Title="10 expense",
                        Date=new DateTime(2020, 12, 13), Money=120, WalletId=2, TopicId = 4 },
                    new Expense() { OwnerId = 1, Title="10 expense",
                        Date=new DateTime(2020, 12, 14), Money=200, WalletId=2, TopicId = 4 },
                    new Expense() { OwnerId = 1, Title="10 expense",
                        Date=new DateTime(2020, 12, 9), Money=230, WalletId=2, TopicId = 4 },
                    new Expense() { OwnerId = 1, Title="10 expense",
                        Date=new DateTime(2020, 12, 8), Money=90, WalletId=2, TopicId = 4 },
                    new Expense() { OwnerId = 1, Title="10 expense",
                        Date=new DateTime(2020, 12, 7), Money=40, WalletId=2, TopicId = 4 },
                    new Expense() { OwnerId = 1, Title="10 expense",
                        Date=new DateTime(2020, 12, 6), Money=50, WalletId=2, TopicId = 4 },
                    new Expense() { OwnerId = 1, Title="10 expense",
                        Date=new DateTime(2020, 12, 5), Money=130, WalletId=2, TopicId = 4 },
                    new Expense() { OwnerId = 1, Title="10 expense",
                        Date=new DateTime(2020, 12, 4), Money=170, WalletId=2, TopicId = 4 },
                    new Expense() { OwnerId = 1, Title="10 expense",
                        Date=new DateTime(2020, 12, 3), Money=150, WalletId=2, TopicId = 4 },
                    new Expense() { OwnerId = 1, Title="10 expense",
                        Date=new DateTime(2020, 12, 2), Money=200, WalletId=2, TopicId = 4 },
                    new Expense() { OwnerId = 1, Title="10 expense",
                        Date=new DateTime(2020, 2, 1), Money=100, WalletId=2, TopicId = 4 },
                    new Expense() { OwnerId = 1, Title="10 expense",
                        Date=new DateTime(2020, 12, 10), Money=160, WalletId=1, TopicId = 2 },
                    new Expense() { OwnerId = 1, Title="10 expense",
                        Date=new DateTime(2020, 12, 11), Money=190, WalletId=1, TopicId = 2 },
                    new Expense() { OwnerId = 1, Title="10 expense",
                        Date=new DateTime(2020, 12, 12), Money=120, WalletId=1, TopicId = 2 },
                    new Expense() { OwnerId = 1, Title="10 expense",
                        Date=new DateTime(2020, 12, 13), Money=150, WalletId=1, TopicId = 3 },
                    new Expense() { OwnerId = 1, Title="10 expense",
                        Date=new DateTime(2020, 12, 14), Money=100, WalletId=1, TopicId = 3 },
                    new Expense() { OwnerId = 1, Title="10 expense",
                        Date=new DateTime(2020, 12, 9), Money=130, WalletId=1, TopicId = 3 },
                    new Expense() { OwnerId = 1, Title="10 expense",
                        Date=new DateTime(2020, 12, 8), Money=30, WalletId=1, TopicId = 1 },
                    new Expense() { OwnerId = 1, Title="10 expense",
                        Date=new DateTime(2020, 12, 7), Money=140, WalletId=1, TopicId = 3 },
                    new Expense() { OwnerId = 1, Title="10 expense",
                        Date=new DateTime(2020, 12, 6), Money=150, WalletId=1, TopicId = 4 },
                    new Expense() { OwnerId = 1, Title="10 expense",
                        Date=new DateTime(2020, 12, 5), Money=230, WalletId=1, TopicId = 4 },
                    new Expense() { OwnerId = 1, Title="10 expense",
                        Date=new DateTime(2020, 12, 4), Money=70, WalletId=1, TopicId = 4 },
                    new Expense() { OwnerId = 1, Title="10 expense",
                        Date=new DateTime(2020, 12, 3), Money=50, WalletId=1, TopicId = 1 },
                    new Expense() { OwnerId = 1, Title="10 expense",
                        Date=new DateTime(2020, 12, 2), Money=100, WalletId=1, TopicId = 4 },
                    new Expense() { OwnerId = 1, Title="10 expense",
                        Date=new DateTime(2020, 2, 1), Money=800, WalletId=1, TopicId = 4 },
                    new Expense() { OwnerId = 1, Title="101 expense",
                        Date=new DateTime(2020, 4, 1), Money=1300, WalletId=1, TopicId = 4 },
                    new Expense() { OwnerId = 1, Title="102 expense",
                        Date=new DateTime(2020, 5, 1), Money=1800, WalletId=1, TopicId = 4 },
                    new Expense() { OwnerId = 1, Title="103 expense",
                        Date=new DateTime(2020, 6, 1), Money=2000, WalletId=1, TopicId = 4 },
                    new Expense() { OwnerId = 1, Title="104 expense",
                        Date=new DateTime(2020, 7, 1), Money=2100, WalletId=1, TopicId = 4 },
                    new Expense() { OwnerId = 1, Title="105 expense",
                        Date=new DateTime(2020, 8, 1), Money=1800, WalletId=1, TopicId = 3 },
                    new Expense() { OwnerId = 1, Title="106 expense",
                        Date=new DateTime(2020, 9, 1), Money=1600, WalletId=1, TopicId = 3 },
                    new Expense() { OwnerId = 1, Title="107 expense",
                        Date=new DateTime(2020, 10, 1), Money=1800, WalletId=1, TopicId = 3 },
                    new Expense() { OwnerId = 1, Title="108 expense",
                        Date=new DateTime(2020, 11, 1), Money=1900, WalletId=1, TopicId = 3 },
                    new Expense() { OwnerId = 1, Title="109 expense",
                        Date=new DateTime(2020, 7, 1), Money=1900, WalletId=2, TopicId = 1 },
                    new Expense() { OwnerId = 1, Title="110 expense",
                        Date=new DateTime(2020, 8, 1), Money=800, WalletId=2, TopicId = 2 },
                    new Expense() { OwnerId = 1, Title="111 expense",
                        Date=new DateTime(2020, 9, 1), Money=1200, WalletId=2, TopicId = 3 },
                    new Expense() { OwnerId = 1, Title="112 expense",
                        Date=new DateTime(2020, 10, 1), Money=1100, WalletId=2, TopicId = 4 },
                    new Expense() { OwnerId = 1, Title="113 expense",
                        Date=new DateTime(2020, 11, 1), Money=1500, WalletId=2, TopicId = 4 },
                };

                await repository.AddRangeAsync(expenses);
                await repository.SaveChangesAsync();
            }
        }
    }
}
