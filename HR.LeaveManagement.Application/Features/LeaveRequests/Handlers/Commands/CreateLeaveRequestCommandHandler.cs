using AutoMapper;
using HR.LeaveManagement.Application.DTOs.LeaveRequest.Validators;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Features.LeaveRequests.Requests.Commands;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Responses;
using HR.LeaveManagement.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HR.LeaveManagement.Application.Models;
using HR.LeaveManagement.Application.Contracts.Infrastructure;
using Microsoft.AspNetCore.Http;
using HR.LeaveManagement.Application.Models.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using HR.LeaveManagement.Application.Constants;

namespace HR.LeaveManagement.Application.Features.LeaveRequests.Handlers.Commands
{
    public class CreateLeaveRequestCommandHandler : IRequestHandler<CreateLeaveRequestCommand, BaseCommandResponse>
    {
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;

        public CreateLeaveRequestCommandHandler(IMapper mapper, IEmailSender emailSender, IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _emailSender = emailSender;
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
        }
        public async Task<BaseCommandResponse> Handle(CreateLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var validator = new CreateLeaveRequestDTOValidator(_unitOfWork.LeaveTypeRepository);
            var validationResult = await validator.ValidateAsync(request.CreateLeaveRequestDTO);
            var userId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(q => q.Type == CustomClaimTypes.Uid)?.Value;

            var allocations = await _unitOfWork.LeaveAllocationRepository.GetUserAllocations(userId, request.CreateLeaveRequestDTO.LeaveTypeId);
            if(allocations is null)
            {
                validationResult.Errors.Add(new FluentValidation.Results.ValidationFailure(nameof(request.CreateLeaveRequestDTO.LeaveTypeId), "You don't have any allocation for this leave type."));
            }
            else
            {
                int dayRequested = (int)(request.CreateLeaveRequestDTO.EndDate - request.CreateLeaveRequestDTO.StartDate).TotalDays;

                if (dayRequested > allocations.NumberOfDays)
                {
                    validationResult.Errors.Add(new FluentValidation.Results.ValidationFailure(nameof(request.CreateLeaveRequestDTO.EndDate), "You don't have enough days for this request."));
                }
            }
            
            if (validationResult.IsValid == false)
            {
                response.Success = false;
                response.Message = "Creation Failed";
                response.Errors = validationResult.Errors.Select(err => err.ErrorMessage).ToList();
            }
            else
            {
                var leaveRequest = _mapper.Map<LeaveRequest>(request.CreateLeaveRequestDTO);
                leaveRequest.RequestingEmployeeId = userId;
                leaveRequest = await _unitOfWork.LeaveRequestRepository.Add(leaveRequest);
                await _unitOfWork.Save();

                response.Success = true;
                response.Message = "Creation Successful";
                response.Id = leaveRequest.Id;

                var emailAddress = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email).Value;

                var email = new Email
                {
                    To = emailAddress,
                    Subject = "Leave Request Submitted",
                    Body = $@"Your leave request for {request.CreateLeaveRequestDTO.StartDate:D} to {request.CreateLeaveRequestDTO.EndDate:D} has been submitted successfully"
                };

                try
                {
                    await _emailSender.SendEmail(email);
                }
                catch (Exception ex)
                {

                    //
                } 
            }
 
            return response;
        }
    }
}
