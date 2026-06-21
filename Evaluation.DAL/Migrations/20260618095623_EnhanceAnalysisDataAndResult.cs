using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evaluation.DAL.Migrations
{
    /// <inheritdoc />
    public partial class EnhanceAnalysisDataAndResult : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnalysisType_FormEvalMatrix_FormEvalMatrixId",
                table: "AnalysisType");

            migrationBuilder.RenameColumn(
                name: "FormEvalMatrixId",
                table: "AnalysisType",
                newName: "ResultFormEvalMatrixId");

            migrationBuilder.RenameIndex(
                name: "IX_AnalysisType_FormEvalMatrixId",
                table: "AnalysisType",
                newName: "IX_AnalysisType_ResultFormEvalMatrixId");

            migrationBuilder.AddColumn<string>(
                name: "DataConfig",
                table: "OutputAnalysisData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MartixTextValue",
                table: "OutputAnalysisData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataFormEvalMatrixId",
                table: "AnalysisType",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnalysisType_DataFormEvalMatrixId",
                table: "AnalysisType",
                column: "DataFormEvalMatrixId");

            migrationBuilder.AddForeignKey(
                name: "FK_AnalysisType_FormEvalMatrix_DataFormEvalMatrixId",
                table: "AnalysisType",
                column: "DataFormEvalMatrixId",
                principalTable: "FormEvalMatrix",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AnalysisType_FormEvalMatrix_ResultFormEvalMatrixId",
                table: "AnalysisType",
                column: "ResultFormEvalMatrixId",
                principalTable: "FormEvalMatrix",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnalysisType_FormEvalMatrix_DataFormEvalMatrixId",
                table: "AnalysisType");

            migrationBuilder.DropForeignKey(
                name: "FK_AnalysisType_FormEvalMatrix_ResultFormEvalMatrixId",
                table: "AnalysisType");

            migrationBuilder.DropIndex(
                name: "IX_AnalysisType_DataFormEvalMatrixId",
                table: "AnalysisType");

            migrationBuilder.DropColumn(
                name: "DataConfig",
                table: "OutputAnalysisData");

            migrationBuilder.DropColumn(
                name: "MartixTextValue",
                table: "OutputAnalysisData");

            migrationBuilder.DropColumn(
                name: "DataFormEvalMatrixId",
                table: "AnalysisType");

            migrationBuilder.RenameColumn(
                name: "ResultFormEvalMatrixId",
                table: "AnalysisType",
                newName: "FormEvalMatrixId");

            migrationBuilder.RenameIndex(
                name: "IX_AnalysisType_ResultFormEvalMatrixId",
                table: "AnalysisType",
                newName: "IX_AnalysisType_FormEvalMatrixId");

            migrationBuilder.AddForeignKey(
                name: "FK_AnalysisType_FormEvalMatrix_FormEvalMatrixId",
                table: "AnalysisType",
                column: "FormEvalMatrixId",
                principalTable: "FormEvalMatrix",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
