using HR.LeaveManagement.Application.DTOs.LeaveRequest;
using HR.LeaveManagement.Application.Features.LeaveRequests.Requests.Commands;
using HR.LeaveManagement.Application.Features.LeaveRequests.Requests.Queries;
using HR.LeaveManagement.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HR.LeaveManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveRequestsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LeaveRequestsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpGet]
        public async Task<ActionResult<List<LeaveRequestListDTO>>> Get(bool isLoggedInUser = false)
        {
            var query = new GetLeaveRequestListRequest() { IsLoggedInUser = isLoggedInUser};
            var leaveRequests = await _mediator.Send(query);

            return Ok(leaveRequests);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LeaveRequestDTO>> Get(int id)
        {
            var query = new GetLeaveRequestDetailRequest();
            var leaveRequest = await _mediator.Send(query);

            return Ok(leaveRequest);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<BaseCommandResponse>> Post([FromBody] CreateLeaveRequestDTO createLeaveRequestDTO)
        {
            var command = new CreateLeaveRequestCommand { CreateLeaveRequestDTO = createLeaveRequestDTO };
            var response = await _mediator.Send(command);

            return Ok(response);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Put(int id, [FromBody] UpdateLeaveRequestDTO updateLeaveRequestDTO)
        {
            var command = new UpdateLeaveRequestCommand { Id = id, UpdateLeaveRequestDTO = updateLeaveRequestDTO };
            await _mediator.Send(command);

            return NoContent();
        }

        [HttpPut("changeapproval/{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] ChangeLeaveRequestApprovalDTO changeLeaveRequestApprovalDTO)
        {
            var command = new UpdateLeaveRequestCommand { Id = id, ChangeLeaveRequestApprovalDTO = changeLeaveRequestApprovalDTO };
            await _mediator.Send(command);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Delete(int id)
        {
            var command = new DeleteLeaveRequestCommand { Id = id };
            await _mediator.Send(command);

            return NoContent();
        }
    }
}
