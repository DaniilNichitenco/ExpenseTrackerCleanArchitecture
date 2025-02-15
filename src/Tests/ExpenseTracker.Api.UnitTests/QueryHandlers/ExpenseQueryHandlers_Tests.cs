﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExpenseTracker.Api.TestsCommon;
using ExpenseTracker.Api.TestsCommon.Data;
using ExpenseTracker.Api.UnitTests.ClassTestData;
using ExpenseTracker.Core.Application.Interfaces;
using ExpenseTracker.Core.Application.Queries.ExpenseQueries;
using ExpenseTracker.Core.Application.QueryableBuilders;
using ExpenseTracker.Core.Application.QueryHandlers.Expenses;
using ExpenseTracker.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace ExpenseTracker.Api.UnitTests.QueryHandlers
{
    public class ExpenseQueryHandlers_Tests : CommonBaseTestClassFixture
    {
        private readonly Mock<IGenericRepository<Expense>> _expenseRepository;
        private readonly Mock<ExpensesBuilder> _expensesBuilder;
        private readonly GetUserExpensesQueryHandler _getUserExpensesQueryHandler;
        private readonly GetExpensesSumForDayQueryHandler _getExpensesSumForDayQueryHandler;
        private readonly GetExpensesSumForMonthQueryHandler _getExpensesSumForMonthQueryHandler;
        private readonly GetExpensesSumForYearQueryHandler _getExpensesSumForYearQueryHandler;
        private readonly GetExpensesSumPerDayForMonthHandler _getExpensesSumPerDayForMonthHandler;
            
        public ExpenseQueryHandlers_Tests()
        {
            _expenseRepository = new Mock<IGenericRepository<Expense>>();
            _expensesBuilder = new Mock<ExpensesBuilder>();
            _getUserExpensesQueryHandler = new GetUserExpensesQueryHandler(_expenseRepository.Object, _mapper);
            _getExpensesSumForDayQueryHandler = new GetExpensesSumForDayQueryHandler(_expenseRepository.Object, _expensesBuilder.Object);
            _getExpensesSumForMonthQueryHandler =
                new GetExpensesSumForMonthQueryHandler(_expenseRepository.Object, _expensesBuilder.Object);
            _getExpensesSumForYearQueryHandler = new GetExpensesSumForYearQueryHandler(_expenseRepository.Object, _expensesBuilder.Object);
            _getExpensesSumPerDayForMonthHandler =
                new GetExpensesSumPerDayForMonthHandler(_expenseRepository.Object, _expensesBuilder.Object);

            var expenses = ExpenseData.GetExpenses();

            _expenseRepository.Setup(x => x.Read()).Returns(expenses.AsQueryable().BuildMock().Object);
        }
        
        [Theory]
        [ClassData(typeof(UserIdData))]
        public async Task GetUserExpensesQueryHandler_ShouldReturnOnlyUserExpenses(Guid userId)
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
        public async Task GetExpensesSumForDayQueryHandler_ShouldReturnOnlyForOneDateUserExpenses(Guid userId, DateTime date)
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
        public async Task GetExpensesSumForMonthQueryHandler_ShouldReturnOnlyForOneMonthUserExpenses(Guid userId, DateTime date)
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
        public async Task GetExpensesSumForYearQueryHandler_ShouldReturnOnlyForOneYearUserExpenses(Guid userId, DateTime date)
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
        public async Task GetExpensesSumPerDayForMonthHandler_ShouldReturnOnlyUserExpensesSumForOneMonthPerEachDay(Guid userId, DateTime date)
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