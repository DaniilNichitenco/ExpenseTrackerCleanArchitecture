using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using ExpenseTracker.Api.TestsCommon;
using ExpenseTracker.Api.TestsCommon.Data;
using ExpenseTracker.Core.Application.Interfaces;
using ExpenseTracker.Core.Application.Queries.ExpenseQueries;
using ExpenseTracker.Core.Application.QueryHandlers.Expenses;
using ExpenseTracker.Core.Domain.Entities;
using ExpenseTracker.Core.Domain.ViewModels;
using ExpenseTracker.Web.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace ExpenseTracker.Api.UnitTests.Controllers
{
    public class ExpensesController_Tests : ControllerHelper
    {
        private readonly Mock<IEFRepository<Expense>> _expenseRepository;
        private readonly ExpensesController _controller;
        
        public ExpensesController_Tests()
        {
            _expenseRepository = new Mock<IEFRepository<Expense>>();
            _controller = new ExpensesController(_mediator.Object, _mapper)
            {
                ControllerContext = {HttpContext = new DefaultHttpContext()}
            };
            
            _expenseRepository.Setup(x => x.Read()).Returns(ExpenseData.GetExpenses().AsQueryable().BuildMock().Object);
            
            _mediator.Setup(x => x.Send(It.IsAny<GetUserExpensesQuery>(), It.IsAny<CancellationToken>()))
                .Returns((GetUserExpensesQuery query, CancellationToken cancellationToken) => new GetUserExpensesQueryHandler(_expenseRepository.Object, _mapper).Handle(
                    query, cancellationToken));
            
        }

        [Theory]
        [AutoData]
        public async Task GetExpenses_ShouldReturnEnumerableOfViewModelType([Range(1,3)]int userId)
        {
            _controller.HttpContext.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
            {
                new ClaimsIdentity(new List<Claim>()
                {
                    new Claim("id", userId.ToString())
                })
            });
            
            var result = await _controller.GetExpenses(CancellationToken.None);
            
            Assert.IsType(typeof(OkObjectResult), result.Result);
            Assert.IsAssignableFrom(typeof(IEnumerable<ExpenseViewModel>), (result.Result as OkObjectResult).Value);
        }
    }
}