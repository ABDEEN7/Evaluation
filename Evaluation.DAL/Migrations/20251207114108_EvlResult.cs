using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evaluation.DAL.Migrations
{
    /// <inheritdoc />
    public partial class EvlResult : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceInitiatorPartyType_MinistryUsers_CreateById",
                table: "ServiceInitiatorPartyType");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceInitiatorPartyType_MinistryUsers_DeleteById",
                table: "ServiceInitiatorPartyType");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceInitiatorPartyType_MinistryUsers_UpdateById",
                table: "ServiceInitiatorPartyType");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceInitiatorPartyType_PartyTypes_PartyTypeId",
                table: "ServiceInitiatorPartyType");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceInitiatorPartyType_Services_serviceId",
                table: "ServiceInitiatorPartyType");

            migrationBuilder.DropForeignKey(
                name: "FK_Team_Departments_DepartmentId",
                table: "Team");

            migrationBuilder.DropForeignKey(
                name: "FK_Team_MinistryUsers_CreateById",
                table: "Team");

            migrationBuilder.DropForeignKey(
                name: "FK_Team_MinistryUsers_DeleteById",
                table: "Team");

            migrationBuilder.DropForeignKey(
                name: "FK_Team_MinistryUsers_UpdateById",
                table: "Team");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTeamScope_MinistryUsers_UserId",
                table: "UserTeamScope");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTeamScope_Team_TeamId",
                table: "UserTeamScope");

            migrationBuilder.DropTable(
                name: "ScopeUserTeam");

            migrationBuilder.DropIndex(
                name: "IX_UserTeamScope_TeamId",
                table: "UserTeamScope");

            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "UserTeamScope");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "UserTeamScope",
                newName: "UserTeamId");

            migrationBuilder.RenameIndex(
                name: "IX_UserTeamScope_UserId",
                table: "UserTeamScope",
                newName: "IX_UserTeamScope_UserTeamId");

            migrationBuilder.AddColumn<bool>(
                name: "ShowInCalendar",
                table: "Services",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<long>(
                name: "Sequence",
                table: "ServiceRequests",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "VisitDateFrom",
                table: "ServiceRequests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "VisitDateTo",
                table: "ServiceRequests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BackendName",
                table: "ScopeTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "AcceditedDate",
                table: "OrgTree",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAccredited",
                table: "OrgTree",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SupportIdentity",
                table: "OrgTree",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateOnly>(
                name: "SupportIdentityDate",
                table: "OrgTree",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<bool>(
                name: "IsOrgManager",
                table: "JobTitle",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsOrgManager",
                table: "Employees",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "StartDate",
                table: "AcademicYears",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "EndDate",
                table: "AcademicYears",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.CreateTable(
                name: "DepEvalMatrixs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AcademicYearId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BackendName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MinValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaxValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RequiredFollowUp = table.Column<bool>(type: "bit", nullable: false),
                    NextFollowUpDays = table.Column<int>(type: "int", nullable: false),
                    NextEvaluationDays = table.Column<int>(type: "int", nullable: false),
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
                name: "RequestAssignmentScopes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequestAssignmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScopeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_RequestAssignmentScopes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestAssignmentScopes_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequestAssignmentScopes_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequestAssignmentScopes_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequestAssignmentScopes_RequestAssignment_RequestAssignmentId",
                        column: x => x.RequestAssignmentId,
                        principalTable: "RequestAssignment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequestAssignmentScopes_Scopes_ScopeId",
                        column: x => x.ScopeId,
                        principalTable: "Scopes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserTeams",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_UserTeams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTeams_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserTeams_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserTeams_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserTeams_MinistryUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserTeams_Team_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Team",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrgEvalResults",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AcademicYearId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrgTreeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DepEvalMatrixId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FinalEvalValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
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

            migrationBuilder.CreateIndex(
                name: "IX_RequestAssignmentScopes_CreateById",
                table: "RequestAssignmentScopes",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_RequestAssignmentScopes_DeleteById",
                table: "RequestAssignmentScopes",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_RequestAssignmentScopes_RequestAssignmentId",
                table: "RequestAssignmentScopes",
                column: "RequestAssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestAssignmentScopes_ScopeId",
                table: "RequestAssignmentScopes",
                column: "ScopeId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestAssignmentScopes_UpdateById",
                table: "RequestAssignmentScopes",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_UserTeams_CreateById",
                table: "UserTeams",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_UserTeams_DeleteById",
                table: "UserTeams",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_UserTeams_TeamId",
                table: "UserTeams",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTeams_UpdateById",
                table: "UserTeams",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_UserTeams_UserId",
                table: "UserTeams",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceInitiatorPartyType_MinistryUsers_CreateById",
                table: "ServiceInitiatorPartyType",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceInitiatorPartyType_MinistryUsers_DeleteById",
                table: "ServiceInitiatorPartyType",
                column: "DeleteById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceInitiatorPartyType_MinistryUsers_UpdateById",
                table: "ServiceInitiatorPartyType",
                column: "UpdateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceInitiatorPartyType_PartyTypes_PartyTypeId",
                table: "ServiceInitiatorPartyType",
                column: "PartyTypeId",
                principalTable: "PartyTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceInitiatorPartyType_Services_serviceId",
                table: "ServiceInitiatorPartyType",
                column: "serviceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Team_Departments_DepartmentId",
                table: "Team",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Team_MinistryUsers_CreateById",
                table: "Team",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Team_MinistryUsers_DeleteById",
                table: "Team",
                column: "DeleteById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Team_MinistryUsers_UpdateById",
                table: "Team",
                column: "UpdateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTeamScope_UserTeams_UserTeamId",
                table: "UserTeamScope",
                column: "UserTeamId",
                principalTable: "UserTeams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceInitiatorPartyType_MinistryUsers_CreateById",
                table: "ServiceInitiatorPartyType");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceInitiatorPartyType_MinistryUsers_DeleteById",
                table: "ServiceInitiatorPartyType");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceInitiatorPartyType_MinistryUsers_UpdateById",
                table: "ServiceInitiatorPartyType");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceInitiatorPartyType_PartyTypes_PartyTypeId",
                table: "ServiceInitiatorPartyType");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceInitiatorPartyType_Services_serviceId",
                table: "ServiceInitiatorPartyType");

            migrationBuilder.DropForeignKey(
                name: "FK_Team_Departments_DepartmentId",
                table: "Team");

            migrationBuilder.DropForeignKey(
                name: "FK_Team_MinistryUsers_CreateById",
                table: "Team");

            migrationBuilder.DropForeignKey(
                name: "FK_Team_MinistryUsers_DeleteById",
                table: "Team");

            migrationBuilder.DropForeignKey(
                name: "FK_Team_MinistryUsers_UpdateById",
                table: "Team");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTeamScope_UserTeams_UserTeamId",
                table: "UserTeamScope");

            migrationBuilder.DropTable(
                name: "OrgEvalResults");

            migrationBuilder.DropTable(
                name: "RequestAssignmentScopes");

            migrationBuilder.DropTable(
                name: "UserTeams");

            migrationBuilder.DropTable(
                name: "DepEvalMatrixs");

            migrationBuilder.DropColumn(
                name: "ShowInCalendar",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "VisitDateFrom",
                table: "ServiceRequests");

            migrationBuilder.DropColumn(
                name: "VisitDateTo",
                table: "ServiceRequests");

            migrationBuilder.DropColumn(
                name: "BackendName",
                table: "ScopeTypes");

            migrationBuilder.DropColumn(
                name: "AcceditedDate",
                table: "OrgTree");

            migrationBuilder.DropColumn(
                name: "IsAccredited",
                table: "OrgTree");

            migrationBuilder.DropColumn(
                name: "SupportIdentity",
                table: "OrgTree");

            migrationBuilder.DropColumn(
                name: "SupportIdentityDate",
                table: "OrgTree");

            migrationBuilder.DropColumn(
                name: "IsOrgManager",
                table: "JobTitle");

            migrationBuilder.DropColumn(
                name: "IsOrgManager",
                table: "Employees");

            migrationBuilder.RenameColumn(
                name: "UserTeamId",
                table: "UserTeamScope",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserTeamScope_UserTeamId",
                table: "UserTeamScope",
                newName: "IX_UserTeamScope_UserId");

            migrationBuilder.AddColumn<Guid>(
                name: "TeamId",
                table: "UserTeamScope",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<int>(
                name: "Sequence",
                table: "ServiceRequests",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "AcademicYears",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "AcademicYears",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.CreateTable(
                name: "ScopeUserTeam",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ScopeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0"),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScopeUserTeam", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScopeUserTeam_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ScopeUserTeam_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ScopeUserTeam_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ScopeUserTeam_MinistryUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScopeUserTeam_Scopes_ScopeId",
                        column: x => x.ScopeId,
                        principalTable: "Scopes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScopeUserTeam_Team_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Team",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserTeamScope_TeamId",
                table: "UserTeamScope",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_ScopeUserTeam_CreateById",
                table: "ScopeUserTeam",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_ScopeUserTeam_DeleteById",
                table: "ScopeUserTeam",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_ScopeUserTeam_ScopeId",
                table: "ScopeUserTeam",
                column: "ScopeId");

            migrationBuilder.CreateIndex(
                name: "IX_ScopeUserTeam_TeamId",
                table: "ScopeUserTeam",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_ScopeUserTeam_UpdateById",
                table: "ScopeUserTeam",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_ScopeUserTeam_UserId",
                table: "ScopeUserTeam",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceInitiatorPartyType_MinistryUsers_CreateById",
                table: "ServiceInitiatorPartyType",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceInitiatorPartyType_MinistryUsers_DeleteById",
                table: "ServiceInitiatorPartyType",
                column: "DeleteById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceInitiatorPartyType_MinistryUsers_UpdateById",
                table: "ServiceInitiatorPartyType",
                column: "UpdateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceInitiatorPartyType_PartyTypes_PartyTypeId",
                table: "ServiceInitiatorPartyType",
                column: "PartyTypeId",
                principalTable: "PartyTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceInitiatorPartyType_Services_serviceId",
                table: "ServiceInitiatorPartyType",
                column: "serviceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Team_Departments_DepartmentId",
                table: "Team",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Team_MinistryUsers_CreateById",
                table: "Team",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Team_MinistryUsers_DeleteById",
                table: "Team",
                column: "DeleteById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Team_MinistryUsers_UpdateById",
                table: "Team",
                column: "UpdateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserTeamScope_MinistryUsers_UserId",
                table: "UserTeamScope",
                column: "UserId",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTeamScope_Team_TeamId",
                table: "UserTeamScope",
                column: "TeamId",
                principalTable: "Team",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
