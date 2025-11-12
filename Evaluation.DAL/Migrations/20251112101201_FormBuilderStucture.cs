using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evaluation.DAL.Migrations
{
    /// <inheritdoc />
    public partial class FormBuilderStucture : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentHoliday_AcademicYears_AcademicYearId",
                table: "DepartmentHoliday");

            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentHoliday_Departments_DepartmentId",
                table: "DepartmentHoliday");

            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentHoliday_MinistryUsers_CreateById",
                table: "DepartmentHoliday");

            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentHoliday_MinistryUsers_DeleteById",
                table: "DepartmentHoliday");

            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentHoliday_MinistryUsers_UpdateById",
                table: "DepartmentHoliday");

            migrationBuilder.DropForeignKey(
                name: "FK_OrgTree_UserGender_UserGenderId",
                table: "OrgTree");

            migrationBuilder.DropIndex(
                name: "IX_OrgTree_UserGenderId",
                table: "OrgTree");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DepartmentHoliday",
                table: "DepartmentHoliday");

            migrationBuilder.DropIndex(
                name: "IX_DepartmentHoliday_DepartmentId",
                table: "DepartmentHoliday");

            migrationBuilder.DropColumn(
                name: "CanViewEntityEvaluation",
                table: "PartyTypes");

            migrationBuilder.DropColumn(
                name: "UserGenderId",
                table: "OrgTree");

            migrationBuilder.DropColumn(
                name: "ActionStepFieldId",
                table: "ActionFieldAttribute");

            migrationBuilder.DropColumn(
                name: "IsUpdateOnModule",
                table: "ActionField");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "DepartmentHoliday");

            migrationBuilder.RenameTable(
                name: "DepartmentHoliday",
                newName: "DepartmentHolidays");

            migrationBuilder.RenameIndex(
                name: "IX_DepartmentHoliday_UpdateById",
                table: "DepartmentHolidays",
                newName: "IX_DepartmentHolidays_UpdateById");

            migrationBuilder.RenameIndex(
                name: "IX_DepartmentHoliday_DeleteById",
                table: "DepartmentHolidays",
                newName: "IX_DepartmentHolidays_DeleteById");

            migrationBuilder.RenameIndex(
                name: "IX_DepartmentHoliday_CreateById",
                table: "DepartmentHolidays",
                newName: "IX_DepartmentHolidays_CreateById");

            migrationBuilder.RenameIndex(
                name: "IX_DepartmentHoliday_AcademicYearId",
                table: "DepartmentHolidays",
                newName: "IX_DepartmentHolidays_AcademicYearId");

            migrationBuilder.AddColumn<Guid>(
                name: "AttachmentId",
                table: "TemplateDocuments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAttachment",
                table: "TemplateDocuments",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameAr",
                table: "TemplateDocuments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NameEn",
                table: "TemplateDocuments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "ServiceId",
                table: "TemplateDocuments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SystemModuleId",
                table: "TemplateDocuments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SystemModuletId",
                table: "TemplateDocuments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "TemplateAr",
                table: "TemplateDocuments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TemplateEn",
                table: "TemplateDocuments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TemplateGenrationTypeId",
                table: "TemplateDocuments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ServiceRequestFieldsValueId",
                table: "FieldValueTransactionsLog",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "EvaluationRequestId",
                table: "Attachment",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ServiceRequestId",
                table: "ActionTransactionsLog",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IsCurrent",
                table: "AcademicYears",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DepartmentHolidays",
                table: "DepartmentHolidays",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "EmailProfile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SenderAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SenderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Host = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Port = table.Column<int>(type: "int", nullable: false),
                    EnableSSL = table.Column<bool>(type: "bit", nullable: false),
                    UseDefaultCredentials = table.Column<bool>(type: "bit", nullable: false),
                    IsBodyHTML = table.Column<bool>(type: "bit", nullable: false),
                    EmailRequestTimeout = table.Column<int>(type: "int", nullable: false),
                    BackendName = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("PK_EmailProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailProfile_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmailProfile_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmailProfile_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IntegrationPoint",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BackendName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EndPoint = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    URLParameter = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseSchema = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsInternal = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("PK_IntegrationPoint", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IntegrationPoint_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_IntegrationPoint_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_IntegrationPoint_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TemplateDocuments_ServiceId",
                table: "TemplateDocuments",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateDocuments_SystemModuleId",
                table: "TemplateDocuments",
                column: "SystemModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateDocuments_TemplateGenrationTypeId",
                table: "TemplateDocuments",
                column: "TemplateGenrationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PartyTypes_SystemModuleId",
                table: "PartyTypes",
                column: "SystemModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldValueTransactionsLog_ServiceRequestFieldsValueId",
                table: "FieldValueTransactionsLog",
                column: "ServiceRequestFieldsValueId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailTemplates_EmailProfileId",
                table: "EmailTemplates",
                column: "EmailProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Attachment_EvaluationRequestId",
                table: "Attachment",
                column: "EvaluationRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionTransactionsLog_ServiceRequestId",
                table: "ActionTransactionsLog",
                column: "ServiceRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailProfile_BackendName",
                table: "EmailProfile",
                column: "BackendName",
                unique: true,
                filter: "[BackendName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_EmailProfile_CreateById",
                table: "EmailProfile",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_EmailProfile_DeleteById",
                table: "EmailProfile",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_EmailProfile_UpdateById",
                table: "EmailProfile",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_IntegrationPoint_CreateById",
                table: "IntegrationPoint",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_IntegrationPoint_DeleteById",
                table: "IntegrationPoint",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_IntegrationPoint_UpdateById",
                table: "IntegrationPoint",
                column: "UpdateById");

            migrationBuilder.AddForeignKey(
                name: "FK_ActionTransactionsLog_ServiceRequests_ServiceRequestId",
                table: "ActionTransactionsLog",
                column: "ServiceRequestId",
                principalTable: "ServiceRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Attachment_EvaluationRequest_EvaluationRequestId",
                table: "Attachment",
                column: "EvaluationRequestId",
                principalTable: "EvaluationRequest",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentHolidays_AcademicYears_AcademicYearId",
                table: "DepartmentHolidays",
                column: "AcademicYearId",
                principalTable: "AcademicYears",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentHolidays_MinistryUsers_CreateById",
                table: "DepartmentHolidays",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentHolidays_MinistryUsers_DeleteById",
                table: "DepartmentHolidays",
                column: "DeleteById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentHolidays_MinistryUsers_UpdateById",
                table: "DepartmentHolidays",
                column: "UpdateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EmailTemplates_EmailProfile_EmailProfileId",
                table: "EmailTemplates",
                column: "EmailProfileId",
                principalTable: "EmailProfile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FieldValueTransactionsLog_ServiceRequestFieldsValue_ServiceRequestFieldsValueId",
                table: "FieldValueTransactionsLog",
                column: "ServiceRequestFieldsValueId",
                principalTable: "ServiceRequestFieldsValue",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PartyTypes_SystemModules_SystemModuleId",
                table: "PartyTypes",
                column: "SystemModuleId",
                principalTable: "SystemModules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TemplateDocuments_Services_ServiceId",
                table: "TemplateDocuments",
                column: "ServiceId",
                principalTable: "Services",
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
                name: "FK_TemplateDocuments_TemplateGenrationTypies_TemplateGenrationTypeId",
                table: "TemplateDocuments",
                column: "TemplateGenrationTypeId",
                principalTable: "TemplateGenrationTypies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActionTransactionsLog_ServiceRequests_ServiceRequestId",
                table: "ActionTransactionsLog");

            migrationBuilder.DropForeignKey(
                name: "FK_Attachment_EvaluationRequest_EvaluationRequestId",
                table: "Attachment");

            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentHolidays_AcademicYears_AcademicYearId",
                table: "DepartmentHolidays");

            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentHolidays_MinistryUsers_CreateById",
                table: "DepartmentHolidays");

            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentHolidays_MinistryUsers_DeleteById",
                table: "DepartmentHolidays");

            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentHolidays_MinistryUsers_UpdateById",
                table: "DepartmentHolidays");

            migrationBuilder.DropForeignKey(
                name: "FK_EmailTemplates_EmailProfile_EmailProfileId",
                table: "EmailTemplates");

            migrationBuilder.DropForeignKey(
                name: "FK_FieldValueTransactionsLog_ServiceRequestFieldsValue_ServiceRequestFieldsValueId",
                table: "FieldValueTransactionsLog");

            migrationBuilder.DropForeignKey(
                name: "FK_PartyTypes_SystemModules_SystemModuleId",
                table: "PartyTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_TemplateDocuments_Services_ServiceId",
                table: "TemplateDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_TemplateDocuments_SystemModules_SystemModuleId",
                table: "TemplateDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_TemplateDocuments_TemplateGenrationTypies_TemplateGenrationTypeId",
                table: "TemplateDocuments");

            migrationBuilder.DropTable(
                name: "EmailProfile");

            migrationBuilder.DropTable(
                name: "IntegrationPoint");

            migrationBuilder.DropIndex(
                name: "IX_TemplateDocuments_ServiceId",
                table: "TemplateDocuments");

            migrationBuilder.DropIndex(
                name: "IX_TemplateDocuments_SystemModuleId",
                table: "TemplateDocuments");

            migrationBuilder.DropIndex(
                name: "IX_TemplateDocuments_TemplateGenrationTypeId",
                table: "TemplateDocuments");

            migrationBuilder.DropIndex(
                name: "IX_PartyTypes_SystemModuleId",
                table: "PartyTypes");

            migrationBuilder.DropIndex(
                name: "IX_FieldValueTransactionsLog_ServiceRequestFieldsValueId",
                table: "FieldValueTransactionsLog");

            migrationBuilder.DropIndex(
                name: "IX_EmailTemplates_EmailProfileId",
                table: "EmailTemplates");

            migrationBuilder.DropIndex(
                name: "IX_Attachment_EvaluationRequestId",
                table: "Attachment");

            migrationBuilder.DropIndex(
                name: "IX_ActionTransactionsLog_ServiceRequestId",
                table: "ActionTransactionsLog");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DepartmentHolidays",
                table: "DepartmentHolidays");

            migrationBuilder.DropColumn(
                name: "AttachmentId",
                table: "TemplateDocuments");

            migrationBuilder.DropColumn(
                name: "IsAttachment",
                table: "TemplateDocuments");

            migrationBuilder.DropColumn(
                name: "NameAr",
                table: "TemplateDocuments");

            migrationBuilder.DropColumn(
                name: "NameEn",
                table: "TemplateDocuments");

            migrationBuilder.DropColumn(
                name: "ServiceId",
                table: "TemplateDocuments");

            migrationBuilder.DropColumn(
                name: "SystemModuleId",
                table: "TemplateDocuments");

            migrationBuilder.DropColumn(
                name: "SystemModuletId",
                table: "TemplateDocuments");

            migrationBuilder.DropColumn(
                name: "TemplateAr",
                table: "TemplateDocuments");

            migrationBuilder.DropColumn(
                name: "TemplateEn",
                table: "TemplateDocuments");

            migrationBuilder.DropColumn(
                name: "TemplateGenrationTypeId",
                table: "TemplateDocuments");

            migrationBuilder.DropColumn(
                name: "ServiceRequestFieldsValueId",
                table: "FieldValueTransactionsLog");

            migrationBuilder.DropColumn(
                name: "EvaluationRequestId",
                table: "Attachment");

            migrationBuilder.DropColumn(
                name: "ServiceRequestId",
                table: "ActionTransactionsLog");

            migrationBuilder.DropColumn(
                name: "IsCurrent",
                table: "AcademicYears");

            migrationBuilder.RenameTable(
                name: "DepartmentHolidays",
                newName: "DepartmentHoliday");

            migrationBuilder.RenameIndex(
                name: "IX_DepartmentHolidays_UpdateById",
                table: "DepartmentHoliday",
                newName: "IX_DepartmentHoliday_UpdateById");

            migrationBuilder.RenameIndex(
                name: "IX_DepartmentHolidays_DeleteById",
                table: "DepartmentHoliday",
                newName: "IX_DepartmentHoliday_DeleteById");

            migrationBuilder.RenameIndex(
                name: "IX_DepartmentHolidays_CreateById",
                table: "DepartmentHoliday",
                newName: "IX_DepartmentHoliday_CreateById");

            migrationBuilder.RenameIndex(
                name: "IX_DepartmentHolidays_AcademicYearId",
                table: "DepartmentHoliday",
                newName: "IX_DepartmentHoliday_AcademicYearId");

            migrationBuilder.AddColumn<bool>(
                name: "CanViewEntityEvaluation",
                table: "PartyTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "UserGenderId",
                table: "OrgTree",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ActionStepFieldId",
                table: "ActionFieldAttribute",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsUpdateOnModule",
                table: "ActionField",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "DepartmentId",
                table: "DepartmentHoliday",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_DepartmentHoliday",
                table: "DepartmentHoliday",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_OrgTree_UserGenderId",
                table: "OrgTree",
                column: "UserGenderId");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentHoliday_DepartmentId",
                table: "DepartmentHoliday",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentHoliday_AcademicYears_AcademicYearId",
                table: "DepartmentHoliday",
                column: "AcademicYearId",
                principalTable: "AcademicYears",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentHoliday_Departments_DepartmentId",
                table: "DepartmentHoliday",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentHoliday_MinistryUsers_CreateById",
                table: "DepartmentHoliday",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentHoliday_MinistryUsers_DeleteById",
                table: "DepartmentHoliday",
                column: "DeleteById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentHoliday_MinistryUsers_UpdateById",
                table: "DepartmentHoliday",
                column: "UpdateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrgTree_UserGender_UserGenderId",
                table: "OrgTree",
                column: "UserGenderId",
                principalTable: "UserGender",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
