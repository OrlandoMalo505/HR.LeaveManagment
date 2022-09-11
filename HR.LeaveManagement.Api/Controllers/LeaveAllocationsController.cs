﻿using HR.LeaveManagement.Application.DTOs.LeaveAllocation;
using HR.LeaveManagement.Application.Features.LeaveAllocations.Requests.Commands;
using HR.LeaveManagement.Application.Features.LeaveAllocations.Requests.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.Xml;

namespace HR.LeaveManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveAllocationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LeaveAllocationsController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet]
        public async Task<ActionResult<List<LeaveAllocationDTO>>> Get()
        {
            var query = new GetLeaveAllocationListRequest();
            var leaveAllocations = await _mediator.Send(query);
            return Ok(leaveAllocations);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LeaveAllocationDTO>> Get(int id)
        {
            var query = new GetLeaveAllocationDetailRequest { Id = id };
            var leaveAllocation = await _mediator.Send(query);
            return Ok(leaveAllocation);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreateLeaveAllocationDTO createLeaveAllocationDTO)
        {
            var command = new CreateLeaveAllocationCommand { CreateLeaveAllocationDTO = createLeaveAllocationDTO };
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPut]
        public async Task<ActionResult> Put([FromBody] UpdateLeaveAllocationDTO updateLeaveAllocationDTO)
        {
            var command = new UpdateLeaveAllocationCommand { UpdateLeaveAllocationDTO = updateLeaveAllocationDTO };
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var command = new DeleteLeaveAllocationCommand { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
