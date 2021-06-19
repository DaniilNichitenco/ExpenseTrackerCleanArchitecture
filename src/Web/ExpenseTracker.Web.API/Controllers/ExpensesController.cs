using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ExpenseTracker.Core.Application.Interfaces;
using ExpenseTracker.Core.Application.Queries.ExpenseQueries;
using ExpenseTracker.Core.Domain.Dtos.Expenses;
using ExpenseTracker.Core.Domain.Entities;
using ExpenseTracker.Core.Domain.ViewModels;
using ExpenseTracker.Infrastructure.Repository.API.Authorization.Attributes;
using ExpenseTracker.Infrastructure.Repository.Shared.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
        [HttpGet("sum/{date}")]
        public async Task<ActionResult<IEnumerable<ExpensesSumViewModel>>> GetSumOfExpensesForDay([FromRoute] DateTime date, CancellationToken cancellationToken)
        {
            var userId = User.GetClaim("id");
            if (userId == null)
            {
                return Forbid();
            }

            var expenses = await _mediator.Send(new GetExpensesSumForDayQuery
            {
                UserId = new Guid(userId.Value),
                Date = date
            }, cancellationToken);

            var result = _mapper.Map<IEnumerable<ExpensesSumViewModel>>(expenses);

            return Ok(result);
        }

        [Read]
        [HttpGet("sum/month/{date}")]
        public async Task<ActionResult<IEnumerable<ExpensesSumViewModel>>> GetSumOfExpensesForMonth([FromRoute] DateTime date, CancellationToken cancellationToken)
        {
            var userId = User.GetClaim("id");
            if (userId == null)
            {
                return Forbid();
            }

            var expenses = await _mediator.Send(new GetExpensesSumForMonthQuery
            {
                UserId = new Guid(userId.Value),
                Date = date
            }, cancellationToken);

            var result = _mapper.Map<IEnumerable<ExpensesSumViewModel>>(expenses);

            return Ok(result);
        }
        
        [Read]
        [HttpGet("sum/year/{date}")]
        public async Task<ActionResult<IEnumerable<ExpensesSumViewModel>>> GetSumOfExpensesForYear([FromRoute] DateTime date, CancellationToken cancellationToken)
        {
            var userId = User.GetClaim("id");
            if (userId == null)
            {
                return Forbid();
            }

            var expenses = await _mediator.Send(new GetExpensesSumForYearQuery
            {
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
