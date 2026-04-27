using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evaluation.DAL.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDepFromCalMethod : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CalcMethods_Departments_DepartmentId",
                table: "CalcMethods");

            migrationBuilder.DropIndex(
                name: "IX_CalcMethods_BackendName",
                table: "CalcMethods");

            migrationBuilder.DropIndex(
                name: "IX_CalcMethods_DepartmentId",
                table: "CalcMethods");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "CalcMethods");

            migrationBuilder.AlterColumn<string>(
                name: "BackendName",
                table: "CalcMethods",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderNo",
                table: "CalcMethods",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CalcMethods_BackendName",
                table: "CalcMethods",
                column: "BackendName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CalcMethods_BackendName",
                table: "CalcMethods");

            migrationBuilder.DropColumn(
                name: "OrderNo",
                table: "CalcMethods");

            migrationBuilder.AlterColumn<string>(
                name: "BackendName",
                table: "CalcMethods",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<Guid>(
                name: "DepartmentId",
                table: "CalcMethods",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_CalcMethods_BackendName",
                table: "CalcMethods",
                column: "BackendName",
                unique: true,
                filter: "[BackendName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CalcMethods_DepartmentId",
                table: "CalcMethods",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_CalcMethods_Departments_DepartmentId",
                table: "CalcMethods",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
