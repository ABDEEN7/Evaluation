using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evaluation.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddOutputAnalysisModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnalysisType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BackendName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Grades = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubjectCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnalysisConfig = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FormEvalMatrixId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OrderNo = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalysisType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnalysisType_FormEvalMatrix_FormEvalMatrixId",
                        column: x => x.FormEvalMatrixId,
                        principalTable: "FormEvalMatrix",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AnalysisType_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AnalysisType_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AnalysisType_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OutputAnalysisFinalResult",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnalysisTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EvaluationRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActualValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FormEvalMatrixValueId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutputAnalysisFinalResult", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OutputAnalysisFinalResult_AnalysisType_AnalysisTypeId",
                        column: x => x.AnalysisTypeId,
                        principalTable: "AnalysisType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OutputAnalysisFinalResult_EvaluationRequest_EvaluationRequestId",
                        column: x => x.EvaluationRequestId,
                        principalTable: "EvaluationRequest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OutputAnalysisFinalResult_FormEvalMatrixValue_FormEvalMatrixValueId",
                        column: x => x.FormEvalMatrixValueId,
                        principalTable: "FormEvalMatrixValue",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OutputAnalysisFinalResult_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OutputAnalysisFinalResult_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OutputAnalysisFinalResult_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OutputAnalysisData",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OutputAnalysisFinalResultId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnalysisTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EvaluationRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Grade = table.Column<int>(type: "int", nullable: false),
                    LastYear = table.Column<int>(type: "int", nullable: false),
                    PreviousYear = table.Column<int>(type: "int", nullable: false),
                    LastYearValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PreviousYearValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Difference = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SubjectCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Track = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FormEvalMatrixValueId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ActualValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutputAnalysisData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OutputAnalysisData_AnalysisType_AnalysisTypeId",
                        column: x => x.AnalysisTypeId,
                        principalTable: "AnalysisType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OutputAnalysisData_EvaluationRequest_EvaluationRequestId",
                        column: x => x.EvaluationRequestId,
                        principalTable: "EvaluationRequest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OutputAnalysisData_FormEvalMatrixValue_FormEvalMatrixValueId",
                        column: x => x.FormEvalMatrixValueId,
                        principalTable: "FormEvalMatrixValue",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OutputAnalysisData_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OutputAnalysisData_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OutputAnalysisData_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OutputAnalysisData_OutputAnalysisFinalResult_OutputAnalysisFinalResultId",
                        column: x => x.OutputAnalysisFinalResultId,
                        principalTable: "OutputAnalysisFinalResult",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnalysisType_CreateById",
                table: "AnalysisType",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_AnalysisType_DeleteById",
                table: "AnalysisType",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_AnalysisType_FormEvalMatrixId",
                table: "AnalysisType",
                column: "FormEvalMatrixId");

            migrationBuilder.CreateIndex(
                name: "IX_AnalysisType_UpdateById",
                table: "AnalysisType",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_OutputAnalysisData_AnalysisTypeId",
                table: "OutputAnalysisData",
                column: "AnalysisTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_OutputAnalysisData_CreateById",
                table: "OutputAnalysisData",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_OutputAnalysisData_DeleteById",
                table: "OutputAnalysisData",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_OutputAnalysisData_EvaluationRequestId",
                table: "OutputAnalysisData",
                column: "EvaluationRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_OutputAnalysisData_FormEvalMatrixValueId",
                table: "OutputAnalysisData",
                column: "FormEvalMatrixValueId");

            migrationBuilder.CreateIndex(
                name: "IX_OutputAnalysisData_OutputAnalysisFinalResultId",
                table: "OutputAnalysisData",
                column: "OutputAnalysisFinalResultId");

            migrationBuilder.CreateIndex(
                name: "IX_OutputAnalysisData_UpdateById",
                table: "OutputAnalysisData",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_OutputAnalysisFinalResult_AnalysisTypeId",
                table: "OutputAnalysisFinalResult",
                column: "AnalysisTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_OutputAnalysisFinalResult_CreateById",
                table: "OutputAnalysisFinalResult",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_OutputAnalysisFinalResult_DeleteById",
                table: "OutputAnalysisFinalResult",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_OutputAnalysisFinalResult_EvaluationRequestId",
                table: "OutputAnalysisFinalResult",
                column: "EvaluationRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_OutputAnalysisFinalResult_FormEvalMatrixValueId",
                table: "OutputAnalysisFinalResult",
                column: "FormEvalMatrixValueId");

            migrationBuilder.CreateIndex(
                name: "IX_OutputAnalysisFinalResult_UpdateById",
                table: "OutputAnalysisFinalResult",
                column: "UpdateById");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OutputAnalysisData");

            migrationBuilder.DropTable(
                name: "OutputAnalysisFinalResult");

            migrationBuilder.DropTable(
                name: "AnalysisType");
        }
    }
}
