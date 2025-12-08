using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evaluation.DAL.Migrations
{
    /// <inheritdoc />
    public partial class EnhancePlanModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActionTransactionsLog_MinistryUsers_CreateById",
                table: "ActionTransactionsLog");

            migrationBuilder.DropForeignKey(
                name: "FK_ActionTransactionsLog_MinistryUsers_DeleteById",
                table: "ActionTransactionsLog");

            migrationBuilder.DropForeignKey(
                name: "FK_ActionTransactionsLog_MinistryUsers_UpdateById",
                table: "ActionTransactionsLog");

            migrationBuilder.DropForeignKey(
                name: "FK_ActionTransactionsLog_ServiceAction_ServiceActionId",
                table: "ActionTransactionsLog");

            migrationBuilder.DropForeignKey(
                name: "FK_ActionTransactionsLog_ServiceRequests_ServiceRequestId",
                table: "ActionTransactionsLog");

            migrationBuilder.DropForeignKey(
                name: "FK_ActionTransactionsLog_SystemModules_SystemModuleId",
                table: "ActionTransactionsLog");

            migrationBuilder.DropForeignKey(
                name: "FK_Plans_Departments_DepartmentId",
                table: "Plans");

            migrationBuilder.DropForeignKey(
                name: "FK_Plans_PlanTypeDepartment_PlanTypeDepartmentId",
                table: "Plans");

            migrationBuilder.DropForeignKey(
                name: "FK_SchoolLevel_OrgTree_SchoolId",
                table: "SchoolLevel");

            migrationBuilder.DropForeignKey(
                name: "FK_UserDepartment_Departments_DepartmentId",
                table: "UserDepartment");

            migrationBuilder.DropForeignKey(
                name: "FK_UserDepartment_MinistryUsers_CreateById",
                table: "UserDepartment");

            migrationBuilder.DropForeignKey(
                name: "FK_UserDepartment_MinistryUsers_DeleteById",
                table: "UserDepartment");

            migrationBuilder.DropForeignKey(
                name: "FK_UserDepartment_MinistryUsers_UpdateById",
                table: "UserDepartment");

            migrationBuilder.DropForeignKey(
                name: "FK_UserDepartment_MinistryUsers_UserId",
                table: "UserDepartment");

            migrationBuilder.DropTable(
                name: "Attachment");

            migrationBuilder.DropTable(
                name: "PlanTypeDepartment");

            migrationBuilder.DropIndex(
                name: "IX_Plans_DepartmentId",
                table: "Plans");

            migrationBuilder.DropIndex(
                name: "IX_Plans_PlanTypeDepartmentId",
                table: "Plans");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Plans");

            migrationBuilder.DropColumn(
                name: "PlanTypeDepartmentId",
                table: "Plans");

            migrationBuilder.AddColumn<string>(
                name: "BackendName",
                table: "Semesters",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NSISCode",
                table: "Semesters",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderNo",
                table: "Semesters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "NSISCode",
                table: "SchoolLevel",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BackendName",
                table: "PlanStatuses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "OrderNo",
                table: "PlanStatuses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<Guid>(
                name: "PlanStatusId",
                table: "Plans",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "AcademicYearId",
                table: "Plans",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "PlanTypeDepId",
                table: "Plans",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EvalAttachments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActionTransactionsLogId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OrgTreeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FieldId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ServiceRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UiFileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileExtension = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    IsOthers = table.Column<bool>(type: "bit", nullable: false),
                    ChildFieldId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Index = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EvaluationRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvalAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EvalAttachments_ActionTransactionsLog_ActionTransactionsLogId",
                        column: x => x.ActionTransactionsLogId,
                        principalTable: "ActionTransactionsLog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EvalAttachments_EvaluationRequest_EvaluationRequestId",
                        column: x => x.EvaluationRequestId,
                        principalTable: "EvaluationRequest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EvalAttachments_Field_ChildFieldId",
                        column: x => x.ChildFieldId,
                        principalTable: "Field",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EvalAttachments_Field_FieldId",
                        column: x => x.FieldId,
                        principalTable: "Field",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EvalAttachments_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EvalAttachments_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EvalAttachments_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EvalAttachments_OrgTree_OrgTreeId",
                        column: x => x.OrgTreeId,
                        principalTable: "OrgTree",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlanTypeDep",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlanTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderNo = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanTypeDep", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanTypeDep_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlanTypeDep_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlanTypeDep_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlanTypeDep_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlanTypeDep_PlanType_PlanTypeId",
                        column: x => x.PlanTypeId,
                        principalTable: "PlanType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Plans_PlanTypeDepId",
                table: "Plans",
                column: "PlanTypeDepId");

            migrationBuilder.CreateIndex(
                name: "IX_EvalAttachments_ActionTransactionsLogId",
                table: "EvalAttachments",
                column: "ActionTransactionsLogId");

            migrationBuilder.CreateIndex(
                name: "IX_EvalAttachments_ChildFieldId",
                table: "EvalAttachments",
                column: "ChildFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_EvalAttachments_CreateById",
                table: "EvalAttachments",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_EvalAttachments_DeleteById",
                table: "EvalAttachments",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_EvalAttachments_EvaluationRequestId",
                table: "EvalAttachments",
                column: "EvaluationRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_EvalAttachments_FieldId",
                table: "EvalAttachments",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_EvalAttachments_OrgTreeId",
                table: "EvalAttachments",
                column: "OrgTreeId");

            migrationBuilder.CreateIndex(
                name: "IX_EvalAttachments_UpdateById",
                table: "EvalAttachments",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_PlanTypeDep_CreateById",
                table: "PlanTypeDep",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_PlanTypeDep_DeleteById",
                table: "PlanTypeDep",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_PlanTypeDep_DepartmentId",
                table: "PlanTypeDep",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanTypeDep_PlanTypeId",
                table: "PlanTypeDep",
                column: "PlanTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanTypeDep_UpdateById",
                table: "PlanTypeDep",
                column: "UpdateById");

            migrationBuilder.AddForeignKey(
                name: "FK_ActionTransactionsLog_MinistryUsers_CreateById",
                table: "ActionTransactionsLog",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ActionTransactionsLog_MinistryUsers_DeleteById",
                table: "ActionTransactionsLog",
                column: "DeleteById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ActionTransactionsLog_MinistryUsers_UpdateById",
                table: "ActionTransactionsLog",
                column: "UpdateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ActionTransactionsLog_ServiceAction_ServiceActionId",
                table: "ActionTransactionsLog",
                column: "ServiceActionId",
                principalTable: "ServiceAction",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ActionTransactionsLog_ServiceRequests_ServiceRequestId",
                table: "ActionTransactionsLog",
                column: "ServiceRequestId",
                principalTable: "ServiceRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ActionTransactionsLog_SystemModules_SystemModuleId",
                table: "ActionTransactionsLog",
                column: "SystemModuleId",
                principalTable: "SystemModules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Plans_PlanTypeDep_PlanTypeDepId",
                table: "Plans",
                column: "PlanTypeDepId",
                principalTable: "PlanTypeDep",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SchoolLevel_Schools_SchoolId",
                table: "SchoolLevel",
                column: "SchoolId",
                principalTable: "Schools",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserDepartment_Departments_DepartmentId",
                table: "UserDepartment",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserDepartment_MinistryUsers_CreateById",
                table: "UserDepartment",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserDepartment_MinistryUsers_DeleteById",
                table: "UserDepartment",
                column: "DeleteById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserDepartment_MinistryUsers_UpdateById",
                table: "UserDepartment",
                column: "UpdateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserDepartment_MinistryUsers_UserId",
                table: "UserDepartment",
                column: "UserId",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActionTransactionsLog_MinistryUsers_CreateById",
                table: "ActionTransactionsLog");

            migrationBuilder.DropForeignKey(
                name: "FK_ActionTransactionsLog_MinistryUsers_DeleteById",
                table: "ActionTransactionsLog");

            migrationBuilder.DropForeignKey(
                name: "FK_ActionTransactionsLog_MinistryUsers_UpdateById",
                table: "ActionTransactionsLog");

            migrationBuilder.DropForeignKey(
                name: "FK_ActionTransactionsLog_ServiceAction_ServiceActionId",
                table: "ActionTransactionsLog");

            migrationBuilder.DropForeignKey(
                name: "FK_ActionTransactionsLog_ServiceRequests_ServiceRequestId",
                table: "ActionTransactionsLog");

            migrationBuilder.DropForeignKey(
                name: "FK_ActionTransactionsLog_SystemModules_SystemModuleId",
                table: "ActionTransactionsLog");

            migrationBuilder.DropForeignKey(
                name: "FK_Plans_PlanTypeDep_PlanTypeDepId",
                table: "Plans");

            migrationBuilder.DropForeignKey(
                name: "FK_SchoolLevel_Schools_SchoolId",
                table: "SchoolLevel");

            migrationBuilder.DropForeignKey(
                name: "FK_UserDepartment_Departments_DepartmentId",
                table: "UserDepartment");

            migrationBuilder.DropForeignKey(
                name: "FK_UserDepartment_MinistryUsers_CreateById",
                table: "UserDepartment");

            migrationBuilder.DropForeignKey(
                name: "FK_UserDepartment_MinistryUsers_DeleteById",
                table: "UserDepartment");

            migrationBuilder.DropForeignKey(
                name: "FK_UserDepartment_MinistryUsers_UpdateById",
                table: "UserDepartment");

            migrationBuilder.DropForeignKey(
                name: "FK_UserDepartment_MinistryUsers_UserId",
                table: "UserDepartment");

            migrationBuilder.DropTable(
                name: "EvalAttachments");

            migrationBuilder.DropTable(
                name: "PlanTypeDep");

            migrationBuilder.DropIndex(
                name: "IX_Plans_PlanTypeDepId",
                table: "Plans");

            migrationBuilder.DropColumn(
                name: "BackendName",
                table: "Semesters");

            migrationBuilder.DropColumn(
                name: "NSISCode",
                table: "Semesters");

            migrationBuilder.DropColumn(
                name: "OrderNo",
                table: "Semesters");

            migrationBuilder.DropColumn(
                name: "NSISCode",
                table: "SchoolLevel");

            migrationBuilder.DropColumn(
                name: "BackendName",
                table: "PlanStatuses");

            migrationBuilder.DropColumn(
                name: "OrderNo",
                table: "PlanStatuses");

            migrationBuilder.DropColumn(
                name: "PlanTypeDepId",
                table: "Plans");

            migrationBuilder.AlterColumn<Guid>(
                name: "PlanStatusId",
                table: "Plans",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "AcademicYearId",
                table: "Plans",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DepartmentId",
                table: "Plans",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "PlanTypeDepartmentId",
                table: "Plans",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Attachment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActionTransactionsLogId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ChildFieldId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EvaluationRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FieldId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FileExtension = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    Index = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0"),
                    IsOthers = table.Column<bool>(type: "bit", nullable: false),
                    ServiceRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UiFileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attachment_ActionTransactionsLog_ActionTransactionsLogId",
                        column: x => x.ActionTransactionsLogId,
                        principalTable: "ActionTransactionsLog",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Attachment_EvaluationRequest_EvaluationRequestId",
                        column: x => x.EvaluationRequestId,
                        principalTable: "EvaluationRequest",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Attachment_Field_ChildFieldId",
                        column: x => x.ChildFieldId,
                        principalTable: "Field",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Attachment_Field_FieldId",
                        column: x => x.FieldId,
                        principalTable: "Field",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Attachment_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Attachment_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Attachment_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PlanTypeDepartment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlanTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderNo = table.Column<int>(type: "int", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanTypeDepartment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanTypeDepartment_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlanTypeDepartment_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlanTypeDepartment_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlanTypeDepartment_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlanTypeDepartment_PlanType_PlanTypeId",
                        column: x => x.PlanTypeId,
                        principalTable: "PlanType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Plans_DepartmentId",
                table: "Plans",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Plans_PlanTypeDepartmentId",
                table: "Plans",
                column: "PlanTypeDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Attachment_ActionTransactionsLogId",
                table: "Attachment",
                column: "ActionTransactionsLogId");

            migrationBuilder.CreateIndex(
                name: "IX_Attachment_ChildFieldId",
                table: "Attachment",
                column: "ChildFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_Attachment_CreateById",
                table: "Attachment",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_Attachment_DeleteById",
                table: "Attachment",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_Attachment_EvaluationRequestId",
                table: "Attachment",
                column: "EvaluationRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Attachment_FieldId",
                table: "Attachment",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_Attachment_UpdateById",
                table: "Attachment",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_PlanTypeDepartment_CreateById",
                table: "PlanTypeDepartment",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_PlanTypeDepartment_DeleteById",
                table: "PlanTypeDepartment",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_PlanTypeDepartment_DepartmentId",
                table: "PlanTypeDepartment",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanTypeDepartment_PlanTypeId",
                table: "PlanTypeDepartment",
                column: "PlanTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanTypeDepartment_UpdateById",
                table: "PlanTypeDepartment",
                column: "UpdateById");

            migrationBuilder.AddForeignKey(
                name: "FK_ActionTransactionsLog_MinistryUsers_CreateById",
                table: "ActionTransactionsLog",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ActionTransactionsLog_MinistryUsers_DeleteById",
                table: "ActionTransactionsLog",
                column: "DeleteById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ActionTransactionsLog_MinistryUsers_UpdateById",
                table: "ActionTransactionsLog",
                column: "UpdateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ActionTransactionsLog_ServiceAction_ServiceActionId",
                table: "ActionTransactionsLog",
                column: "ServiceActionId",
                principalTable: "ServiceAction",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ActionTransactionsLog_ServiceRequests_ServiceRequestId",
                table: "ActionTransactionsLog",
                column: "ServiceRequestId",
                principalTable: "ServiceRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ActionTransactionsLog_SystemModules_SystemModuleId",
                table: "ActionTransactionsLog",
                column: "SystemModuleId",
                principalTable: "SystemModules",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Plans_Departments_DepartmentId",
                table: "Plans",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Plans_PlanTypeDepartment_PlanTypeDepartmentId",
                table: "Plans",
                column: "PlanTypeDepartmentId",
                principalTable: "PlanTypeDepartment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SchoolLevel_OrgTree_SchoolId",
                table: "SchoolLevel",
                column: "SchoolId",
                principalTable: "OrgTree",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserDepartment_Departments_DepartmentId",
                table: "UserDepartment",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserDepartment_MinistryUsers_CreateById",
                table: "UserDepartment",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserDepartment_MinistryUsers_DeleteById",
                table: "UserDepartment",
                column: "DeleteById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserDepartment_MinistryUsers_UpdateById",
                table: "UserDepartment",
                column: "UpdateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserDepartment_MinistryUsers_UserId",
                table: "UserDepartment",
                column: "UserId",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
