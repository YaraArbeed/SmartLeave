using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartLeave.DAL.Migrations
{
    public partial class MakeApprovedByIdNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "45deb9d6-c1ae-44a0-03b-c9a5cfc15f2f",
                column: "ConcurrencyStamp",
                value: "e6e069b6-40d7-4b7f-a566-61f1640115e7");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "639de03f-7876-4fff-96ec-37f8bd3bf180",
                column: "ConcurrencyStamp",
                value: "058f8ea3-4ec4-4d9b-8ef5-c3ffd2f75d41");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e7f2b8d4-3c19-4e5a-9a7d-2fb8c6e117a3",
                column: "ConcurrencyStamp",
                value: "f832bc6a-4982-4d41-a28e-332affb8aff0");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "45deb9d6-c1ae-44a0-03b-c9a5cfc15f2f",
                column: "ConcurrencyStamp",
                value: "73d05187-afd7-4bb7-bd81-88cd10f7ebd7");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "639de03f-7876-4fff-96ec-37f8bd3bf180",
                column: "ConcurrencyStamp",
                value: "ef412365-4b67-44b6-85d7-fe314d5aab62");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e7f2b8d4-3c19-4e5a-9a7d-2fb8c6e117a3",
                column: "ConcurrencyStamp",
                value: "93b3a4a4-989e-414d-81c6-768a7ab9f315");
        }
    }
}
