using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Kernel;
using AutoFixture.Xunit2;
using ExpenseTracker.Api.TestsCommon;
using ExpenseTracker.Core.Application.Interfaces;
using ExpenseTracker.Core.Application.Queries.ExpenseQueries;
using ExpenseTracker.Core.Application.QueryHandlers;
using ExpenseTracker.Core.Domain.Dtos.Expenses;
using ExpenseTracker.Core.Domain.Entities;
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
            
        public ExpenseQueryHandlers_Tests()
        {
            _expenseRepository = new Mock<IEFRepository<Expense>>();
            _getUserExpensesQueryHandler = new GetUserExpensesQueryHandler(_expenseRepository.Object, Mapper);

            var expenses = Fixture.Build<Expense>()
                .Without(x => x.Wallet)
                .Without(x => x.Topic)
                .With(x => x.OwnerId, () => new Random().Next(1,3))
                .CreateMany<Expense>(100);

            _expenseRepository.Setup(x => x.Read()).Returns(expenses.AsQueryable().BuildMock().Object);
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
    }
}