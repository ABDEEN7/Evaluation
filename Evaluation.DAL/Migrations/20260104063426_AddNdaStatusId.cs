using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evaluation.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddNdaStatusId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "NdaDate",
                table: "EvaluationRequestAssignment",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "NdaStatusId",
                table: "EvaluationRequestAssignment",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EvaluationRequestAssignment_NdaStatusId",
                table: "EvaluationRequestAssignment",
                column: "NdaStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_EvaluationRequestAssignment_NdaStatus_NdaStatusId",
                table: "EvaluationRequestAssignment",
                column: "NdaStatusId",
                principalTable: "NdaStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EvaluationRequestAssignment_NdaStatus_NdaStatusId",
                table: "EvaluationRequestAssignment");

            migrationBuilder.DropIndex(
                name: "IX_EvaluationRequestAssignment_NdaStatusId",
                table: "EvaluationRequestAssignment");

            migrationBuilder.DropColumn(
                name: "NdaDate",
                table: "EvaluationRequestAssignment");

            migrationBuilder.DropColumn(
                name: "NdaStatusId",
                table: "EvaluationRequestAssignment");
        }
    }
}
