using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartLeave.DAL.Migrations
{
    public partial class MakeManagerIdFieldsNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "45deb9d6-c1ae-44a0-03b-c9a5cfc15f2f",
                column: "ConcurrencyStamp",
                value: "08b3a3e4-93bc-4d19-882f-7970bce349cb");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "639de03f-7876-4fff-96ec-37f8bd3bf180",
                column: "ConcurrencyStamp",
                value: "8a53ce74-1dba-4e53-ad36-b595048a5fb6");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e7f2b8d4-3c19-4e5a-9a7d-2fb8c6e117a3",
                column: "ConcurrencyStamp",
                value: "0902975d-d8d0-4022-9d5e-858b3aea4236");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "45deb9d6-c1ae-44a0-03b-c9a5cfc15f2f",
                column: "ConcurrencyStamp",
                value: "3fe844cb-1feb-466a-880a-a53e7ea8cb40");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "639de03f-7876-4fff-96ec-37f8bd3bf180",
                column: "ConcurrencyStamp",
                value: "47288ccb-1869-4156-9a10-750df981a291");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e7f2b8d4-3c19-4e5a-9a7d-2fb8c6e117a3",
                column: "ConcurrencyStamp",
                value: "f50a528b-5250-4cea-9b0d-73c6263342b7");
        }
    }
}
