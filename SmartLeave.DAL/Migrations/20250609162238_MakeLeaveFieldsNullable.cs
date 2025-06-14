using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartLeave.DAL.Migrations
{
    public partial class MakeLeaveFieldsNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AttachmentUrl",
                table: "Leaves",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "ApprovalNote",
                table: "Leaves",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AttachmentUrl",
                table: "Leaves",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ApprovalNote",
                table: "Leaves",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "45deb9d6-c1ae-44a0-03b-c9a5cfc15f2f",
                column: "ConcurrencyStamp",
                value: "b7dfd0b2-429b-4826-b513-9111948f9c2a");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "639de03f-7876-4fff-96ec-37f8bd3bf180",
                column: "ConcurrencyStamp",
                value: "b90b2caf-8ba7-4b5d-a1ec-97eb7ce1cb26");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e7f2b8d4-3c19-4e5a-9a7d-2fb8c6e117a3",
                column: "ConcurrencyStamp",
                value: "25033842-ebbd-43ac-955f-5554df8f9191");
        }
    }
}
