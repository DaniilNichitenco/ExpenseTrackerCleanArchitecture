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
using ExpenseTracker.Infrastructure.API.Authorization.Attributes;
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
            var id = User.Claims.FirstOrDefault(x => x.Type == "id");
            if (id == null)
            {
                return Forbid();
            }
            
            var expenses = await _mediator.Send(new GetUserExpensesQuery { UserId = long.Parse(id.Value) }, cancellationToken);
            
            var result = _mapper.Map<IEnumerable<ExpenseViewModel>>(expenses);
            
            return Ok(result);
        }

        [Read]
        [HttpGet("sum/{date}")]
        public async Task<IActionResult> GetSumOfExpensesForDay([FromRoute] DateTime date, CancellationToken cancellationToken)
        {
            var id = User.Claims.FirstOrDefault(x => x.Type == "id");
            if (id == null)
            {
                return Forbid();
            }

            var expenses = await _mediator.Send(new GetExpensesSumForDayQuery
            {
                UserId = long.Parse(id.Value),
                Date = date
            }, cancellationToken);

            var result = _mapper.Map<IEnumerable<ExpensesSumViewModel>>(expenses);

            return Ok(result);
        }
    }
}
