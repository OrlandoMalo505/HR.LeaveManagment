using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Features.LeaveTypes.Handlers.Commands;
using HR.LeaveManagement.Application.Features.LeaveTypes.Requests.Commands;
using HR.LeaveManagement.Application.Profiles;
using HR.LeaveManagement.Application.UnitTests.Mock;
using HR.LeaveManagement.Domain;
using MediatR;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HR.LeaveManagement.Application.UnitTests.LeaveTypes.Commands
{
    public class DeleteLeaveTypeCommandHandlerTests
    {
        private readonly Mock<ILeaveTypeRepository> _mockRepo;
        private readonly IMapper _mapper;
        public DeleteLeaveTypeCommandHandlerTests()
        {
            _mockRepo = MockLeaveTypeRepository.GetLeaveTypeRepository();

            var mapperConfig = new MapperConfiguration(m => m.AddProfile<MappingProfile>());
            _mapper = mapperConfig.CreateMapper();
        }

        [Fact]
        public async Task DeleteLeaveType_WithExistingLeaveType_ReturnNoContent()
        {
            var leaveType = new LeaveType {Id = 1, DefaultDays = 10, Name = "Test Delete" };

            var handler = new DeleteLeaveTypeCommandHandler(_mockRepo.Object, _mapper);

            var result = await handler.Handle(new DeleteLeaveTypeCommand { Id = leaveType.Id}, CancellationToken.None);

            result.ShouldBeOfType<Unit>();
   
        }

    }
}
