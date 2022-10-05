using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.DTOs.LeaveType;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Features.LeaveTypes.Handlers.Commands;
using HR.LeaveManagement.Application.Features.LeaveTypes.Requests.Commands;
using HR.LeaveManagement.Application.Profiles;
using HR.LeaveManagement.Application.UnitTests.Mock;
using HR.LeaveManagement.Application.UnitTests.Mocks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
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
    public class UpdateLeaveTypeCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUow;
        private readonly IMapper _mapper;
        private readonly LeaveTypeDTO _leaveTypeDTO;
        public UpdateLeaveTypeCommandHandlerTests()
        {
            _mockUow = MockUnitOfWork.GetUnitOfWork();

            var mapperConfig = new MapperConfiguration(m => m.AddProfile<MappingProfile>());
            _mapper = new Mapper(mapperConfig);

            _leaveTypeDTO = new LeaveTypeDTO
            {
                Id = 1,
                DefaultDays = 2,
                Name = "Update Test"
            };
        }

        [Fact]
        public async Task Update_Valid_LeaveType_ReturnNoContent()
        {
            var handler = new UpdateLeaveTypeCommandHandler(_mockUow.Object, _mapper);

            var result = await handler.Handle(new UpdateLeaveTypeCommand { LeaveTypeDTO = _leaveTypeDTO }, CancellationToken.None);

            result.ShouldBeOfType<Unit>();
        }

        [Fact]
        public async Task Update_Invalid_LeaveType_ReturnValidationException()
        {
            _leaveTypeDTO.DefaultDays = -1;

            var handler = new UpdateLeaveTypeCommandHandler(_mockUow.Object, _mapper);

            ValidationException ex = await Should.ThrowAsync<ValidationException>(
                async () => await handler.Handle(new UpdateLeaveTypeCommand() { LeaveTypeDTO = _leaveTypeDTO }, CancellationToken.None));

            ex.ShouldNotBeNull();
        }
    }
}
