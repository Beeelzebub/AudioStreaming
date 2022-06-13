using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AudioStreaming.Persistence.Migrations
{
    public partial class changechart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chart_Track_TrackId",
                table: "Chart");

            migrationBuilder.DropIndex(
                name: "IX_Chart_TrackId",
                table: "Chart");

            migrationBuilder.AddColumn<int>(
                name: "PositionInChart",
                table: "Track",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Track_PositionInChart",
                table: "Track",
                column: "PositionInChart",
                unique: true,
                filter: "[PositionInChart] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Track_Chart_PositionInChart",
                table: "Track",
                column: "PositionInChart",
                principalTable: "Chart",
                principalColumn: "Position");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Track_Chart_PositionInChart",
                table: "Track");

            migrationBuilder.DropIndex(
                name: "IX_Track_PositionInChart",
                table: "Track");

            migrationBuilder.DropColumn(
                name: "PositionInChart",
                table: "Track");

            migrationBuilder.CreateIndex(
                name: "IX_Chart_TrackId",
                table: "Chart",
                column: "TrackId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Chart_Track_TrackId",
                table: "Chart",
                column: "TrackId",
                principalTable: "Track",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
