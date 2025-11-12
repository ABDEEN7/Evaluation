using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evaluation.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddUserGenderToEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserGenderId",
                table: "OrgTree",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrgTree_UserGenderId",
                table: "OrgTree",
                column: "UserGenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrgTree_UserGender_UserGenderId",
                table: "OrgTree",
                column: "UserGenderId",
                principalTable: "UserGender",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrgTree_UserGender_UserGenderId",
                table: "OrgTree");

            migrationBuilder.DropIndex(
                name: "IX_OrgTree_UserGenderId",
                table: "OrgTree");

            migrationBuilder.DropColumn(
                name: "UserGenderId",
                table: "OrgTree");
        }
    }
}
