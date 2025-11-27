using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evaluation.DAL.Migrations
{
    /// <inheritdoc />
    public partial class deletedunneededacademicYearId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Plans_Departments_DepartmentId",
                table: "Plans");

            migrationBuilder.DropIndex(
                name: "IX_Plans_DepartmentId",
                table: "Plans");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Plans");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DepartmentId",
                table: "Plans",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Plans_DepartmentId",
                table: "Plans",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Plans_Departments_DepartmentId",
                table: "Plans",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
