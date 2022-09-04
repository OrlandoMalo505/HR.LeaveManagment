using FluentValidation;
using HR.LeaveManagement.Application.Persistence.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.DTOs.LeaveAllocation.Validators
{
    public class CreateLeaveAllocationDTOValidator : AbstractValidator<CreateLeaveAllocationDTO>
    {
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        public CreateLeaveAllocationDTOValidator(ILeaveTypeRepository leaveTypeRepository)
        {
            _leaveTypeRepository = leaveTypeRepository;
            Include(new ILeaveAllocationDTOValidator(_leaveTypeRepository));
        }
    }
}
