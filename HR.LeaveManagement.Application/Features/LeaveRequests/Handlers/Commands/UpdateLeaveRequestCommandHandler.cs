using AutoMapper;
using HR.LeaveManagement.Application.DTOs.LeaveRequest.Validators;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Features.LeaveRequests.Requests.Commands;
using HR.LeaveManagement.Application.Contracts.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Features.LeaveRequests.Handlers.Commands
{
    public class UpdateLeaveRequestCommandHandler : IRequestHandler<UpdateLeaveRequestCommand, Unit>
    {
        private readonly ILeaveRequestRepository _requestRepository;
        private readonly IMapper _mapper;
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly ILeaveAllocationRepository _allocationRepository;

        public UpdateLeaveRequestCommandHandler(ILeaveRequestRepository requestRepository, IMapper mapper, ILeaveTypeRepository leaveTypeRepository, ILeaveAllocationRepository allocationRepository)
        {
            _requestRepository = requestRepository;
            _mapper = mapper;
            _leaveTypeRepository = leaveTypeRepository;
            _allocationRepository = allocationRepository;
        }
        public async Task<Unit> Handle(UpdateLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            var leaveRequest = await _requestRepository.Get(request.Id);
            if(request.UpdateLeaveRequestDTO != null)
            {
                var validator = new UpdateLeaveRequestDTOValidator(_leaveTypeRepository);
                var validationResult = await validator.ValidateAsync(request.UpdateLeaveRequestDTO);

                if (validationResult.IsValid == false)
                    throw new ValidationException(validationResult);
                await _requestRepository.Update(leaveRequest);
            }
            else if (request.ChangeLeaveRequestApprovalDTO is not null)
            {
                await _requestRepository.ChangeApprovalStatus(leaveRequest, request.ChangeLeaveRequestApprovalDTO.Approved);

                if (request.ChangeLeaveRequestApprovalDTO.Approved)
                {
                    var allocation = await _allocationRepository.GetUserAllocations(leaveRequest.RequestingEmployeeId, leaveRequest.LeaveTypeId);
                    int daysRequested = (int)(leaveRequest.EndDate - leaveRequest.StartDate).TotalDays;

                    allocation.NumberOfDays -= daysRequested;

                    await _allocationRepository.Update(allocation);
                }
            }

            return Unit.Value;
        }
    }
}
