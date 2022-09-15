using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HR.LeaveManagement.Identity.Migrations
{
    public partial class IdentityMigrationFirst : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "572a6611-0b92-41e4-a50b-2054dce996f7",
                column: "ConcurrencyStamp",
                value: "c718413e-a7f7-4490-b620-81ad71f92a20");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "afec160e-c2ff-42d9-86e5-679dd20b7fed",
                column: "ConcurrencyStamp",
                value: "6581c44a-af3b-4ce9-8c91-6639cf6033ec");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3044d54b-a65f-46c3-85a3-4657465f8ee2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "754f6f51-04fe-48bc-b918-69366f6bcdbf", "AQAAAAEAACcQAAAAEAQFK6XjoIoO0IIfDQa3q8wzF+4O/w+RcfK5GBGKCyiLSrW+gK+nuuxVEv9x29VHpQ==", "22d864a0-0f96-426f-a14c-d4c97ce03e75" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b15b2a6c-21ee-4f45-ba28-f33a333fbb48",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e8380700-3cde-48d0-9912-2c36e8fae2dc", "AQAAAAEAACcQAAAAELL5W5y7QWVZnKY7T+ejHJaM/mc95X+Cwa9MHGo2KXC8a0bSD0hHThpaiO4DjjjDtQ==", "387e51ae-f13e-458a-9e5f-26ce1304c950" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "572a6611-0b92-41e4-a50b-2054dce996f7",
                column: "ConcurrencyStamp",
                value: "b711ee82-1062-43a8-a764-013e8bb78894");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "afec160e-c2ff-42d9-86e5-679dd20b7fed",
                column: "ConcurrencyStamp",
                value: "38e223c7-93ef-4bb3-9a82-bc255a5c6a78");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3044d54b-a65f-46c3-85a3-4657465f8ee2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b8fbf177-67ac-4dff-945b-83f861042f3c", "AQAAAAEAACcQAAAAEFhECHRn0zdU71SBnuNuF6iF5JTclBVu1+8wbZHbLKvEMhSUaWvPQ7iYDLQlHzaJeg==", "e73016b4-07b2-4f65-8ec8-8ebdc34121e7" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b15b2a6c-21ee-4f45-ba28-f33a333fbb48",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d55cab33-fa9c-4c0b-ab4c-4410af53953a", "AQAAAAEAACcQAAAAENGvbqAfFvxHQbscoWO8DcJ9XjFMND+6NSRtqvEUs4IbQ2H9YQZOGADvK/fuKA9pnw==", "a534ea42-319e-4624-977f-b369cd03d57c" });
        }
    }
}
