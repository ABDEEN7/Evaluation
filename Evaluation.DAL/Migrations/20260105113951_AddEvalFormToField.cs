using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evaluation.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddEvalFormToField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EvalFormId",
                table: "Field",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Field_EvalFormId",
                table: "Field",
                column: "EvalFormId");

            migrationBuilder.AddForeignKey(
                name: "FK_Field_EvalForms_EvalFormId",
                table: "Field",
                column: "EvalFormId",
                principalTable: "EvalForms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Field_EvalForms_EvalFormId",
                table: "Field");

            migrationBuilder.DropIndex(
                name: "IX_Field_EvalFormId",
                table: "Field");

            migrationBuilder.DropColumn(
                name: "EvalFormId",
                table: "Field");
        }
    }
}
