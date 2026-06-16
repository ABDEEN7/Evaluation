using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evaluation.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddAnalysisTypeToEvalItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AnalysisTypeId",
                table: "FormItem",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FormItem_AnalysisTypeId",
                table: "FormItem",
                column: "AnalysisTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_FormItem_AnalysisType_AnalysisTypeId",
                table: "FormItem",
                column: "AnalysisTypeId",
                principalTable: "AnalysisType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormItem_AnalysisType_AnalysisTypeId",
                table: "FormItem");

            migrationBuilder.DropIndex(
                name: "IX_FormItem_AnalysisTypeId",
                table: "FormItem");

            migrationBuilder.DropColumn(
                name: "AnalysisTypeId",
                table: "FormItem");
        }
    }
}
