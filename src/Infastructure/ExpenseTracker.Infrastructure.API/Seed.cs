using ExpenseTracker.Core.Application.Interfaces;
using ExpenseTracker.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Infrastructure.Repository.API
{
    public static class Seed
    {
        public static async Task SeedWallets(IGenericRepository<Wallet> repository)
        {
            if (!repository.Read().Any())
            {
                List<Wallet> Wallets = new List<Wallet>()
                {
                    new Wallet()
                        {OwnerId = new Guid("E896E2E1-3C8E-49FB-BCEB-B1C63E56A5DA"), Bill = 1000, CurrencyCode = "USD"},
                    new Wallet()
                        {OwnerId = new Guid("E896E2E1-3C8E-49FB-BCEB-B1C63E56A5DA"), Bill = 2000, CurrencyCode = "EUR"},
                };

                await repository.AddRangeAsync(Wallets);
                await repository.SaveChangesAsync();
            }
        }

        public static async Task SeedTopics(IGenericRepository<Topic> repository)
        {
            if (!repository.Read().Any())
            {
                List<Topic> topics = new List<Topic>()
                {
                    new Topic() {Name = "Food", OwnerId = new Guid("E896E2E1-3C8E-49FB-BCEB-B1C63E56A5DA")},
                    new Topic() {Name = "Transport", OwnerId = new Guid("E896E2E1-3C8E-49FB-BCEB-B1C63E56A5DA")},
                    new Topic() {Name = "Health", OwnerId = new Guid("E896E2E1-3C8E-49FB-BCEB-B1C63E56A5DA")},
                    new Topic() {Name = "Sport", OwnerId = new Guid("E896E2E1-3C8E-49FB-BCEB-B1C63E56A5DA")}
                };

                await repository.AddRangeAsync(topics);
                await repository.SaveChangesAsync();
            }
        }

        public static async Task SeedExpenses(IGenericRepository<Expense> repository)
        {
            if (!repository.Read().Any())
            {
                List<Expense> expenses = new List<Expense>()
                {
                    new Expense()
                    {
                        OwnerId = new Guid("E896E2E1-3C8E-49FB-BCEB-B1C63E56A5DA"),
                        Title = "First expense",
                        Date = new DateTime(2020, 1, 1), Money = 900,
                        WalletId = new Guid("748E55F2-440F-480F-9909-EC86CFDEB19E"),
                        TopicId = new Guid("4CFF9DA4-EF3C-4AD1-89FB-962A592FD0CD")
                    },
                    new Expense()
                    {
                        OwnerId = new Guid("E896E2E1-3C8E-49FB-BCEB-B1C63E56A5DA"),
                        Title = "Second expense",
                        Date = new DateTime(2020, 1, 2), Money = 700,
                        WalletId = new Guid("748E55F2-440F-480F-9909-EC86CFDEB19E"),
                        TopicId = new Guid("4CFF9DA4-EF3C-4AD1-89FB-962A592FD0CD")
                    },
                    new Expense()
                    {
                        OwnerId = new Guid("E896E2E1-3C8E-49FB-BCEB-B1C63E56A5DA"),
                        Title = "3 expense",
                        Date = new DateTime(2020, 2, 3), Money = 1400,
                        WalletId = new Guid("748E55F2-440F-480F-9909-EC86CFDEB19E"),
                        TopicId = new Guid("4CFF9DA4-EF3C-4AD1-89FB-962A592FD0CD")
                    },
                    new Expense()
                    {
                        OwnerId = new Guid("E896E2E1-3C8E-49FB-BCEB-B1C63E56A5DA"),
                        Title = "4 expense",
                        Date = new DateTime(2020, 2, 4), Money = 800,
                        WalletId = new Guid("748E55F2-440F-480F-9909-EC86CFDEB19E"),
                        TopicId = new Guid("D6A01F6E-CCB5-4FE7-9B10-DDFCCC4F484D")
                    },
                    new Expense()
                    {
                        OwnerId = new Guid("E896E2E1-3C8E-49FB-BCEB-B1C63E56A5DA"),
                        Title = "5 expense",
                        Date = new DateTime(2020, 3, 5), Money = 1200,
                        WalletId = new Guid("748E55F2-440F-480F-9909-EC86CFDEB19E"),
                        TopicId = new Guid("D6A01F6E-CCB5-4FE7-9B10-DDFCCC4F484D")
                    },
                    new Expense()
                    {
                        OwnerId = new Guid("E896E2E1-3C8E-49FB-BCEB-B1C63E56A5DA"),
                        Title = "6 expense",
                        Date = new DateTime(2020, 3, 6), Money = 1100,
                        WalletId = new Guid("748E55F2-440F-480F-9909-EC86CFDEB19E"),
                        TopicId = new Guid("D6A01F6E-CCB5-4FE7-9B10-DDFCCC4F484D")
                    },
                    new Expense()
                    {
                        OwnerId = new Guid("E896E2E1-3C8E-49FB-BCEB-B1C63E56A5DA"),
                        Title = "7 expense",
                        Date = new DateTime(2020, 1, 7), Money = 2400,
                        WalletId = new Guid("86426013-1B44-4F11-81B1-994C79264A9B"),
                        TopicId = new Guid("03E9DF21-11F1-43E3-AB86-12D5E746A132")
                    },
                    new Expense()
                    {
                        OwnerId = new Guid("E896E2E1-3C8E-49FB-BCEB-B1C63E56A5DA"),
                        Title = "8 expense",
                        Date = new DateTime(2020, 1, 8), Money = 1400,
                        WalletId = new Guid("86426013-1B44-4F11-81B1-994C79264A9B"),
                        TopicId = new Guid("03E9DF21-11F1-43E3-AB86-12D5E746A132")
                    },
                    new Expense()
                    {
                        OwnerId = new Guid("E896E2E1-3C8E-49FB-BCEB-B1C63E56A5DA"),
                        Title = "9 expense",
                        Date = new DateTime(2020, 2, 9), Money = 2200,
                        WalletId = new Guid("86426013-1B44-4F11-81B1-994C79264A9B"),
                        TopicId = new Guid("03E9DF21-11F1-43E3-AB86-12D5E746A132")
                    },
                    new Expense()
                    {
                        OwnerId = new Guid("E896E2E1-3C8E-49FB-BCEB-B1C63E56A5DA"),
                        Title = "10 expense",
                        Date = new DateTime(2020, 2, 10), Money = 1200,
                        WalletId = new Guid("86426013-1B44-4F11-81B1-994C79264A9B"),
                        TopicId = new Guid("89F1F22F-2AB6-4998-954D-B4C95D24B7F7")
                    },
                    new Expense()
                    {
                        OwnerId = new Guid("E896E2E1-3C8E-49FB-BCEB-B1C63E56A5DA"),
                        Title = "11 expense",
                        Date = new DateTime(2020, 3, 11), Money = 2100,
                        WalletId = new Guid("86426013-1B44-4F11-81B1-994C79264A9B"),
                        TopicId = new Guid("89F1F22F-2AB6-4998-954D-B4C95D24B7F7")
                    },
                    new Expense()
                    {
                        OwnerId = new Guid("E896E2E1-3C8E-49FB-BCEB-B1C63E56A5DA"),
                        Title = "12 expense",
                        Date = new DateTime(2020, 3, 12), Money = 1100,
                        WalletId = new Guid("86426013-1B44-4F11-81B1-994C79264A9B"),
                        TopicId = new Guid("89F1F22F-2AB6-4998-954D-B4C95D24B7F7")
                    },
                    new Expense()
                    {
                        OwnerId = new Guid("E896E2E1-3C8E-49FB-BCEB-B1C63E56A5DA"),
                        Title = "13 expense",
                        Date = new DateTime(2020, 4, 12), Money = 3100,
                        WalletId = new Guid("86426013-1B44-4F11-81B1-994C79264A9B"),
                        TopicId = new Guid("89F1F22F-2AB6-4998-954D-B4C95D24B7F7")
                    },
                    new Expense()
                    {
                        OwnerId = new Guid("E896E2E1-3C8E-49FB-BCEB-B1C63E56A5DA"),
                        Title = "13 expense",
                        Date = new DateTime(2020, 4, 13), Money = 1100,
                        WalletId = new Guid("86426013-1B44-4F11-81B1-994C79264A9B"),
                        TopicId = new Guid("89F1F22F-2AB6-4998-954D-B4C95D24B7F7")
                    },
                    new Expense()
                    {
                        OwnerId = new Guid("E896E2E1-3C8E-49FB-BCEB-B1C63E56A5DA"),
                        Title = "14 expense",
                        Date = new DateTime(2020, 4, 14), Money = 1400,
                        WalletId = new Guid("86426013-1B44-4F11-81B1-994C79264A9B"),
                        TopicId = new Guid("89F1F22F-2AB6-4998-954D-B4C95D24B7F7")
                    },
                    new Expense()
                    {
                        OwnerId = new Guid("E896E2E1-3C8E-49FB-BCEB-B1C63E56A5DA"),
                        Title = "15 expense",
                        Date = new DateTime(2020, 5, 12), Money = 1100,
                        WalletId = new Guid("86426013-1B44-4F11-81B1-994C79264A9B"),
                        TopicId = new Guid("89F1F22F-2AB6-4998-954D-B4C95D24B7F7")
                    },
                    new Expense()
                    {
                        OwnerId = new Guid("E896E2E1-3C8E-49FB-BCEB-B1C63E56A5DA"),
                        Title = "16 expense",
                        Date = new DateTime(2020, 5, 13), Money = 1100,
                        WalletId = new Guid("86426013-1B44-4F11-81B1-994C79264A9B"),
                        TopicId = new Guid("89F1F22F-2AB6-4998-954D-B4C95D24B7F7")
                    },
                    new Expense()
                    {
                        OwnerId = new Guid("E896E2E1-3C8E-49FB-BCEB-B1C63E56A5DA"),
                        Title = "17 expense",
                        Date = new DateTime(2020, 5, 14), Money = 600,
                        WalletId = new Guid("86426013-1B44-4F11-81B1-994C79264A9B"),
                        TopicId = new Guid("89F1F22F-2AB6-4998-954D-B4C95D24B7F7")
                    },
                    new Expense()
                    {
                        OwnerId = new Guid("E896E2E1-3C8E-49FB-BCEB-B1C63E56A5DA"),
                        Title = "18 expense",
                        Date = new DateTime(2020, 6, 13), Money = 600,
                        WalletId = new Guid("86426013-1B44-4F11-81B1-994C79264A9B"),
                        TopicId = new Guid("89F1F22F-2AB6-4998-954D-B4C95D24B7F7")
                    },
                    new Expense()
                    {
                        OwnerId = new Guid("E896E2E1-3C8E-49FB-BCEB-B1C63E56A5DA"),
                        Title = "19 expense",
                        Date = new DateTime(2020, 6, 14), Money = 600,
                        WalletId = new Guid("86426013-1B44-4F11-81B1-994C79264A9B"),
                        TopicId = new Guid("89F1F22F-2AB6-4998-954D-B4C95D24B7F7")
                    },
                    new Expense()
                    {
                        OwnerId = new Guid("E896E2E1-3C8E-49FB-BCEB-B1C63E56A5DA"),
                        Title = "20 expense",
                        Date = new DateTime(2020, 6, 15), Money = 400,
                        WalletId = new Guid("86426013-1B44-4F11-81B1-994C79264A9B"),
                        TopicId = new Guid("89F1F22F-2AB6-4998-954D-B4C95D24B7F7")
                    },
                    new Expense()
                    {
                        OwnerId = new Guid("E896E2E1-3C8E-49FB-BCEB-B1C63E56A5DA"),
                        Title = "21 expense",
                        Date = new DateTime(2020, 6, 16), Money = 270,
                        WalletId = new Guid("86426013-1B44-4F11-81B1-994C79264A9B"),
                        TopicId = new Guid("89F1F22F-2AB6-4998-954D-B4C95D24B7F7")
                    },
                    new Expense()
                    {
                        OwnerId = new Guid("E896E2E1-3C8E-49FB-BCEB-B1C63E56A5DA"),
                        Title = "22 expense",
                        Date = new DateTime(2020, 12, 8), Money = 340,
                        WalletId = new Guid("748E55F2-440F-480F-9909-EC86CFDEB19E"),
                        TopicId = new Guid("89F1F22F-2AB6-4998-954D-B4C95D24B7F7")
                    },
                    new Expense()
                    {
                        OwnerId = new Guid("E896E2E1-3C8E-49FB-BCEB-B1C63E56A5DA"),
                        Title = "23 expense",
                        Date = new DateTime(2020, 12, 9), Money = 90,
                        WalletId = new Guid("86426013-1B44-4F11-81B1-994C79264A9B"),
                        TopicId = new Guid("4CFF9DA4-EF3C-4AD1-89FB-962A592FD0CD")
                    },
                    new Expense()
                    {
                        OwnerId = new Guid("E896E2E1-3C8E-49FB-BCEB-B1C63E56A5DA"),
                        Title = "24 expense",
                        Date = new DateTime(2020, 12, 10), Money = 40,
                        WalletId = new Guid("86426013-1B44-4F11-81B1-994C79264A9B"),
                        TopicId = new Guid("89F1F22F-2AB6-4998-954D-B4C95D24B7F7")
                    },
                    new Expense()
                    {
                        OwnerId = new Guid("E896E2E1-3C8E-49FB-BCEB-B1C63E56A5DA"),
                        Title = "10 expense",
                        Date = new DateTime(2020, 12, 10), Money = 100,
                        WalletId = new Guid("86426013-1B44-4F11-81B1-994C79264A9B"),
                        TopicId = new Guid("89F1F22F-2AB6-4998-954D-B4C95D24B7F7")
                    },
                    new Expense()
                    {
                        OwnerId = new Guid("E896E2E1-3C8E-49FB-BCEB-B1C63E56A5DA"),
                        Title = "10 expense",
                        Date = new DateTime(2020, 12, 11), Money = 90,
                        WalletId = new Guid("86426013-1B44-4F11-81B1-994C79264A9B"),
                        TopicId = new Guid("89F1F22F-2AB6-4998-954D-B4C95D24B7F7")
                    },
                    new Expense()
                    {
                        OwnerId = new Guid("E896E2E1-3C8E-49FB-BCEB-B1C63E56A5DA"),
                        Title = "10 expense",
                        Date = new DateTime(2020, 12, 12), Money = 150,
                        WalletId = new Guid("86426013-1B44-4F11-81B1-994C79264A9B"),
                        TopicId = new Guid("89F1F22F-2AB6-4998-954D-B4C95D24B7F7")
                    },
                    new Expense()
                    {
                        OwnerId = new Guid("E896E2E1-3C8E-49FB-BCEB-B1C63E56A5DA"),
                        Title = "10 expense",
                        Date = new DateTime(2020, 12, 13), Money = 120,
                        WalletId = new Guid("86426013-1B44-4F11-81B1-994C79264A9B"),
                        TopicId = new Guid("89F1F22F-2AB6-4998-954D-B4C95D24B7F7")
                    },
                    new Expense()
                    {
                        OwnerId = new Guid("E896E2E1-3C8E-49FB-BCEB-B1C63E56A5DA"),
                        Title = "10 expense",
                        Date = new DateTime(2020, 12, 14), Money = 200,
                        WalletId = new Guid("86426013-1B44-4F11-81B1-994C79264A9B"),
                        TopicId = new Guid("89F1F22F-2AB6-4998-954D-B4C95D24B7F7")
                    },
                    new Expense()
                    {
                        OwnerId = new Guid("E896E2E1-3C8E-49FB-BCEB-B1C63E56A5DA"),
                        Title = "10 expense",
                        Date = new DateTime(2020, 12, 9), Money = 230,
                        WalletId = new Guid("86426013-1B44-4F11-81B1-994C79264A9B"),
                        TopicId = new Guid("89F1F22F-2AB6-4998-954D-B4C95D24B7F7")
                    },
                    new Expense()
                    {
                        OwnerId = new Guid("E896E2E1-3C8E-49FB-BCEB-B1C63E56A5DA"),
                        Title = "10 expense",
                        Date = new DateTime(2020, 12, 8), Money = 90,
                        WalletId = new Guid("86426013-1B44-4F11-81B1-994C79264A9B"),
                        TopicId = new Guid("89F1F22F-2AB6-4998-954D-B4C95D24B7F7")
                    },
                    new Expense()
                    {
                        OwnerId = new Guid("E896E2E1-3C8E-49FB-BCEB-B1C63E56A5DA"),
                        Title = "10 expense",
                        Date = new DateTime(2020, 12, 7), Money = 40,
                        WalletId = new Guid("86426013-1B44-4F11-81B1-994C79264A9B"),
                        TopicId = new Guid("89F1F22F-2AB6-4998-954D-B4C95D24B7F7")
                    },
                    new Expense()
                    {
                        OwnerId = new Guid("E896E2E1-3C8E-49FB-BCEB-B1C63E56A5DA"),
                        Title = "10 expense",
                        Date = new DateTime(2020, 12, 6), Money = 50,
                        WalletId = new Guid("86426013-1B44-4F11-81B1-994C79264A9B"),
                        TopicId = new Guid("89F1F22F-2AB6-4998-954D-B4C95D24B7F7")
                    },
                    new Expense()
                    {
                        OwnerId = new Guid("E896E2E1-3C8E-49FB-BCEB-B1C63E56A5DA"),
                        Title = "10 expense",
                        Date = new DateTime(2020, 12, 5), Money = 130,
                        WalletId = new Guid("86426013-1B44-4F11-81B1-994C79264A9B"),
                        TopicId = new Guid("89F1F22F-2AB6-4998-954D-B4C95D24B7F7")
                    },
                    new Expense()
                    {
                        OwnerId = new Guid("E896E2E1-3C8E-49FB-BCEB-B1C63E56A5DA"),
                        Title = "10 expense",
                        Date = new DateTime(2020, 12, 4), Money = 170,
                        WalletId = new Guid("86426013-1B44-4F11-81B1-994C79264A9B"),
                        TopicId = new Guid("89F1F22F-2AB6-4998-954D-B4C95D24B7F7")
                    },
                    new Expense()
                    {
                        OwnerId = new Guid("E896E2E1-3C8E-49FB-BCEB-B1C63E56A5DA"),
                        Title = "10 expense",
                        Date = new DateTime(2020, 12, 3), Money = 150,
                        WalletId = new Guid("86426013-1B44-4F11-81B1-994C79264A9B"),
                        TopicId = new Guid("89F1F22F-2AB6-4998-954D-B4C95D24B7F7")
                    },
                    new Expense()
                    {
                        OwnerId = new Guid("E896E2E1-3C8E-49FB-BCEB-B1C63E56A5DA"),
                        Title = "10 expense",
                        Date = new DateTime(2020, 12, 2), Money = 200,
                        WalletId = new Guid("86426013-1B44-4F11-81B1-994C79264A9B"),
                        TopicId = new Guid("89F1F22F-2AB6-4998-954D-B4C95D24B7F7")
                    },
                    new Expense()
                    {
                        OwnerId = new Guid("E896E2E1-3C8E-49FB-BCEB-B1C63E56A5DA"),
                        Title = "10 expense",
                        Date = new DateTime(2020, 2, 1), Money = 100,
                        WalletId = new Guid("86426013-1B44-4F11-81B1-994C79264A9B"),
                        TopicId = new Guid("89F1F22F-2AB6-4998-954D-B4C95D24B7F7")
                    },
                    new Expense()
                    {
                        OwnerId = new Guid("E896E2E1-3C8E-49FB-BCEB-B1C63E56A5DA"),
                        Title = "10 expense",
                        Date = new DateTime(2020, 12, 10), Money = 160,
                        WalletId = new Guid("748E55F2-440F-480F-9909-EC86CFDEB19E"),
                        TopicId = new Guid("D6A01F6E-CCB5-4FE7-9B10-DDFCCC4F484D")
                    },
                    new Expense()
                    {
                        OwnerId = new Guid("E896E2E1-3C8E-49FB-BCEB-B1C63E56A5DA"),
                        Title = "10 expense",
                        Date = new DateTime(2020, 12, 11), Money = 190,
                        WalletId = new Guid("748E55F2-440F-480F-9909-EC86CFDEB19E"),
                        TopicId = new Guid("D6A01F6E-CCB5-4FE7-9B10-DDFCCC4F484D")
                    },
                    new Expense()
                    {
                        OwnerId = new Guid("E896E2E1-3C8E-49FB-BCEB-B1C63E56A5DA"),
                        Title = "10 expense",
                        Date = new DateTime(2020, 12, 12), Money = 120,
                        WalletId = new Guid("748E55F2-440F-480F-9909-EC86CFDEB19E"),
                        TopicId = new Guid("D6A01F6E-CCB5-4FE7-9B10-DDFCCC4F484D")
                    },
                    new Expense()
                    {
                        OwnerId = new Guid("E896E2E1-3C8E-49FB-BCEB-B1C63E56A5DA"),
                        Title = "10 expense",
                        Date = new DateTime(2020, 12, 13), Money = 150,
                        WalletId = new Guid("748E55F2-440F-480F-9909-EC86CFDEB19E"),
                        TopicId = new Guid("03E9DF21-11F1-43E3-AB86-12D5E746A132")
                    },
                    new Expense()
                    {
                        OwnerId = new Guid("E896E2E1-3C8E-49FB-BCEB-B1C63E56A5DA"),
                        Title = "10 expense",
                        Date = new DateTime(2020, 12, 14), Money = 100,
                        WalletId = new Guid("748E55F2-440F-480F-9909-EC86CFDEB19E"),
                        TopicId = new Guid("03E9DF21-11F1-43E3-AB86-12D5E746A132")
                    },
                    new Expense()
                    {
                        OwnerId = new Guid("E896E2E1-3C8E-49FB-BCEB-B1C63E56A5DA"),
                        Title = "10 expense",
                        Date = new DateTime(2020, 12, 9), Money = 130,
                        WalletId = new Guid("748E55F2-440F-480F-9909-EC86CFDEB19E"),
                        TopicId = new Guid("03E9DF21-11F1-43E3-AB86-12D5E746A132")
                    },
                    new Expense()
                    {
                        OwnerId = new Guid("E896E2E1-3C8E-49FB-BCEB-B1C63E56A5DA"),
                        Title = "10 expense",
                        Date = new DateTime(2020, 12, 8), Money = 30,
                        WalletId = new Guid("748E55F2-440F-480F-9909-EC86CFDEB19E"),
                        TopicId = new Guid("4CFF9DA4-EF3C-4AD1-89FB-962A592FD0CD")
                    },
                    new Expense()
                    {
                        OwnerId = new Guid("E896E2E1-3C8E-49FB-BCEB-B1C63E56A5DA"),
                        Title = "10 expense",
                        Date = new DateTime(2020, 12, 7), Money = 140,
                        WalletId = new Guid("748E55F2-440F-480F-9909-EC86CFDEB19E"),
                        TopicId = new Guid("03E9DF21-11F1-43E3-AB86-12D5E746A132")
                    },
                    new Expense()
                    {
                        OwnerId = new Guid("E896E2E1-3C8E-49FB-BCEB-B1C63E56A5DA"),
                        Title = "10 expense",
                        Date = new DateTime(2020, 12, 6), Money = 150,
                        WalletId = new Guid("748E55F2-440F-480F-9909-EC86CFDEB19E"),
                        TopicId = new Guid("89F1F22F-2AB6-4998-954D-B4C95D24B7F7")
                    },
                    new Expense()
                    {
                        OwnerId = new Guid("E896E2E1-3C8E-49FB-BCEB-B1C63E56A5DA"),
                        Title = "10 expense",
                        Date = new DateTime(2020, 12, 5), Money = 230,
                        WalletId = new Guid("748E55F2-440F-480F-9909-EC86CFDEB19E"),
                        TopicId = new Guid("89F1F22F-2AB6-4998-954D-B4C95D24B7F7")
                    },
                    new Expense()
                    {
                        OwnerId = new Guid("E896E2E1-3C8E-49FB-BCEB-B1C63E56A5DA"),
                        Title = "10 expense",
                        Date = new DateTime(2020, 12, 4), Money = 70,
                        WalletId = new Guid("748E55F2-440F-480F-9909-EC86CFDEB19E"),
                        TopicId = new Guid("89F1F22F-2AB6-4998-954D-B4C95D24B7F7")
                    },
                    new Expense()
                    {
                        OwnerId = new Guid("E896E2E1-3C8E-49FB-BCEB-B1C63E56A5DA"),
                        Title = "10 expense",
                        Date = new DateTime(2020, 12, 3), Money = 50,
                        WalletId = new Guid("748E55F2-440F-480F-9909-EC86CFDEB19E"),
                        TopicId = new Guid("4CFF9DA4-EF3C-4AD1-89FB-962A592FD0CD")
                    },
                    new Expense()
                    {
                        OwnerId = new Guid("E896E2E1-3C8E-49FB-BCEB-B1C63E56A5DA"),
                        Title = "10 expense",
                        Date = new DateTime(2020, 12, 2), Money = 100,
                        WalletId = new Guid("748E55F2-440F-480F-9909-EC86CFDEB19E"),
                        TopicId = new Guid("89F1F22F-2AB6-4998-954D-B4C95D24B7F7")
                    },
                    new Expense()
                    {
                        OwnerId = new Guid("E896E2E1-3C8E-49FB-BCEB-B1C63E56A5DA"),
                        Title = "10 expense",
                        Date = new DateTime(2020, 2, 1), Money = 800,
                        WalletId = new Guid("748E55F2-440F-480F-9909-EC86CFDEB19E"),
                        TopicId = new Guid("89F1F22F-2AB6-4998-954D-B4C95D24B7F7")
                    },
                    new Expense()
                    {
                        OwnerId = new Guid("E896E2E1-3C8E-49FB-BCEB-B1C63E56A5DA"),
                        Title = "101 expense",
                        Date = new DateTime(2020, 4, 1), Money = 1300,
                        WalletId = new Guid("748E55F2-440F-480F-9909-EC86CFDEB19E"),
                        TopicId = new Guid("89F1F22F-2AB6-4998-954D-B4C95D24B7F7")
                    },
                    new Expense()
                    {
                        OwnerId = new Guid("E896E2E1-3C8E-49FB-BCEB-B1C63E56A5DA"),
                        Title = "102 expense",
                        Date = new DateTime(2020, 5, 1), Money = 1800,
                        WalletId = new Guid("748E55F2-440F-480F-9909-EC86CFDEB19E"),
                        TopicId = new Guid("89F1F22F-2AB6-4998-954D-B4C95D24B7F7")
                    },
                    new Expense()
                    {
                        OwnerId = new Guid("E896E2E1-3C8E-49FB-BCEB-B1C63E56A5DA"),
                        Title = "103 expense",
                        Date = new DateTime(2020, 6, 1), Money = 2000,
                        WalletId = new Guid("748E55F2-440F-480F-9909-EC86CFDEB19E"),
                        TopicId = new Guid("89F1F22F-2AB6-4998-954D-B4C95D24B7F7")
                    },
                    new Expense()
                    {
                        OwnerId = new Guid("E896E2E1-3C8E-49FB-BCEB-B1C63E56A5DA"),
                        Title = "104 expense",
                        Date = new DateTime(2020, 7, 1), Money = 2100,
                        WalletId = new Guid("748E55F2-440F-480F-9909-EC86CFDEB19E"),
                        TopicId = new Guid("89F1F22F-2AB6-4998-954D-B4C95D24B7F7")
                    },
                    new Expense()
                    {
                        OwnerId = new Guid("E896E2E1-3C8E-49FB-BCEB-B1C63E56A5DA"),
                        Title = "105 expense",
                        Date = new DateTime(2020, 8, 1), Money = 1800,
                        WalletId = new Guid("748E55F2-440F-480F-9909-EC86CFDEB19E"),
                        TopicId = new Guid("03E9DF21-11F1-43E3-AB86-12D5E746A132")
                    },
                    new Expense()
                    {
                        OwnerId = new Guid("E896E2E1-3C8E-49FB-BCEB-B1C63E56A5DA"),
                        Title = "106 expense",
                        Date = new DateTime(2020, 9, 1), Money = 1600,
                        WalletId = new Guid("748E55F2-440F-480F-9909-EC86CFDEB19E"),
                        TopicId = new Guid("03E9DF21-11F1-43E3-AB86-12D5E746A132")
                    },
                    new Expense()
                    {
                        OwnerId = new Guid("E896E2E1-3C8E-49FB-BCEB-B1C63E56A5DA"),
                        Title = "107 expense",
                        Date = new DateTime(2020, 10, 1), Money = 1800,
                        WalletId = new Guid("748E55F2-440F-480F-9909-EC86CFDEB19E"),
                        TopicId = new Guid("03E9DF21-11F1-43E3-AB86-12D5E746A132")
                    },
                    new Expense()
                    {
                        OwnerId = new Guid("E896E2E1-3C8E-49FB-BCEB-B1C63E56A5DA"),
                        Title = "108 expense",
                        Date = new DateTime(2020, 11, 1), Money = 1900,
                        WalletId = new Guid("748E55F2-440F-480F-9909-EC86CFDEB19E"),
                        TopicId = new Guid("03E9DF21-11F1-43E3-AB86-12D5E746A132")
                    },
                    new Expense()
                    {
                        OwnerId = new Guid("E896E2E1-3C8E-49FB-BCEB-B1C63E56A5DA"),
                        Title = "109 expense",
                        Date = new DateTime(2020, 7, 1), Money = 1900,
                        WalletId = new Guid("86426013-1B44-4F11-81B1-994C79264A9B"),
                        TopicId = new Guid("4CFF9DA4-EF3C-4AD1-89FB-962A592FD0CD")
                    },
                    new Expense()
                    {
                        OwnerId = new Guid("E896E2E1-3C8E-49FB-BCEB-B1C63E56A5DA"),
                        Title = "110 expense",
                        Date = new DateTime(2020, 8, 1), Money = 800,
                        WalletId = new Guid("86426013-1B44-4F11-81B1-994C79264A9B"),
                        TopicId = new Guid("D6A01F6E-CCB5-4FE7-9B10-DDFCCC4F484D")
                    },
                    new Expense()
                    {
                        OwnerId = new Guid("E896E2E1-3C8E-49FB-BCEB-B1C63E56A5DA"),
                        Title = "111 expense",
                        Date = new DateTime(2020, 9, 1), Money = 1200,
                        WalletId = new Guid("86426013-1B44-4F11-81B1-994C79264A9B"),
                        TopicId = new Guid("03E9DF21-11F1-43E3-AB86-12D5E746A132")
                    },
                    new Expense()
                    {
                        OwnerId = new Guid("E896E2E1-3C8E-49FB-BCEB-B1C63E56A5DA"),
                        Title = "112 expense",
                        Date = new DateTime(2020, 10, 1), Money = 1100,
                        WalletId = new Guid("86426013-1B44-4F11-81B1-994C79264A9B"),
                        TopicId = new Guid("89F1F22F-2AB6-4998-954D-B4C95D24B7F7")
                    },
                    new Expense()
                    {
                        OwnerId = new Guid("E896E2E1-3C8E-49FB-BCEB-B1C63E56A5DA"),
                        Title = "113 expense",
                        Date = new DateTime(2020, 11, 1), Money = 1500,
                        WalletId = new Guid("86426013-1B44-4F11-81B1-994C79264A9B"),
                        TopicId = new Guid("89F1F22F-2AB6-4998-954D-B4C95D24B7F7")
                    },
                };

                await repository.AddRangeAsync(expenses);
                await repository.SaveChangesAsync();
            }
        }
    }
}
