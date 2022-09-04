using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.DTOs.LeaveType.Validators
{
    public class UpdateLeaveTypeDTOValidator : AbstractValidator<LeaveTypeDTO>
    {
        public UpdateLeaveTypeDTOValidator()
        {
            Include(new ILeaveTypeDTOValidator());

            RuleFor(p => p.Id)
                .NotNull()
                .WithMessage("{PropertyName} must be present.");
        }
    }
}
