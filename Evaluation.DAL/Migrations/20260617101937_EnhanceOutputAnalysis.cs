using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evaluation.DAL.Migrations
{
    /// <inheritdoc />
    public partial class EnhanceOutputAnalysis : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LastYearStudentCount",
                table: "OutputAnalysisData",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PreviousYearStudentCount",
                table: "OutputAnalysisData",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TermCode",
                table: "OutputAnalysisData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReportDescAr",
                table: "FormEvalMatrixValue",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReportDescEn",
                table: "FormEvalMatrixValue",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReportTextAr",
                table: "FormEvalMatrixValue",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReportTextEn",
                table: "FormEvalMatrixValue",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastYearStudentCount",
                table: "OutputAnalysisData");

            migrationBuilder.DropColumn(
                name: "PreviousYearStudentCount",
                table: "OutputAnalysisData");

            migrationBuilder.DropColumn(
                name: "TermCode",
                table: "OutputAnalysisData");

            migrationBuilder.DropColumn(
                name: "ReportDescAr",
                table: "FormEvalMatrixValue");

            migrationBuilder.DropColumn(
                name: "ReportDescEn",
                table: "FormEvalMatrixValue");

            migrationBuilder.DropColumn(
                name: "ReportTextAr",
                table: "FormEvalMatrixValue");

            migrationBuilder.DropColumn(
                name: "ReportTextEn",
                table: "FormEvalMatrixValue");
        }
    }
}
