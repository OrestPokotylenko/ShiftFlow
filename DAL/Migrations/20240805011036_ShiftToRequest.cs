using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class ShiftToRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ShiftId",
                table: "Requests",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Requests_ShiftId",
                table: "Requests",
                column: "ShiftId");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Shifts_ShiftId",
                table: "Requests",
                column: "ShiftId",
                principalTable: "Shifts",
                principalColumn: "ShiftId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Shifts_ShiftId",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_ShiftId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "ShiftId",
                table: "Requests");
        }
    }
}
