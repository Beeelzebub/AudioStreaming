using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AudioStreaming.Persistence.Migrations
{
    public partial class changechart3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Chart_TrackId",
                table: "Chart");

            migrationBuilder.AddColumn<int>(
                name: "PositionInChart",
                table: "Track",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Chart_TrackId",
                table: "Chart",
                column: "TrackId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Chart_TrackId",
                table: "Chart");

            migrationBuilder.DropColumn(
                name: "PositionInChart",
                table: "Track");

            migrationBuilder.CreateIndex(
                name: "IX_Chart_TrackId",
                table: "Chart",
                column: "TrackId",
                unique: true);
        }
    }
}
