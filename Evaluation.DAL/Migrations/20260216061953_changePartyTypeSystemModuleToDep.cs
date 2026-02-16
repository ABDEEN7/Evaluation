using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evaluation.DAL.Migrations
{
    /// <inheritdoc />
    public partial class changePartyTypeSystemModuleToDep : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PartyTypes_SystemModules_SystemModuleId",
                table: "PartyTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_TemplateDocuments_SystemModules_SystemModuleId",
                table: "TemplateDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTeamScope_MinistryUsers_CreateById",
                table: "UserTeamScope");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTeamScope_MinistryUsers_DeleteById",
                table: "UserTeamScope");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTeamScope_MinistryUsers_UpdateById",
                table: "UserTeamScope");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTeamScope_Scopes_ScopeId",
                table: "UserTeamScope");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTeamScope_UserTeams_UserTeamId",
                table: "UserTeamScope");

            migrationBuilder.DropIndex(
                name: "IX_PartyTypes_SystemModuleId",
                table: "PartyTypes");

            migrationBuilder.DropColumn(
                name: "SystemModuletId",
                table: "TemplateDocuments");

            migrationBuilder.DropColumn(
                name: "SystemModuleId",
                table: "PartyTypes");

            migrationBuilder.RenameColumn(
                name: "SystemModuleId",
                table: "TemplateDocuments",
                newName: "DepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_TemplateDocuments_SystemModuleId",
                table: "TemplateDocuments",
                newName: "IX_TemplateDocuments_DepartmentId");

            migrationBuilder.AddColumn<Guid>(
                name: "DepartmentId",
                table: "PartyTypes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ServiceStatusConfiguration",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrentStatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NextStatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_ServiceStatusConfiguration", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceStatusConfiguration_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceStatusConfiguration_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ServiceStatusConfiguration_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ServiceStatusConfiguration_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PartyTypes_DepartmentId",
                table: "PartyTypes",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceStatusConfiguration_CreateById",
                table: "ServiceStatusConfiguration",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceStatusConfiguration_DeleteById",
                table: "ServiceStatusConfiguration",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceStatusConfiguration_ServiceId",
                table: "ServiceStatusConfiguration",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceStatusConfiguration_UpdateById",
                table: "ServiceStatusConfiguration",
                column: "UpdateById");

            migrationBuilder.AddForeignKey(
                name: "FK_PartyTypes_Departments_DepartmentId",
                table: "PartyTypes",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TemplateDocuments_Departments_DepartmentId",
                table: "TemplateDocuments",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTeamScope_MinistryUsers_CreateById",
                table: "UserTeamScope",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTeamScope_MinistryUsers_DeleteById",
                table: "UserTeamScope",
                column: "DeleteById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTeamScope_MinistryUsers_UpdateById",
                table: "UserTeamScope",
                column: "UpdateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTeamScope_Scopes_ScopeId",
                table: "UserTeamScope",
                column: "ScopeId",
                principalTable: "Scopes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTeamScope_UserTeams_UserTeamId",
                table: "UserTeamScope",
                column: "UserTeamId",
                principalTable: "UserTeams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PartyTypes_Departments_DepartmentId",
                table: "PartyTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_TemplateDocuments_Departments_DepartmentId",
                table: "TemplateDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTeamScope_MinistryUsers_CreateById",
                table: "UserTeamScope");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTeamScope_MinistryUsers_DeleteById",
                table: "UserTeamScope");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTeamScope_MinistryUsers_UpdateById",
                table: "UserTeamScope");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTeamScope_Scopes_ScopeId",
                table: "UserTeamScope");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTeamScope_UserTeams_UserTeamId",
                table: "UserTeamScope");

            migrationBuilder.DropTable(
                name: "ServiceStatusConfiguration");

            migrationBuilder.DropIndex(
                name: "IX_PartyTypes_DepartmentId",
                table: "PartyTypes");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "PartyTypes");

            migrationBuilder.RenameColumn(
                name: "DepartmentId",
                table: "TemplateDocuments",
                newName: "SystemModuleId");

            migrationBuilder.RenameIndex(
                name: "IX_TemplateDocuments_DepartmentId",
                table: "TemplateDocuments",
                newName: "IX_TemplateDocuments_SystemModuleId");

            migrationBuilder.AddColumn<Guid>(
                name: "SystemModuletId",
                table: "TemplateDocuments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SystemModuleId",
                table: "PartyTypes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_PartyTypes_SystemModuleId",
                table: "PartyTypes",
                column: "SystemModuleId");

            migrationBuilder.AddForeignKey(
                name: "FK_PartyTypes_SystemModules_SystemModuleId",
                table: "PartyTypes",
                column: "SystemModuleId",
                principalTable: "SystemModules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TemplateDocuments_SystemModules_SystemModuleId",
                table: "TemplateDocuments",
                column: "SystemModuleId",
                principalTable: "SystemModules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTeamScope_MinistryUsers_CreateById",
                table: "UserTeamScope",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTeamScope_MinistryUsers_DeleteById",
                table: "UserTeamScope",
                column: "DeleteById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserTeamScope_MinistryUsers_UpdateById",
                table: "UserTeamScope",
                column: "UpdateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserTeamScope_Scopes_ScopeId",
                table: "UserTeamScope",
                column: "ScopeId",
                principalTable: "Scopes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTeamScope_UserTeams_UserTeamId",
                table: "UserTeamScope",
                column: "UserTeamId",
                principalTable: "UserTeams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
