using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AudioStreaming.Persistence.Migrations
{
    public partial class changelisteninghistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ListeningHistory_AspNetUsers_UserId",
                table: "ListeningHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_Permission_AspNetUsers_UserId",
                table: "Permission");

            migrationBuilder.DropForeignKey(
                name: "FK_Permission_Playlist_PlaylistId",
                table: "Permission");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ListeningHistory",
                table: "ListeningHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Permission",
                table: "Permission");

            migrationBuilder.RenameTable(
                name: "Permission",
                newName: "PlaylistPermission");

            migrationBuilder.RenameIndex(
                name: "IX_Permission_UserId",
                table: "PlaylistPermission",
                newName: "IX_PlaylistPermission_UserId");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ListeningHistory",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ListeningHistory",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ListeningHistory",
                table: "ListeningHistory",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlaylistPermission",
                table: "PlaylistPermission",
                columns: new[] { "PlaylistId", "UserId", "Type" });

            migrationBuilder.CreateIndex(
                name: "IX_ListeningHistory_UserId",
                table: "ListeningHistory",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ListeningHistory_AspNetUsers_UserId",
                table: "ListeningHistory",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlaylistPermission_AspNetUsers_UserId",
                table: "PlaylistPermission",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlaylistPermission_Playlist_PlaylistId",
                table: "PlaylistPermission",
                column: "PlaylistId",
                principalTable: "Playlist",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ListeningHistory_AspNetUsers_UserId",
                table: "ListeningHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_PlaylistPermission_AspNetUsers_UserId",
                table: "PlaylistPermission");

            migrationBuilder.DropForeignKey(
                name: "FK_PlaylistPermission_Playlist_PlaylistId",
                table: "PlaylistPermission");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ListeningHistory",
                table: "ListeningHistory");

            migrationBuilder.DropIndex(
                name: "IX_ListeningHistory_UserId",
                table: "ListeningHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlaylistPermission",
                table: "PlaylistPermission");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ListeningHistory");

            migrationBuilder.RenameTable(
                name: "PlaylistPermission",
                newName: "Permission");

            migrationBuilder.RenameIndex(
                name: "IX_PlaylistPermission_UserId",
                table: "Permission",
                newName: "IX_Permission_UserId");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ListeningHistory",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ListeningHistory",
                table: "ListeningHistory",
                columns: new[] { "UserId", "TrackId", "Date" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Permission",
                table: "Permission",
                columns: new[] { "PlaylistId", "UserId", "Type" });

            migrationBuilder.AddForeignKey(
                name: "FK_ListeningHistory_AspNetUsers_UserId",
                table: "ListeningHistory",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Permission_AspNetUsers_UserId",
                table: "Permission",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Permission_Playlist_PlaylistId",
                table: "Permission",
                column: "PlaylistId",
                principalTable: "Playlist",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
