using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evaluation.DAL.Migrations
{
    /// <inheritdoc />
    public partial class EnhanceServiceRequestGradeAndSection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SchoolClassId",
                table: "ServiceRequests",
                newName: "TeacherId");

            migrationBuilder.AddColumn<Guid>(
                name: "GradeLevelId",
                table: "ServiceRequests",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolGradeSectionId",
                table: "ServiceRequests",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PlanAttachmentId",
                table: "Plans",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "EvaluationRequest",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ScopeReportNote",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EvaluationRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScopeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PositivePoint = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NegativePoint = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    table.PrimaryKey("PK_ScopeReportNote", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScopeReportNote_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScopeReportNote_EvaluationRequest_EvaluationRequestId",
                        column: x => x.EvaluationRequestId,
                        principalTable: "EvaluationRequest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScopeReportNote_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScopeReportNote_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ScopeReportNote_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ScopeReportNote_Scopes_ScopeId",
                        column: x => x.ScopeId,
                        principalTable: "Scopes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequests_GradeLevelId",
                table: "ServiceRequests",
                column: "GradeLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequests_SchoolGradeSectionId",
                table: "ServiceRequests",
                column: "SchoolGradeSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequests_TeacherId",
                table: "ServiceRequests",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Plans_PlanAttachmentId",
                table: "Plans",
                column: "PlanAttachmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ScopeReportNote_CreateById",
                table: "ScopeReportNote",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_ScopeReportNote_DeleteById",
                table: "ScopeReportNote",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_ScopeReportNote_DepartmentId",
                table: "ScopeReportNote",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ScopeReportNote_EvaluationRequestId",
                table: "ScopeReportNote",
                column: "EvaluationRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_ScopeReportNote_ScopeId",
                table: "ScopeReportNote",
                column: "ScopeId");

            migrationBuilder.CreateIndex(
                name: "IX_ScopeReportNote_UpdateById",
                table: "ScopeReportNote",
                column: "UpdateById");

            migrationBuilder.AddForeignKey(
                name: "FK_Plans_EvalAttachments_PlanAttachmentId",
                table: "Plans",
                column: "PlanAttachmentId",
                principalTable: "EvalAttachments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceRequests_Employees_TeacherId",
                table: "ServiceRequests",
                column: "TeacherId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceRequests_GradeLevel_GradeLevelId",
                table: "ServiceRequests",
                column: "GradeLevelId",
                principalTable: "GradeLevel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceRequests_SchoolGradeSection_SchoolGradeSectionId",
                table: "ServiceRequests",
                column: "SchoolGradeSectionId",
                principalTable: "SchoolGradeSection",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Plans_EvalAttachments_PlanAttachmentId",
                table: "Plans");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRequests_Employees_TeacherId",
                table: "ServiceRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRequests_GradeLevel_GradeLevelId",
                table: "ServiceRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRequests_SchoolGradeSection_SchoolGradeSectionId",
                table: "ServiceRequests");

            migrationBuilder.DropTable(
                name: "ScopeReportNote");

            migrationBuilder.DropIndex(
                name: "IX_ServiceRequests_GradeLevelId",
                table: "ServiceRequests");

            migrationBuilder.DropIndex(
                name: "IX_ServiceRequests_SchoolGradeSectionId",
                table: "ServiceRequests");

            migrationBuilder.DropIndex(
                name: "IX_ServiceRequests_TeacherId",
                table: "ServiceRequests");

            migrationBuilder.DropIndex(
                name: "IX_Plans_PlanAttachmentId",
                table: "Plans");

            migrationBuilder.DropColumn(
                name: "GradeLevelId",
                table: "ServiceRequests");

            migrationBuilder.DropColumn(
                name: "SchoolGradeSectionId",
                table: "ServiceRequests");

            migrationBuilder.DropColumn(
                name: "PlanAttachmentId",
                table: "Plans");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "EvaluationRequest");

            migrationBuilder.RenameColumn(
                name: "TeacherId",
                table: "ServiceRequests",
                newName: "SchoolClassId");
        }
    }
}
