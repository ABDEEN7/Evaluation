using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evaluation.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddScopeParentToScopeAcademicYear : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ScopeParentId",
                table: "ScopeAcademicYears",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ScopeAcademicYears_ScopeParentId",
                table: "ScopeAcademicYears",
                column: "ScopeParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScopeAcademicYears_Scopes_ScopeParentId",
                table: "ScopeAcademicYears",
                column: "ScopeParentId",
                principalTable: "Scopes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScopeAcademicYears_Scopes_ScopeParentId",
                table: "ScopeAcademicYears");

            migrationBuilder.DropIndex(
                name: "IX_ScopeAcademicYears_ScopeParentId",
                table: "ScopeAcademicYears");

            migrationBuilder.DropColumn(
                name: "ScopeParentId",
                table: "ScopeAcademicYears");
        }
    }
}
