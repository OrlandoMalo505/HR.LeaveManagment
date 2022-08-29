using AutoMapper;
using HR.LeaveManagement.Application.DTOs.LeaveRequest;
using HR.LeaveManagement.Application.Features.LeaveRequests.Requests.Queries;
using HR.LeaveManagement.Application.Persistence.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Features.LeaveRequests.Handlers.Queries
{
    public class GetLeaveRequestDetailRequestHandler : IRequestHandler<GetLeaveRequestDetailRequest, LeaveRequestDTO>
    {
        private readonly ILeaveRequestRepository _requestRepository;
        private readonly IMapper _mapper;

        public GetLeaveRequestDetailRequestHandler(ILeaveRequestRepository requestRepository, IMapper mapper)
        {
            _requestRepository = requestRepository;
            _mapper = mapper;
        }
        public async Task<LeaveRequestDTO> Handle(GetLeaveRequestDetailRequest request, CancellationToken cancellationToken)
        {
            var leaveRequest = await _requestRepository.GetLeaveRequestWithDetails(request.Id);
            return _mapper.Map<LeaveRequestDTO>(leaveRequest);
        }
    }
}
