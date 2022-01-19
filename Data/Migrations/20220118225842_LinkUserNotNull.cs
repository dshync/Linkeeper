using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Linkeeper.Migrations
{
    public partial class LinkUserNotNull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Links_AspNetUsers_userId",
                table: "Links");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "Links",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Links_userId",
                table: "Links",
                newName: "IX_Links_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Links_AspNetUsers_UserId",
                table: "Links",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
            /*
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { 
                    "Id", 
                    "UserName",	
                    "NormalizedUserName",
	                "Email",
	                "NormalizedEmail",
	                "EmailConfirmed",
	                "PasswordHash",
	                "SecurityStamp",
	                "ConcurrencyStamp",
	                "PhoneNumber",
	                "PhoneNumberConfirmed",
	                "TwoFactorEnabled",
	                "LockoutEnd",
	                "LockoutEnabled",
	                "AccessFailedCount"
                },
                values: new object[,]
                {
                    { 
                        "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa",  
                        "admin@admin.admin",
                        "ADMIN@ADMIN.ADMIN", 
                        "admin@admin.admin",
                        "ADMIN@ADMIN.ADMIN",
                        "1",
                        "AQAAAAEAACcQAAAAED/3j+rx+Z9z/3W9l/DWygNMofz/n2iY24Z9EDYH8WLmjto/5ZSSm8UtrMfeW3pb1A==",
                        "Q2QCBPZG52AHHPREIJPWEHOKLARVRUHX",
                        "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa",
                        "000000000000",
                        "1",
                        "0",
                        "2222-01-01 02:00:00.000000",
                        "1",
                        "0" 
                    }
                });
            */

            migrationBuilder.UpdateData(
                table: "Links",
                keyColumn: "UserId",
                keyValue: null,
                column: "UserId",
                value: "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"
                );

            migrationBuilder.AlterColumn<IdentityUser>(
                name: "UserId",
                table: "Links",
                nullable: false,
                defaultValue: "0"
                );

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<IdentityUser>(
                name: "UserId",
                table: "Links",
                nullable: true
                );

            migrationBuilder.DropForeignKey(
                name: "FK_Links_AspNetUsers_UserId",
                table: "Links");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Links",
                newName: "userId");

            migrationBuilder.RenameIndex(
                name: "IX_Links_UserId",
                table: "Links",
                newName: "IX_Links_userId");

            migrationBuilder.AddForeignKey(
                name: "FK_Links_AspNetUsers_userId",
                table: "Links",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
