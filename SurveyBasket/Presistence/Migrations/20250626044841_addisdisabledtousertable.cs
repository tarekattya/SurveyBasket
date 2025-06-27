using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurveyBasket.Presistence.Migrations
{
    /// <inheritdoc />
    public partial class addisdisabledtousertable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDisable",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "019791ae-3c34-726a-8b42-c4b2a8f511ed",
                column: "IsDisable",
                value: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDisable",
                table: "AspNetUsers");
        }
    }
}
