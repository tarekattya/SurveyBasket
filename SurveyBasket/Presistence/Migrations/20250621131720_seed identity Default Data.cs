using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SurveyBasket.Presistence.Migrations
{
    /// <inheritdoc />
    public partial class seedidentityDefaultData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ProfileImageContentType",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "IsDefault", "IsDeleted", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "019791d7-92ab-7022-8774-1eecdba1f2a2", "019791d9c60a7d889dab3f7809e740f3", false, false, "Admin", "ADMIN" },
                    { "019791d7-da52-70db-96ec-87c909bc811a", "019791d864df7179a5d6d15c06eb1e83", true, false, "Member", "MEMBER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ProfileImage", "ProfileImageContentType", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "019791ae-3c34-726a-8b42-c4b2a8f511ed", 0, "019791af89de76b5928115f69be7e708", "Admin@Survey-Basket.com", true, "Survey Basket", "Admin", false, null, "ADMIN@SURVEY-BASKET.COM", "ADMIN@SURVEY-BASKET.COM", "AQAAAAIAAYagAAAAEFoBZtZrBXuTETtpN2af//gQ1MSZXe55JL1W9DvO/E9344/QpahxmhAMcjCkA7Zrqg==", null, false, null, "", "019791af-20da-7916-a88b-06f1dbc5ab92", false, "Admin@Survey-Basket.com" });

            migrationBuilder.InsertData(
                table: "AspNetRoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[,]
                {
                    { 1, "permissions", "polls:read", "019791d7-92ab-7022-8774-1eecdba1f2a2" },
                    { 2, "permissions", "polls:add", "019791d7-92ab-7022-8774-1eecdba1f2a2" },
                    { 3, "permissions", "polls:update", "019791d7-92ab-7022-8774-1eecdba1f2a2" },
                    { 4, "permissions", "polls:delete", "019791d7-92ab-7022-8774-1eecdba1f2a2" },
                    { 5, "permissions", "questions:read", "019791d7-92ab-7022-8774-1eecdba1f2a2" },
                    { 6, "permissions", "questions:add", "019791d7-92ab-7022-8774-1eecdba1f2a2" },
                    { 7, "permissions", "questions:update", "019791d7-92ab-7022-8774-1eecdba1f2a2" },
                    { 8, "permissions", "users:read", "019791d7-92ab-7022-8774-1eecdba1f2a2" },
                    { 9, "permissions", "users:add", "019791d7-92ab-7022-8774-1eecdba1f2a2" },
                    { 10, "permissions", "users:update", "019791d7-92ab-7022-8774-1eecdba1f2a2" },
                    { 11, "permissions", "roles:read", "019791d7-92ab-7022-8774-1eecdba1f2a2" },
                    { 12, "permissions", "roles:add", "019791d7-92ab-7022-8774-1eecdba1f2a2" },
                    { 13, "permissions", "roles:update", "019791d7-92ab-7022-8774-1eecdba1f2a2" },
                    { 14, "permissions", "results:read", "019791d7-92ab-7022-8774-1eecdba1f2a2" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "019791d7-92ab-7022-8774-1eecdba1f2a2", "019791ae-3c34-726a-8b42-c4b2a8f511ed" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "019791d7-da52-70db-96ec-87c909bc811a");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "019791d7-92ab-7022-8774-1eecdba1f2a2", "019791ae-3c34-726a-8b42-c4b2a8f511ed" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "019791d7-92ab-7022-8774-1eecdba1f2a2");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "019791ae-3c34-726a-8b42-c4b2a8f511ed");

            migrationBuilder.AlterColumn<string>(
                name: "ProfileImageContentType",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);
        }
    }
}
