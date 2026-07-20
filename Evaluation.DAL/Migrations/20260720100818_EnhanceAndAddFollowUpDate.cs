using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evaluation.DAL.Migrations
{
    /// <inheritdoc />
    public partial class EnhanceAndAddFollowUpDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormItemValueHistory_DepEvalMatrixs_DepEvalMatrixId",
                table: "FormItemValueHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_FormItemValues_DepEvalMatrixs_DepEvalMatrixId",
                table: "FormItemValues");

            migrationBuilder.DropTable(
                name: "OrgEvalResults");

            migrationBuilder.DropTable(
                name: "DepEvalMatrixs");

            migrationBuilder.RenameColumn(
                name: "DepEvalMatrixId",
                table: "FormItemValues",
                newName: "FormEvalMatrixId");

            migrationBuilder.RenameIndex(
                name: "IX_FormItemValues_DepEvalMatrixId",
                table: "FormItemValues",
                newName: "IX_FormItemValues_FormEvalMatrixId");

            migrationBuilder.RenameColumn(
                name: "DepEvalMatrixId",
                table: "FormItemValueHistory",
                newName: "FormEvalMatrixId");

            migrationBuilder.RenameIndex(
                name: "IX_FormItemValueHistory_DepEvalMatrixId",
                table: "FormItemValueHistory",
                newName: "IX_FormItemValueHistory_FormEvalMatrixId");

            migrationBuilder.AddColumn<int>(
                name: "NextFollowUpDays",
                table: "FormEvalMatrixValue",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "RequiredFollowUp",
                table: "FormEvalMatrix",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "FollowUpDays",
                table: "EvaluationRequest",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "NextFollowUpDate",
                table: "EvaluationRequest",
                type: "date",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FormItemValueHistory_FormEvalMatrix_FormEvalMatrixId",
                table: "FormItemValueHistory",
                column: "FormEvalMatrixId",
                principalTable: "FormEvalMatrix",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FormItemValues_FormEvalMatrix_FormEvalMatrixId",
                table: "FormItemValues",
                column: "FormEvalMatrixId",
                principalTable: "FormEvalMatrix",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormItemValueHistory_FormEvalMatrix_FormEvalMatrixId",
                table: "FormItemValueHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_FormItemValues_FormEvalMatrix_FormEvalMatrixId",
                table: "FormItemValues");

            migrationBuilder.DropColumn(
                name: "NextFollowUpDays",
                table: "FormEvalMatrixValue");

            migrationBuilder.DropColumn(
                name: "RequiredFollowUp",
                table: "FormEvalMatrix");

            migrationBuilder.DropColumn(
                name: "FollowUpDays",
                table: "EvaluationRequest");

            migrationBuilder.DropColumn(
                name: "NextFollowUpDate",
                table: "EvaluationRequest");

            migrationBuilder.RenameColumn(
                name: "FormEvalMatrixId",
                table: "FormItemValues",
                newName: "DepEvalMatrixId");

            migrationBuilder.RenameIndex(
                name: "IX_FormItemValues_FormEvalMatrixId",
                table: "FormItemValues",
                newName: "IX_FormItemValues_DepEvalMatrixId");

            migrationBuilder.RenameColumn(
                name: "FormEvalMatrixId",
                table: "FormItemValueHistory",
                newName: "DepEvalMatrixId");

            migrationBuilder.RenameIndex(
                name: "IX_FormItemValueHistory_FormEvalMatrixId",
                table: "FormItemValueHistory",
                newName: "IX_FormItemValueHistory_DepEvalMatrixId");

            migrationBuilder.CreateTable(
                name: "DepEvalMatrixs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AcademicYearId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BackendName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ItemValue = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MaxValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MinValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NextEvaluationDays = table.Column<int>(type: "int", nullable: false),
                    NextFollowUpDays = table.Column<int>(type: "int", nullable: false),
                    RequiredFollowUp = table.Column<bool>(type: "bit", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepEvalMatrixs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DepEvalMatrixs_AcademicYears_AcademicYearId",
                        column: x => x.AcademicYearId,
                        principalTable: "AcademicYears",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DepEvalMatrixs_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DepEvalMatrixs_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DepEvalMatrixs_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrgEvalResults",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AcademicYearId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DepEvalMatrixId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrgTreeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FinalEvalValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrgEvalResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrgEvalResults_AcademicYears_AcademicYearId",
                        column: x => x.AcademicYearId,
                        principalTable: "AcademicYears",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrgEvalResults_DepEvalMatrixs_DepEvalMatrixId",
                        column: x => x.DepEvalMatrixId,
                        principalTable: "DepEvalMatrixs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrgEvalResults_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrgEvalResults_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrgEvalResults_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrgEvalResults_OrgTree_OrgTreeId",
                        column: x => x.OrgTreeId,
                        principalTable: "OrgTree",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DepEvalMatrixs_AcademicYearId",
                table: "DepEvalMatrixs",
                column: "AcademicYearId");

            migrationBuilder.CreateIndex(
                name: "IX_DepEvalMatrixs_CreateById",
                table: "DepEvalMatrixs",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_DepEvalMatrixs_DeleteById",
                table: "DepEvalMatrixs",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_DepEvalMatrixs_UpdateById",
                table: "DepEvalMatrixs",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_OrgEvalResults_AcademicYearId",
                table: "OrgEvalResults",
                column: "AcademicYearId");

            migrationBuilder.CreateIndex(
                name: "IX_OrgEvalResults_CreateById",
                table: "OrgEvalResults",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_OrgEvalResults_DeleteById",
                table: "OrgEvalResults",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_OrgEvalResults_DepEvalMatrixId",
                table: "OrgEvalResults",
                column: "DepEvalMatrixId");

            migrationBuilder.CreateIndex(
                name: "IX_OrgEvalResults_OrgTreeId",
                table: "OrgEvalResults",
                column: "OrgTreeId");

            migrationBuilder.CreateIndex(
                name: "IX_OrgEvalResults_UpdateById",
                table: "OrgEvalResults",
                column: "UpdateById");

            migrationBuilder.AddForeignKey(
                name: "FK_FormItemValueHistory_DepEvalMatrixs_DepEvalMatrixId",
                table: "FormItemValueHistory",
                column: "DepEvalMatrixId",
                principalTable: "DepEvalMatrixs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FormItemValues_DepEvalMatrixs_DepEvalMatrixId",
                table: "FormItemValues",
                column: "DepEvalMatrixId",
                principalTable: "DepEvalMatrixs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
