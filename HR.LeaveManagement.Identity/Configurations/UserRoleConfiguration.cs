using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Identity.Configurations
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            builder.HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "afec160e-c2ff-42d9-86e5-679dd20b7fed",
                    UserId = "3044d54b-a65f-46c3-85a3-4657465f8ee2"
                },
                new IdentityUserRole<string>
                {
                    RoleId = "572a6611-0b92-41e4-a50b-2054dce996f7",
                    UserId = "b15b2a6c-21ee-4f45-ba28-f33a333fbb48"
                });
        }
    }
}
