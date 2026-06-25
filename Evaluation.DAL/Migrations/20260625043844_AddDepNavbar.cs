using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evaluation.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddDepNavbar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DepartmentId",
                table: "Navbar",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Navbar_DepartmentId",
                table: "Navbar",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Navbar_Departments_DepartmentId",
                table: "Navbar",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Navbar_Departments_DepartmentId",
                table: "Navbar");

            migrationBuilder.DropIndex(
                name: "IX_Navbar_DepartmentId",
                table: "Navbar");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Navbar");
        }
    }
}
