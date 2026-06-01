using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evaluation.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddFinalEvalMatrixIdToEvalForm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FinalEvalMatrixId",
                table: "EvalForms",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EvaluationTypeId",
                table: "DepEvaluationType",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EvalForms_FinalEvalMatrixId",
                table: "EvalForms",
                column: "FinalEvalMatrixId");

            migrationBuilder.CreateIndex(
                name: "IX_DepEvaluationType_EvaluationTypeId",
                table: "DepEvaluationType",
                column: "EvaluationTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_DepEvaluationType_EvaluationType_EvaluationTypeId",
                table: "DepEvaluationType",
                column: "EvaluationTypeId",
                principalTable: "EvaluationType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EvalForms_FormEvalMatrix_FinalEvalMatrixId",
                table: "EvalForms",
                column: "FinalEvalMatrixId",
                principalTable: "FormEvalMatrix",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DepEvaluationType_EvaluationType_EvaluationTypeId",
                table: "DepEvaluationType");

            migrationBuilder.DropForeignKey(
                name: "FK_EvalForms_FormEvalMatrix_FinalEvalMatrixId",
                table: "EvalForms");

            migrationBuilder.DropIndex(
                name: "IX_EvalForms_FinalEvalMatrixId",
                table: "EvalForms");

            migrationBuilder.DropIndex(
                name: "IX_DepEvaluationType_EvaluationTypeId",
                table: "DepEvaluationType");

            migrationBuilder.DropColumn(
                name: "FinalEvalMatrixId",
                table: "EvalForms");

            migrationBuilder.DropColumn(
                name: "EvaluationTypeId",
                table: "DepEvaluationType");
        }
    }
}
