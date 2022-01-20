using Microsoft.EntityFrameworkCore.Migrations;

namespace Linkeeper.Migrations
{
    public partial class AddUserIdToLinkModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "userId",
                table: "Links",
                type: "varchar(95)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Links_userId",
                table: "Links",
                column: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_Links_AspNetUsers_userId",
                table: "Links",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Links_AspNetUsers_userId",
                table: "Links");

            migrationBuilder.DropIndex(
                name: "IX_Links_userId",
                table: "Links");

            migrationBuilder.DropColumn(
                name: "userId",
                table: "Links");
        }
    }
}
