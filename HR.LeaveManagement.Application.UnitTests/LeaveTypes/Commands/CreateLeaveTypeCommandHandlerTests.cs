using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.DTOs.LeaveType;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Features.LeaveTypes.Handlers.Commands;
using HR.LeaveManagement.Application.Features.LeaveTypes.Requests.Commands;
using HR.LeaveManagement.Application.Profiles;
using HR.LeaveManagement.Application.UnitTests.Mock;
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
    public class CreateLeaveTypeCommandHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<ILeaveTypeRepository> _mockRepo;
        private readonly CreateLeaveTypeDTO _leaveTypeDTO;
        private readonly CreateLeaveTypeRequestHandler _handler;
        public CreateLeaveTypeCommandHandlerTests()
        {
            _mockRepo = MockLeaveTypeRepository.GetLeaveTypeRepository();

            var mapperConfig = new MapperConfiguration(m => m.AddProfile<MappingProfile>());
            _mapper = mapperConfig.CreateMapper();

            _handler = new CreateLeaveTypeRequestHandler(_mockRepo.Object, _mapper);

            _leaveTypeDTO = new CreateLeaveTypeDTO
            { 
                DefaultDays = 20,
                Name = "Test DTO"
            };
        }

        [Fact]
        public async Task Valid_LeaveType_Added()
        {
            var result = await _handler.Handle(new CreateLeaveTypeCommand() { LeaveTypeDTO = _leaveTypeDTO}, CancellationToken.None);

            var leaveTypes = await _mockRepo.Object.GetAll();

            result.ShouldBeOfType<int>();

            leaveTypes.Count.ShouldBe(3);
        }

        [Fact]
        public async Task Invalid_LeaveType_Added()
        {
            _leaveTypeDTO.DefaultDays = -1;

            ValidationException ex = await Should.ThrowAsync<ValidationException>(
                async () => await _handler.Handle(new CreateLeaveTypeCommand() { LeaveTypeDTO = _leaveTypeDTO }, CancellationToken.None));

            var leaveTypes = await _mockRepo.Object.GetAll();

            leaveTypes.Count.ShouldBe(2);
            ex.ShouldNotBeNull();
        }
    }
}
