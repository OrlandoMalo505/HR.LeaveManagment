using AutoMapper;
using HR.LeaveManagement.Application.DTOs.LeaveRequest;
using HR.LeaveManagement.Application.Features.LeaveRequests.Requests.Queries;
using HR.LeaveManagement.Application.Contracts.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Domain;
using HR.LeaveManagement.Application.Constants;

namespace HR.LeaveManagement.Application.Features.LeaveRequests.Handlers.Queries
{
    public class GetLeaveRequestListRequestHandler : IRequestHandler<GetLeaveRequestListRequest, List<LeaveRequestListDTO>>
    {
        private readonly ILeaveRequestRepository _requestRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _user;

        public GetLeaveRequestListRequestHandler(ILeaveRequestRepository requestRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor, IUserService user)
        {
            _requestRepository = requestRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _user = user;
        }
        public async Task<List<LeaveRequestListDTO>> Handle(GetLeaveRequestListRequest request, CancellationToken cancellationToken)
        {
            var leaveRequests = new List<LeaveRequest>();
            var requests = new List<LeaveRequestListDTO>();

            if (request.IsLoggedInUser)
            {
                var userId = _httpContextAccessor.HttpContext.User.FindFirst(q => q.Type == CustomClaimTypes.Uid)?.Value;

                leaveRequests = await _requestRepository.GetLeaveRequestsWithDetails(userId);

                var employee = await _user.GetEmployee(userId);

                requests = _mapper.Map<List<LeaveRequestListDTO>>(leaveRequests);

                foreach (var req in requests)
                {
                    req.Employee = employee;
                }

            }
            else
            {
                leaveRequests = await _requestRepository.GetLeaveRequestsWithDetails();
                requests = _mapper.Map<List<LeaveRequestListDTO>>(leaveRequests);

                foreach (var req in requests)
                {
                    req.Employee = await _user.GetEmployee(req.RequestingEmployeeId);
                }
            }
            return requests;
        }
    }
}
