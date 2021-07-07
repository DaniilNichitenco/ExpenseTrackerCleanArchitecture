using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ExpenseTracker.Core.Application.Queries.ExpenseQueries;
using ExpenseTracker.Core.Domain.Enums;
using ExpenseTracker.Core.Domain.ViewModels;
using ExpenseTracker.Infrastructure.Repository.API.Authorization.Attributes;
using ExpenseTracker.Infrastructure.Repository.Shared.Extensions;
using MediatR;

namespace ExpenseTracker.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public ExpensesController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [Read]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExpenseViewModel>>> GetExpenses(CancellationToken cancellationToken)
        {
            var userId = User.GetClaim("id");
            if (userId == null)
            {
                return Forbid();
            }

            var expenses = await _mediator.Send(new GetUserExpensesQuery {UserId = new Guid(userId.Value)},
                cancellationToken);
            
            var result = _mapper.Map<IEnumerable<ExpenseViewModel>>(expenses);
            
            return Ok(result);
        }

        [Read]
        [HttpGet("sum/{expensesForPeriod}/{date}")]
        public async Task<ActionResult<IEnumerable<ExpensesSumViewModel>>> GetSumOfExpensesForPeriod([FromRoute] ExpensesForPeriod expensesForPeriod, [FromRoute] DateTime date, CancellationToken cancellationToken)
        {
            var userId = User.GetClaim("id");
            if (userId == null)
            {
                return Forbid();
            }

            var expenses = await _mediator.Send(new GetExpensesForPeriodQuery
            {
                ExpensesForPeriod = expensesForPeriod,
                UserId = new Guid(userId.Value),
                Date = date
            }, cancellationToken);

            var result = _mapper.Map<IEnumerable<ExpensesSumViewModel>>(expenses);

            return Ok(result);
        }

        [Read]
        [HttpGet("sum/per-day/month/{date}")]
        public async Task<ActionResult<IEnumerable<ExpensesSumPerDayViewModel>>> GetSumOfExpensesPerDayForMonth(
            [FromRoute] DateTime date, CancellationToken cancellationToken)
        {
            var userId = User.GetClaim("id");
            if (userId == null)
            {
                return Forbid();
            }

            var expenses = await _mediator.Send(new GetExpensesSumPerDayForMonth
            {
                Date = date,
                UserId = new Guid(userId.Value)
            }, cancellationToken);

            var result = _mapper.Map<ExpensesSumPerDayViewModel>(expenses);

            return Ok(result);
        }
    }
}
