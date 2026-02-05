using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evaluation.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddSupportFiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSupportFiles",
                table: "EvaluationParties",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "ScopeId",
                table: "EvalAttachments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EvalAttachments_ScopeId",
                table: "EvalAttachments",
                column: "ScopeId");

            migrationBuilder.AddForeignKey(
                name: "FK_EvalAttachments_Scopes_ScopeId",
                table: "EvalAttachments",
                column: "ScopeId",
                principalTable: "Scopes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EvalAttachments_Scopes_ScopeId",
                table: "EvalAttachments");

            migrationBuilder.DropIndex(
                name: "IX_EvalAttachments_ScopeId",
                table: "EvalAttachments");

            migrationBuilder.DropColumn(
                name: "IsSupportFiles",
                table: "EvaluationParties");

            migrationBuilder.DropColumn(
                name: "ScopeId",
                table: "EvalAttachments");
        }
    }
}
