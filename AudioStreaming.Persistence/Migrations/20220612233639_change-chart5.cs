using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AudioStreaming.Persistence.Migrations
{
    public partial class changechart5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrackId",
                table: "Chart");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TrackId",
                table: "Chart",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
