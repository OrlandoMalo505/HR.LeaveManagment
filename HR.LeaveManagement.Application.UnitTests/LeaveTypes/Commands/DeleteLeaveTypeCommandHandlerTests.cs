using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Features.LeaveTypes.Handlers.Commands;
using HR.LeaveManagement.Application.Features.LeaveTypes.Requests.Commands;
using HR.LeaveManagement.Application.Profiles;
using HR.LeaveManagement.Application.UnitTests.Mock;
using HR.LeaveManagement.Application.UnitTests.Mocks;
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
        private readonly Mock<IUnitOfWork> _mockUow;
        private readonly IMapper _mapper;
        public DeleteLeaveTypeCommandHandlerTests()
        {
            _mockUow = MockUnitOfWork.GetUnitOfWork();

            var mapperConfig = new MapperConfiguration(m => m.AddProfile<MappingProfile>());
            _mapper = mapperConfig.CreateMapper();
        }

        [Fact]
        public async Task DeleteLeaveType_WithExistingLeaveType_ReturnNoContent()
        {
 
            var handler = new DeleteLeaveTypeCommandHandler(_mockUow.Object, _mapper);

            var result = await handler.Handle(new DeleteLeaveTypeCommand { Id = 3}, CancellationToken.None);

            result.ShouldBeOfType<Unit>();
   
        }

        [Fact]
        public async Task DeleteLeaveType_WithNoExistingLeaveType_ReturnsNotFound()
        {
            var handler = new DeleteLeaveTypeCommandHandler(_mockUow.Object, _mapper);

            NotFoundException ex = await Should.ThrowAsync<NotFoundException>(
                async () => await handler.Handle(new DeleteLeaveTypeCommand { Id = 1 }, CancellationToken.None));

            ex.ShouldNotBeNull();
        }

    }
}
