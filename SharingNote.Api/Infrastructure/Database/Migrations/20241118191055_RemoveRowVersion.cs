using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SharingNote.Api.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRowVersion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostInteraction_Posts_PostId",
                table: "PostInteraction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostInteraction",
                table: "PostInteraction");

            migrationBuilder.RenameTable(
                name: "PostInteraction",
                newName: "PostInteractions");

            migrationBuilder.RenameIndex(
                name: "IX_PostInteraction_PostId",
                table: "PostInteractions",
                newName: "IX_PostInteractions_PostId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostInteractions",
                table: "PostInteractions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PostInteractions_Posts_PostId",
                table: "PostInteractions",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostInteractions_Posts_PostId",
                table: "PostInteractions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostInteractions",
                table: "PostInteractions");

            migrationBuilder.RenameTable(
                name: "PostInteractions",
                newName: "PostInteraction");

            migrationBuilder.RenameIndex(
                name: "IX_PostInteractions_PostId",
                table: "PostInteraction",
                newName: "IX_PostInteraction_PostId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostInteraction",
                table: "PostInteraction",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PostInteraction_Posts_PostId",
                table: "PostInteraction",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
