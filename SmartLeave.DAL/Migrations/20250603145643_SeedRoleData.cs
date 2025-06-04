using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartLeave.DAL.Migrations
{
    public partial class SeedRoleData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offices_Countries_CountryId",
                table: "Offices");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "45deb9d6-c1ae-44a0-03b-c9a5cfc15f2f", "7c2013f6-bdc4-45a5-b163-0076decbdaa9", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "639de03f-7876-4fff-96ec-37f8bd3bf180", "a8140022-3354-45b7-a441-17116da254c3", "Employee", "EMPLOYEE" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "e7f2b8d4-3c19-4e5a-9a7d-2fb8c6e117a3", "7471c5bf-7caa-4392-9a33-0902a2727062", "Manager", "MANAGER" });

            migrationBuilder.AddForeignKey(
                name: "FK_Offices_Countries_CountryId",
                table: "Offices",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offices_Countries_CountryId",
                table: "Offices");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "45deb9d6-c1ae-44a0-03b-c9a5cfc15f2f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "639de03f-7876-4fff-96ec-37f8bd3bf180");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e7f2b8d4-3c19-4e5a-9a7d-2fb8c6e117a3");

            migrationBuilder.AddForeignKey(
                name: "FK_Offices_Countries_CountryId",
                table: "Offices",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
