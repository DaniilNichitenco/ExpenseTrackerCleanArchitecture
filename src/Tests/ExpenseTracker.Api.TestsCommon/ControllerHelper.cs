using MediatR;
using Moq;

namespace ExpenseTracker.Api.TestsCommon
{
    public class ControllerHelper : CommonBaseTestClassFixture
    {
        protected readonly Mock<IMediator> _mediator;

        public ControllerHelper()
        {
            _mediator = new Mock<IMediator>();
        }
    }
}