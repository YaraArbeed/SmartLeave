using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartLeave.DAL.Migrations
{
    public partial class MakeUserFieldsNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ManagerId",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "45deb9d6-c1ae-44a0-03b-c9a5cfc15f2f",
                column: "ConcurrencyStamp",
                value: "b6d9214c-d890-4e87-bf73-b0ee80d3d5fe");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "639de03f-7876-4fff-96ec-37f8bd3bf180",
                column: "ConcurrencyStamp",
                value: "0f384251-bdf9-4b04-81a8-bcabe4496666");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e7f2b8d4-3c19-4e5a-9a7d-2fb8c6e117a3",
                column: "ConcurrencyStamp",
                value: "717e27ac-5c45-47c5-88c1-948176e355e5");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ManagerId",
                table: "AspNetUsers",
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
                value: "8a98ca94-2aa8-41b6-8467-52592f097110");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "639de03f-7876-4fff-96ec-37f8bd3bf180",
                column: "ConcurrencyStamp",
                value: "adf09dfe-b0ef-47f7-8200-1f74e75dbe0f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e7f2b8d4-3c19-4e5a-9a7d-2fb8c6e117a3",
                column: "ConcurrencyStamp",
                value: "8f88298d-aba8-4c30-8acc-2004f55b1b3f");
        }
    }
}
