using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evaluation.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddNoteToSubEvalItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Scopes_Departments_DepartmentId",
                table: "Scopes");

            migrationBuilder.DropIndex(
                name: "IX_Scopes_DepartmentId",
                table: "Scopes");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Scopes");

            migrationBuilder.AddColumn<bool>(
                name: "HasNote",
                table: "SubFormItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "NoteRequired",
                table: "SubFormItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "OrderNo",
                table: "SubFormItems",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasNote",
                table: "SubFormItems");

            migrationBuilder.DropColumn(
                name: "NoteRequired",
                table: "SubFormItems");

            migrationBuilder.DropColumn(
                name: "OrderNo",
                table: "SubFormItems");

            migrationBuilder.AddColumn<Guid>(
                name: "DepartmentId",
                table: "Scopes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Scopes_DepartmentId",
                table: "Scopes",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Scopes_Departments_DepartmentId",
                table: "Scopes",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
