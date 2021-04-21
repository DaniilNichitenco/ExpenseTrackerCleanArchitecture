using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Xunit2;
using ExpenseTracker.Api.TestsCommon;
using ExpenseTracker.Api.UnitTests.ClassTestData;
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
        private readonly GetExpensesSumForYearQueryHandler _getExpensesSumForYearQueryHandler;
        private readonly GetExpensesSumPerDayForMonthHandler _getExpensesSumPerDayForMonthHandler;
            
        public ExpenseQueryHandlers_Tests()
        {
            _expenseRepository = new Mock<IEFRepository<Expense>>();
            _getUserExpensesQueryHandler = new GetUserExpensesQueryHandler(_expenseRepository.Object, Mapper);
            _getExpensesSumForDayQueryHandler = new GetExpensesSumForDayQueryHandler(_expenseRepository.Object);
            _getExpensesSumForMonthQueryHandler =
                new GetExpensesSumForMonthQueryHandler(_expenseRepository.Object);
            _getExpensesSumForYearQueryHandler = new GetExpensesSumForYearQueryHandler(_expenseRepository.Object);
            _getExpensesSumPerDayForMonthHandler = new GetExpensesSumPerDayForMonthHandler(_expenseRepository.Object);

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
            
            Assert.All(expensesDto, expense =>
            {
                Assert.Contains(expenses, e => e.Id == expense.Id);
            });
                
            Assert.All(expenses, expense =>
            {
                Assert.Contains(expensesDto, e => e.Id == expense.Id);
            });
        }

        [Theory]
        [ClassData(typeof(UserDateData))]
        public async Task GetExpensesSumForDayQueryHandler_ShouldReturnOnlyForOneDateUserExpenses(int userId, DateTime date)
        {
            var expensesSumDto = await _getExpensesSumForDayQueryHandler.Handle(new GetExpensesSumForDayQuery
                {
                    UserId = userId,
                    Date = date
                },
                CancellationToken.None);
            
            var expenses = _expenseRepository.Object.Read()
                .Where(x => x.Date.Date == date.Date
                            && x.OwnerId == userId
                );
            
            
            Assert.Equal(expenses.Sum(x => x.Money), expensesSumDto.Sum(x => x.Sum));
        }

        [Theory]
        [ClassData(typeof(UserDateData))]
        public async Task GetExpensesSumForMonthQueryHandler_ShouldReturnOnlyForOneMonthUserExpenses(int userId, DateTime date)
        {
            
            var expensesSumDto = await _getExpensesSumForMonthQueryHandler.Handle(new GetExpensesSumForMonthQuery
                {
                    UserId = userId,
                    Date = date
                },
                CancellationToken.None);
            
            var expenses = _expenseRepository.Object.Read()
                .Where(x => x.Date.Month == date.Month
                            && x.Date.Year == date.Year
                            && x.OwnerId == userId
                            );
            Assert.Equal(expenses.Sum(x => x.Money), expensesSumDto.Sum(x => x.Sum));
        }
        
        [Theory]
        [ClassData(typeof(UserDateData))]
        public async Task GetExpensesSumForYearQueryHandler_ShouldReturnOnlyForOneYearUserExpenses(int userId, DateTime date)
        {
            
            var expensesSumDto = await _getExpensesSumForYearQueryHandler.Handle(new GetExpensesSumForYearQuery
                {
                    UserId = userId,
                    Date = date
                },
                CancellationToken.None);
            
            
            
            var expenses = _expenseRepository.Object.Read()
                .Where(x => x.Date.Year == date.Year
                            && x.OwnerId == userId
                            );
            
            Assert.Equal(expenses.Sum(x => x.Money), expensesSumDto.Sum(x => x.Sum));
        }
        
        [Theory]
        [ClassData(typeof(UserDateData))]
        public async Task GetExpensesSumPerDayForMonthHandler_ShouldReturnOnlyUserExpensesSumForOneMonthPerEachDay(int userId, DateTime date)
        {
            var countDays = DateTime.DaysInMonth(date.Year, date.Month);
            
            var expensesDto = await _getExpensesSumPerDayForMonthHandler.Handle(new GetExpensesSumPerDayForMonth
            {
                UserId = userId, 
                Date = date
            }, CancellationToken.None);
            
            var expenses = await _expenseRepository.Object.Read()
                .Include(x => x.Wallet)
                .Where(x => x.OwnerId == userId
                && x.Date.Year == date.Year
                && x.Date.Month == date.Month
                )
                .GroupBy(x => x.WalletId)
                .Select(x => new
                {
                    WalletId = x.Key,
                    x.FirstOrDefault().Wallet.CurrencyCode,
                    Expenses = x.GroupBy(x => new { x.Date.Day })
                })
                .ToListAsync();
            
            Assert.All(expensesDto, expenseSum =>
            {
                Assert.Equal(expenseSum.Expenses.Count(), countDays);
            });
            
            Assert.All(expenses, expenseSum =>
            {
                var expenseDto = expensesDto.FirstOrDefault(x => x.CurrencyCode == expenseSum.CurrencyCode);
                Assert.NotNull(expenseDto);
                
                Assert.All(expenseSum.Expenses, expense =>
                {
                    Assert.Equal(expense.Sum(x => x.Money), expenseDto.Expenses.Sum(x => x.Sum));
                });
            });
        }
    }
}