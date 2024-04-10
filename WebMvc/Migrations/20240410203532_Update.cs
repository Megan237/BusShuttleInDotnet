using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebMvc.Migrations
{
    /// <inheritdoc />
    public partial class Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BusId",
                table: "Entry",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DriverId",
                table: "Entry",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LoopId",
                table: "Entry",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StopId",
                table: "Entry",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Entry_BusId",
                table: "Entry",
                column: "BusId");

            migrationBuilder.CreateIndex(
                name: "IX_Entry_DriverId",
                table: "Entry",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_Entry_LoopId",
                table: "Entry",
                column: "LoopId");

            migrationBuilder.CreateIndex(
                name: "IX_Entry_StopId",
                table: "Entry",
                column: "StopId");

            migrationBuilder.AddForeignKey(
                name: "FK_Entry_Bus_BusId",
                table: "Entry",
                column: "BusId",
                principalTable: "Bus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Entry_Driver_DriverId",
                table: "Entry",
                column: "DriverId",
                principalTable: "Driver",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Entry_Loop_LoopId",
                table: "Entry",
                column: "LoopId",
                principalTable: "Loop",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Entry_Stop_StopId",
                table: "Entry",
                column: "StopId",
                principalTable: "Stop",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entry_Bus_BusId",
                table: "Entry");

            migrationBuilder.DropForeignKey(
                name: "FK_Entry_Driver_DriverId",
                table: "Entry");

            migrationBuilder.DropForeignKey(
                name: "FK_Entry_Loop_LoopId",
                table: "Entry");

            migrationBuilder.DropForeignKey(
                name: "FK_Entry_Stop_StopId",
                table: "Entry");

            migrationBuilder.DropIndex(
                name: "IX_Entry_BusId",
                table: "Entry");

            migrationBuilder.DropIndex(
                name: "IX_Entry_DriverId",
                table: "Entry");

            migrationBuilder.DropIndex(
                name: "IX_Entry_LoopId",
                table: "Entry");

            migrationBuilder.DropIndex(
                name: "IX_Entry_StopId",
                table: "Entry");

            migrationBuilder.DropColumn(
                name: "BusId",
                table: "Entry");

            migrationBuilder.DropColumn(
                name: "DriverId",
                table: "Entry");

            migrationBuilder.DropColumn(
                name: "LoopId",
                table: "Entry");

            migrationBuilder.DropColumn(
                name: "StopId",
                table: "Entry");
        }
    }
}
