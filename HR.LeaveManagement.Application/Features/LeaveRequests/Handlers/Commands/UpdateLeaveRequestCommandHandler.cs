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
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateLeaveRequestCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<Unit> Handle(UpdateLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            var leaveRequest = await _unitOfWork.LeaveRequestRepository.Get(request.Id);
            if (leaveRequest == null)
                throw new NotFoundException(nameof(leaveRequest), request.UpdateLeaveRequestDTO.Id);
            if(request.UpdateLeaveRequestDTO != null)
            {
                var validator = new UpdateLeaveRequestDTOValidator(_unitOfWork.LeaveTypeRepository);
                var validationResult = await validator.ValidateAsync(request.UpdateLeaveRequestDTO);

                if (validationResult.IsValid == false)
                    throw new ValidationException(validationResult);
                await _unitOfWork.LeaveRequestRepository.Update(leaveRequest);
                await _unitOfWork.Save();
            }
            else if (request.ChangeLeaveRequestApprovalDTO is not null)
            {
                await _unitOfWork.LeaveRequestRepository.ChangeApprovalStatus(leaveRequest, request.ChangeLeaveRequestApprovalDTO.Approved);

                if (request.ChangeLeaveRequestApprovalDTO.Approved)
                {
                    var allocation = await _unitOfWork.LeaveAllocationRepository.GetUserAllocations(leaveRequest.RequestingEmployeeId, leaveRequest.LeaveTypeId);
                    int daysRequested = (int)(leaveRequest.EndDate - leaveRequest.StartDate).TotalDays;

                    allocation.NumberOfDays -= daysRequested;

                    await _unitOfWork.LeaveAllocationRepository.Update(allocation);
                }

                await _unitOfWork.Save();
            }

            return Unit.Value;
        }
    }
}
