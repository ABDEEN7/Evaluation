using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evaluation.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddFormItemValueHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScopeReportNote_Departments_DepartmentId",
                table: "ScopeReportNote");

            migrationBuilder.DropForeignKey(
                name: "FK_ScopeReportNote_EvaluationRequest_EvaluationRequestId",
                table: "ScopeReportNote");

            migrationBuilder.DropForeignKey(
                name: "FK_ScopeReportNote_MinistryUsers_CreateById",
                table: "ScopeReportNote");

            migrationBuilder.DropForeignKey(
                name: "FK_ScopeReportNote_MinistryUsers_DeleteById",
                table: "ScopeReportNote");

            migrationBuilder.DropForeignKey(
                name: "FK_ScopeReportNote_MinistryUsers_UpdateById",
                table: "ScopeReportNote");

            migrationBuilder.DropForeignKey(
                name: "FK_ScopeReportNote_Scopes_ScopeId",
                table: "ScopeReportNote");

            migrationBuilder.CreateTable(
                name: "FormItemValueHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubmissionCodeHistory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FormItemValueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FormItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DepEvalMatrixId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FormEvalMatrixValueId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DepEvalMatrixValueName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActualValue = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CalcMethodId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RenameItem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FormItemConfigId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ServiceRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EvaluationRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ItemWeight = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ItemConfigWeight = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
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
                    table.PrimaryKey("PK_FormItemValueHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormItemValueHistory_CalcMethods_CalcMethodId",
                        column: x => x.CalcMethodId,
                        principalTable: "CalcMethods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FormItemValueHistory_DepEvalMatrixs_DepEvalMatrixId",
                        column: x => x.DepEvalMatrixId,
                        principalTable: "DepEvalMatrixs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FormItemValueHistory_EvaluationRequest_EvaluationRequestId",
                        column: x => x.EvaluationRequestId,
                        principalTable: "EvaluationRequest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FormItemValueHistory_FormEvalMatrixValue_FormEvalMatrixValueId",
                        column: x => x.FormEvalMatrixValueId,
                        principalTable: "FormEvalMatrixValue",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FormItemValueHistory_FormItemConfig_FormItemConfigId",
                        column: x => x.FormItemConfigId,
                        principalTable: "FormItemConfig",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FormItemValueHistory_FormItemValues_FormItemValueId",
                        column: x => x.FormItemValueId,
                        principalTable: "FormItemValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FormItemValueHistory_FormItem_FormItemId",
                        column: x => x.FormItemId,
                        principalTable: "FormItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FormItemValueHistory_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FormItemValueHistory_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FormItemValueHistory_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FormItemValueHistory_MinistryUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FormItemValueHistory_ServiceRequests_ServiceRequestId",
                        column: x => x.ServiceRequestId,
                        principalTable: "ServiceRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FormItemValueHistory_CalcMethodId",
                table: "FormItemValueHistory",
                column: "CalcMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_FormItemValueHistory_CreateById",
                table: "FormItemValueHistory",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_FormItemValueHistory_DeleteById",
                table: "FormItemValueHistory",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_FormItemValueHistory_DepEvalMatrixId",
                table: "FormItemValueHistory",
                column: "DepEvalMatrixId");

            migrationBuilder.CreateIndex(
                name: "IX_FormItemValueHistory_EvaluationRequestId",
                table: "FormItemValueHistory",
                column: "EvaluationRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_FormItemValueHistory_FormEvalMatrixValueId",
                table: "FormItemValueHistory",
                column: "FormEvalMatrixValueId");

            migrationBuilder.CreateIndex(
                name: "IX_FormItemValueHistory_FormItemConfigId",
                table: "FormItemValueHistory",
                column: "FormItemConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_FormItemValueHistory_FormItemId",
                table: "FormItemValueHistory",
                column: "FormItemId");

            migrationBuilder.CreateIndex(
                name: "IX_FormItemValueHistory_FormItemValueId",
                table: "FormItemValueHistory",
                column: "FormItemValueId");

            migrationBuilder.CreateIndex(
                name: "IX_FormItemValueHistory_ServiceRequestId",
                table: "FormItemValueHistory",
                column: "ServiceRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_FormItemValueHistory_UpdateById",
                table: "FormItemValueHistory",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_FormItemValueHistory_UserId",
                table: "FormItemValueHistory",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScopeReportNote_Departments_DepartmentId",
                table: "ScopeReportNote",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ScopeReportNote_EvaluationRequest_EvaluationRequestId",
                table: "ScopeReportNote",
                column: "EvaluationRequestId",
                principalTable: "EvaluationRequest",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ScopeReportNote_MinistryUsers_CreateById",
                table: "ScopeReportNote",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ScopeReportNote_MinistryUsers_DeleteById",
                table: "ScopeReportNote",
                column: "DeleteById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ScopeReportNote_MinistryUsers_UpdateById",
                table: "ScopeReportNote",
                column: "UpdateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ScopeReportNote_Scopes_ScopeId",
                table: "ScopeReportNote",
                column: "ScopeId",
                principalTable: "Scopes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScopeReportNote_Departments_DepartmentId",
                table: "ScopeReportNote");

            migrationBuilder.DropForeignKey(
                name: "FK_ScopeReportNote_EvaluationRequest_EvaluationRequestId",
                table: "ScopeReportNote");

            migrationBuilder.DropForeignKey(
                name: "FK_ScopeReportNote_MinistryUsers_CreateById",
                table: "ScopeReportNote");

            migrationBuilder.DropForeignKey(
                name: "FK_ScopeReportNote_MinistryUsers_DeleteById",
                table: "ScopeReportNote");

            migrationBuilder.DropForeignKey(
                name: "FK_ScopeReportNote_MinistryUsers_UpdateById",
                table: "ScopeReportNote");

            migrationBuilder.DropForeignKey(
                name: "FK_ScopeReportNote_Scopes_ScopeId",
                table: "ScopeReportNote");

            migrationBuilder.DropTable(
                name: "FormItemValueHistory");

            migrationBuilder.AddForeignKey(
                name: "FK_ScopeReportNote_Departments_DepartmentId",
                table: "ScopeReportNote",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ScopeReportNote_EvaluationRequest_EvaluationRequestId",
                table: "ScopeReportNote",
                column: "EvaluationRequestId",
                principalTable: "EvaluationRequest",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ScopeReportNote_MinistryUsers_CreateById",
                table: "ScopeReportNote",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ScopeReportNote_MinistryUsers_DeleteById",
                table: "ScopeReportNote",
                column: "DeleteById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ScopeReportNote_MinistryUsers_UpdateById",
                table: "ScopeReportNote",
                column: "UpdateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ScopeReportNote_Scopes_ScopeId",
                table: "ScopeReportNote",
                column: "ScopeId",
                principalTable: "Scopes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
