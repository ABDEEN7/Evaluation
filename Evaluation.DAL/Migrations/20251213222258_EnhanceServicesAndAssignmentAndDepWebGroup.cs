using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evaluation.DAL.Migrations
{
    /// <inheritdoc />
    public partial class EnhanceServicesAndAssignmentAndDepWebGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EvaluationRequest_EvaluationType_EvaluationTypeId",
                table: "EvaluationRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_SystemModuleType_MinistryUsers_CreateById",
                table: "SystemModuleType");

            migrationBuilder.DropForeignKey(
                name: "FK_SystemModuleType_MinistryUsers_DeleteById",
                table: "SystemModuleType");

            migrationBuilder.DropForeignKey(
                name: "FK_SystemModuleType_MinistryUsers_UpdateById",
                table: "SystemModuleType");

            migrationBuilder.DropForeignKey(
                name: "FK_SystemModuleType_SystemModuleType_ParentModuleTypeId",
                table: "SystemModuleType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SystemModuleType",
                table: "SystemModuleType");

            migrationBuilder.DropColumn(
                name: "IsNDA",
                table: "RequestAssignment");

            migrationBuilder.DropColumn(
                name: "NDAApproveDate",
                table: "RequestAssignment");

            migrationBuilder.DropColumn(
                name: "NDAStatusId",
                table: "RequestAssignment");

            migrationBuilder.RenameTable(
                name: "SystemModuleType",
                newName: "SystemModuleTypes");

            migrationBuilder.RenameColumn(
                name: "SchNoDefinition",
                table: "SystemModules",
                newName: "NoDefinition");

            migrationBuilder.RenameColumn(
                name: "EvaluationTypeId",
                table: "EvaluationRequest",
                newName: "DepEvaluationTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_EvaluationRequest_EvaluationTypeId",
                table: "EvaluationRequest",
                newName: "IX_EvaluationRequest_DepEvaluationTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_SystemModuleType_UpdateById",
                table: "SystemModuleTypes",
                newName: "IX_SystemModuleTypes_UpdateById");

            migrationBuilder.RenameIndex(
                name: "IX_SystemModuleType_ParentModuleTypeId",
                table: "SystemModuleTypes",
                newName: "IX_SystemModuleTypes_ParentModuleTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_SystemModuleType_DeleteById",
                table: "SystemModuleTypes",
                newName: "IX_SystemModuleTypes_DeleteById");

            migrationBuilder.RenameIndex(
                name: "IX_SystemModuleType_CreateById",
                table: "SystemModuleTypes",
                newName: "IX_SystemModuleTypes_CreateById");

            migrationBuilder.AddColumn<Guid>(
                name: "SystemModuleTypeId",
                table: "SystemModules",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "RequestAssignment",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "WebGroupId",
                table: "Banner",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SystemModuleTypes",
                table: "SystemModuleTypes",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "DepEvaluationType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BackendName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    table.PrimaryKey("PK_DepEvaluationType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DepEvaluationType_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DepEvaluationType_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DepEvaluationType_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DepEvaluationType_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EvaluationRequestAssignment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MinistryUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EvaluationRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PartyTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsLeader = table.Column<bool>(type: "bit", nullable: false),
                    IsNDA = table.Column<bool>(type: "bit", nullable: false),
                    NDAStatusId = table.Column<bool>(type: "bit", nullable: false),
                    NDAApproveDate = table.Column<bool>(type: "bit", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_EvaluationRequestAssignment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EvaluationRequestAssignment_EvaluationRequest_EvaluationRequestId",
                        column: x => x.EvaluationRequestId,
                        principalTable: "EvaluationRequest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EvaluationRequestAssignment_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EvaluationRequestAssignment_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EvaluationRequestAssignment_MinistryUsers_MinistryUserId",
                        column: x => x.MinistryUserId,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EvaluationRequestAssignment_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EvaluationRequestAssignment_PartyTypes_PartyTypeId",
                        column: x => x.PartyTypeId,
                        principalTable: "PartyTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WebGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoutingPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BackendName = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    table.PrimaryKey("PK_WebGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WebGroups_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WebGroups_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WebGroups_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EvalRequestAssignmentScope",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EvaluationRequestAssignmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScopeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_EvalRequestAssignmentScope", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EvalRequestAssignmentScope_EvaluationRequestAssignment_EvaluationRequestAssignmentId",
                        column: x => x.EvaluationRequestAssignmentId,
                        principalTable: "EvaluationRequestAssignment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EvalRequestAssignmentScope_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EvalRequestAssignmentScope_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EvalRequestAssignmentScope_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EvalRequestAssignmentScope_Scopes_ScopeId",
                        column: x => x.ScopeId,
                        principalTable: "Scopes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DepWebGroup",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WebGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_DepWebGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DepWebGroup_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DepWebGroup_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DepWebGroup_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DepWebGroup_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DepWebGroup_WebGroups_WebGroupId",
                        column: x => x.WebGroupId,
                        principalTable: "WebGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SystemModules_SystemModuleTypeId",
                table: "SystemModules",
                column: "SystemModuleTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Banner_WebGroupId",
                table: "Banner",
                column: "WebGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_DepEvaluationType_CreateById",
                table: "DepEvaluationType",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_DepEvaluationType_DeleteById",
                table: "DepEvaluationType",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_DepEvaluationType_DepartmentId",
                table: "DepEvaluationType",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_DepEvaluationType_UpdateById",
                table: "DepEvaluationType",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_DepWebGroup_CreateById",
                table: "DepWebGroup",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_DepWebGroup_DeleteById",
                table: "DepWebGroup",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_DepWebGroup_DepartmentId",
                table: "DepWebGroup",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_DepWebGroup_UpdateById",
                table: "DepWebGroup",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_DepWebGroup_WebGroupId",
                table: "DepWebGroup",
                column: "WebGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_EvalRequestAssignmentScope_CreateById",
                table: "EvalRequestAssignmentScope",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_EvalRequestAssignmentScope_DeleteById",
                table: "EvalRequestAssignmentScope",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_EvalRequestAssignmentScope_EvaluationRequestAssignmentId",
                table: "EvalRequestAssignmentScope",
                column: "EvaluationRequestAssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_EvalRequestAssignmentScope_ScopeId",
                table: "EvalRequestAssignmentScope",
                column: "ScopeId");

            migrationBuilder.CreateIndex(
                name: "IX_EvalRequestAssignmentScope_UpdateById",
                table: "EvalRequestAssignmentScope",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_EvaluationRequestAssignment_CreateById",
                table: "EvaluationRequestAssignment",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_EvaluationRequestAssignment_DeleteById",
                table: "EvaluationRequestAssignment",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_EvaluationRequestAssignment_EvaluationRequestId",
                table: "EvaluationRequestAssignment",
                column: "EvaluationRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_EvaluationRequestAssignment_MinistryUserId",
                table: "EvaluationRequestAssignment",
                column: "MinistryUserId");

            migrationBuilder.CreateIndex(
                name: "IX_EvaluationRequestAssignment_PartyTypeId",
                table: "EvaluationRequestAssignment",
                column: "PartyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_EvaluationRequestAssignment_UpdateById",
                table: "EvaluationRequestAssignment",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_WebGroups_CreateById",
                table: "WebGroups",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_WebGroups_DeleteById",
                table: "WebGroups",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_WebGroups_UpdateById",
                table: "WebGroups",
                column: "UpdateById");

            migrationBuilder.AddForeignKey(
                name: "FK_Banner_WebGroups_WebGroupId",
                table: "Banner",
                column: "WebGroupId",
                principalTable: "WebGroups",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EvaluationRequest_DepEvaluationType_DepEvaluationTypeId",
                table: "EvaluationRequest",
                column: "DepEvaluationTypeId",
                principalTable: "DepEvaluationType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SystemModules_SystemModuleTypes_SystemModuleTypeId",
                table: "SystemModules",
                column: "SystemModuleTypeId",
                principalTable: "SystemModuleTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SystemModuleTypes_MinistryUsers_CreateById",
                table: "SystemModuleTypes",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SystemModuleTypes_MinistryUsers_DeleteById",
                table: "SystemModuleTypes",
                column: "DeleteById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SystemModuleTypes_MinistryUsers_UpdateById",
                table: "SystemModuleTypes",
                column: "UpdateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SystemModuleTypes_SystemModuleTypes_ParentModuleTypeId",
                table: "SystemModuleTypes",
                column: "ParentModuleTypeId",
                principalTable: "SystemModuleTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Banner_WebGroups_WebGroupId",
                table: "Banner");

            migrationBuilder.DropForeignKey(
                name: "FK_EvaluationRequest_DepEvaluationType_DepEvaluationTypeId",
                table: "EvaluationRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_SystemModules_SystemModuleTypes_SystemModuleTypeId",
                table: "SystemModules");

            migrationBuilder.DropForeignKey(
                name: "FK_SystemModuleTypes_MinistryUsers_CreateById",
                table: "SystemModuleTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_SystemModuleTypes_MinistryUsers_DeleteById",
                table: "SystemModuleTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_SystemModuleTypes_MinistryUsers_UpdateById",
                table: "SystemModuleTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_SystemModuleTypes_SystemModuleTypes_ParentModuleTypeId",
                table: "SystemModuleTypes");

            migrationBuilder.DropTable(
                name: "DepEvaluationType");

            migrationBuilder.DropTable(
                name: "DepWebGroup");

            migrationBuilder.DropTable(
                name: "EvalRequestAssignmentScope");

            migrationBuilder.DropTable(
                name: "WebGroups");

            migrationBuilder.DropTable(
                name: "EvaluationRequestAssignment");

            migrationBuilder.DropIndex(
                name: "IX_SystemModules_SystemModuleTypeId",
                table: "SystemModules");

            migrationBuilder.DropIndex(
                name: "IX_Banner_WebGroupId",
                table: "Banner");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SystemModuleTypes",
                table: "SystemModuleTypes");

            migrationBuilder.DropColumn(
                name: "SystemModuleTypeId",
                table: "SystemModules");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "RequestAssignment");

            migrationBuilder.DropColumn(
                name: "WebGroupId",
                table: "Banner");

            migrationBuilder.RenameTable(
                name: "SystemModuleTypes",
                newName: "SystemModuleType");

            migrationBuilder.RenameColumn(
                name: "NoDefinition",
                table: "SystemModules",
                newName: "SchNoDefinition");

            migrationBuilder.RenameColumn(
                name: "DepEvaluationTypeId",
                table: "EvaluationRequest",
                newName: "EvaluationTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_EvaluationRequest_DepEvaluationTypeId",
                table: "EvaluationRequest",
                newName: "IX_EvaluationRequest_EvaluationTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_SystemModuleTypes_UpdateById",
                table: "SystemModuleType",
                newName: "IX_SystemModuleType_UpdateById");

            migrationBuilder.RenameIndex(
                name: "IX_SystemModuleTypes_ParentModuleTypeId",
                table: "SystemModuleType",
                newName: "IX_SystemModuleType_ParentModuleTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_SystemModuleTypes_DeleteById",
                table: "SystemModuleType",
                newName: "IX_SystemModuleType_DeleteById");

            migrationBuilder.RenameIndex(
                name: "IX_SystemModuleTypes_CreateById",
                table: "SystemModuleType",
                newName: "IX_SystemModuleType_CreateById");

            migrationBuilder.AddColumn<bool>(
                name: "IsNDA",
                table: "RequestAssignment",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "NDAApproveDate",
                table: "RequestAssignment",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "NDAStatusId",
                table: "RequestAssignment",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SystemModuleType",
                table: "SystemModuleType",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EvaluationRequest_EvaluationType_EvaluationTypeId",
                table: "EvaluationRequest",
                column: "EvaluationTypeId",
                principalTable: "EvaluationType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SystemModuleType_MinistryUsers_CreateById",
                table: "SystemModuleType",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SystemModuleType_MinistryUsers_DeleteById",
                table: "SystemModuleType",
                column: "DeleteById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SystemModuleType_MinistryUsers_UpdateById",
                table: "SystemModuleType",
                column: "UpdateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SystemModuleType_SystemModuleType_ParentModuleTypeId",
                table: "SystemModuleType",
                column: "ParentModuleTypeId",
                principalTable: "SystemModuleType",
                principalColumn: "Id");
        }
    }
}
