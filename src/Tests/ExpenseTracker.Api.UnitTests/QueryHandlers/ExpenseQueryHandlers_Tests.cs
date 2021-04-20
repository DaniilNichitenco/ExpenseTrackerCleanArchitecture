using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Xunit2;
using ExpenseTracker.Api.TestsCommon;
using ExpenseTracker.Core.Application.Interfaces;
using ExpenseTracker.Core.Application.Queries.ExpenseQueries;
using ExpenseTracker.Core.Application.QueryHandlers.Expenses;
using ExpenseTracker.Core.Domain.Entities;
using JetBrains.ReSharper.TestRunner.Abstractions.Extensions;
using JetBrains.ReSharper.TestRunner.Adapters.XUnit.Extensions;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace ExpenseTracker.Api.UnitTests.QueryHandlers
{
    public class ExpenseQueryHandlers_Tests : CommonBaseTestClassFixture
    {
        private readonly Mock<IEFRepository<Expense>> _expenseRepository;
        private readonly GetUserExpensesQueryHandler _getUserExpensesQueryHandler;
        private readonly GetExpensesSumForDayQueryHandler _getExpensesSumForDayQueryHandler;
        private readonly GetExpensesSumForMonthQueryHandler _getExpensesSumForMonthQueryHandler;
            
        public ExpenseQueryHandlers_Tests()
        {
            _expenseRepository = new Mock<IEFRepository<Expense>>();
            _getUserExpensesQueryHandler = new GetUserExpensesQueryHandler(_expenseRepository.Object, Mapper);
            _getExpensesSumForDayQueryHandler = new GetExpensesSumForDayQueryHandler(_expenseRepository.Object);
            _getExpensesSumForMonthQueryHandler =
                new GetExpensesSumForMonthQueryHandler(_expenseRepository.Object, Mapper);

            var expenses = GetExpenses();

            _expenseRepository.Setup(x => x.Read()).Returns(expenses.AsQueryable().BuildMock().Object);
        }

        private static IEnumerable<Expense> GetExpenses()
        {
            var wallets = GetWallets();
                
            var expenses = Fixture.Build<Expense>()
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

        private static IEnumerable<Wallet> GetWallets()
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
        
        [Theory]
        [AutoData]
        public async Task GetUserExpensesQueryHandler_ShouldReturnOnlyUserExpenses([Range(1,3)]int userId)
        {
            var expensesDto = await _getUserExpensesQueryHandler.Handle(new GetUserExpensesQuery{UserId = userId}, CancellationToken.None);
            var expenses = await _expenseRepository.Object.Read().Where(x => x.OwnerId == userId).ToListAsync();
            
            if (expensesDto.Any() || expenses.Any())
            {
                Assert.All(expensesDto, expense =>
                {
                    Assert.Contains(expenses, e => e.Id == expense.Id);
                });
                
                Assert.All(expenses, expense =>
                {
                    Assert.Contains(expensesDto, e => e.Id == expense.Id);
                });
            }
        }

        [Theory]
        [AutoData]
        public async Task GetUserExpensesQueryHandler_ShouldReturnOnlyForOneDateUserExpenses([Range(1,3)]int userId)
        {
            var date1 = new DateTime(2020, 9, 15);
            var date2 = new DateTime(2019, 6, 19);
            var date3 = DateTime.Today;
            
            var expensesSumDto1 = await _getExpensesSumForDayQueryHandler.Handle(new GetExpensesSumForDayQuery
                {
                    UserId = userId,
                    Date = date1
                },
                CancellationToken.None);
            
            var expensesSumDto2 = await _getExpensesSumForDayQueryHandler.Handle(new GetExpensesSumForDayQuery
                {
                    UserId = userId,
                    Date = date2
                },
                CancellationToken.None);
            
            var expensesSumDto3 = await _getExpensesSumForDayQueryHandler.Handle(new GetExpensesSumForDayQuery
                {
                    UserId = userId,
                    Date = date3
                },
                CancellationToken.None);
            
            var sum1 = _expenseRepository.Object.Read()
                .Where(x => x.Date.Date == date1.Date
                            && x.OwnerId == userId
                ).Sum(x => x.Money);
            var sum2 = _expenseRepository.Object.Read()
                .Where(x => x.Date.Date == date2.Date
                            && x.OwnerId == userId
                ).Sum(x => x.Money);
            var sum3 = _expenseRepository.Object.Read()
                .Where(x => x.Date.Date == date3.Date
                            && x.OwnerId == userId
                ).Sum(x => x.Money);
            
            Assert.Equal(sum1, expensesSumDto1.Sum(x => x.Sum));
            Assert.Equal(sum2, expensesSumDto2.Sum(x => x.Sum));
            Assert.Equal(sum3, expensesSumDto3.Sum(x => x.Sum));
        }

        [Theory]
        [AutoData]
        public async Task GetExpensesSumForMonthQueryHandler_ShouldReturnOnlyForOneMonthUserExpenses([Range(1,3)]int userId)
        {
            var date1 = new DateTime(2020, 9, 15);
            var date2 = new DateTime(2019, 6, 19);
            var date3 = DateTime.Today;
            
            var expensesSumDto1 = await _getExpensesSumForMonthQueryHandler.Handle(new GetExpensesSumForMonthQuery
                {
                    UserId = userId,
                    Date = date1
                },
                CancellationToken.None);
            
            var expensesSumDto2 = await _getExpensesSumForMonthQueryHandler.Handle(new GetExpensesSumForMonthQuery
                {
                    UserId = userId,
                    Date = date2
                },
                CancellationToken.None);
            
            var expensesSumDto3 = await _getExpensesSumForMonthQueryHandler.Handle(new GetExpensesSumForMonthQuery
                {
                    UserId = userId,
                    Date = date3
                },
                CancellationToken.None);
            
            var sum1 = _expenseRepository.Object.Read()
                .Where(x => x.Date.Month == date1.Month
                            && x.Date.Year == date1.Year
                            && x.OwnerId == userId
                ).Sum(x => x.Money);
            var sum2 = _expenseRepository.Object.Read()
                .Where(x => x.Date.Month == date2.Month
                            && x.Date.Year == date2.Year
                            && x.OwnerId == userId
                ).Sum(x => x.Money);
            var sum3 = _expenseRepository.Object.Read()
                .Where(x => x.Date.Month == date3.Month
                            && x.Date.Year == date3.Year
                            && x.OwnerId == userId
                ).Sum(x => x.Money);
            
            Assert.Equal(sum1, expensesSumDto1.Sum(x => x.Sum));
            Assert.Equal(sum2, expensesSumDto2.Sum(x => x.Sum));
            Assert.Equal(sum3, expensesSumDto3.Sum(x => x.Sum));
        }
    }
}