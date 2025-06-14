using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartLeave.DAL.Migrations
{
    public partial class ApprovedByIdNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ApprovedById",
                table: "Leaves",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "45deb9d6-c1ae-44a0-03b-c9a5cfc15f2f",
                column: "ConcurrencyStamp",
                value: "ec6b2285-ff6a-42c7-a3da-0d71564f0078");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "639de03f-7876-4fff-96ec-37f8bd3bf180",
                column: "ConcurrencyStamp",
                value: "f725b07a-b8e2-414f-904d-c51ece4344a0");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e7f2b8d4-3c19-4e5a-9a7d-2fb8c6e117a3",
                column: "ConcurrencyStamp",
                value: "9b1a026e-967d-4abb-b4c6-0db7a3995407");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ApprovedById",
                table: "Leaves",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

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
    }
}
