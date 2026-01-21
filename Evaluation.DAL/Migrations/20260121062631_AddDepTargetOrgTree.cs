using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evaluation.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddDepTargetOrgTree : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AcademicYearScope_MinistryUsers_CreateById",
                table: "AcademicYearScope");

            migrationBuilder.DropForeignKey(
                name: "FK_ActionAssignPartyType_MinistryUsers_CreateById",
                table: "ActionAssignPartyType");

            migrationBuilder.DropForeignKey(
                name: "FK_ActionCondition_MinistryUsers_CreateById",
                table: "ActionCondition");

            migrationBuilder.DropForeignKey(
                name: "FK_ActionPartyType_MinistryUsers_CreateById",
                table: "ActionPartyType");

            migrationBuilder.DropForeignKey(
                name: "FK_Attribute_MinistryUsers_CreateById",
                table: "Attribute");

            migrationBuilder.DropForeignKey(
                name: "FK_Banner_MinistryUsers_CreateById",
                table: "Banner");

            migrationBuilder.DropForeignKey(
                name: "FK_ChangeRequestStatus_MinistryUsers_CreateById",
                table: "ChangeRequestStatus");

            migrationBuilder.DropForeignKey(
                name: "FK_CssApplyType_MinistryUsers_CreateById",
                table: "CssApplyType");

            migrationBuilder.DropForeignKey(
                name: "FK_CssApplyType_MinistryUsers_DeleteById",
                table: "CssApplyType");

            migrationBuilder.DropForeignKey(
                name: "FK_CssApplyType_MinistryUsers_UpdateById",
                table: "CssApplyType");

            migrationBuilder.DropForeignKey(
                name: "FK_CssClass_CssApplyType_ApplyTypeId",
                table: "CssClass");

            migrationBuilder.DropForeignKey(
                name: "FK_CssClass_MinistryUsers_CreateById",
                table: "CssClass");

            migrationBuilder.DropForeignKey(
                name: "FK_CssClass_MinistryUsers_DeleteById",
                table: "CssClass");

            migrationBuilder.DropForeignKey(
                name: "FK_CssClass_MinistryUsers_UpdateById",
                table: "CssClass");

            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentEvaluationParty_MinistryUsers_CreateById",
                table: "DepartmentEvaluationParty");

            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Category_CategoryId",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_Departments_OrgTree_TargetOrgTreeId",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_EmailLog_MinistryUsers_CreateById",
                table: "EmailLog");

            migrationBuilder.DropForeignKey(
                name: "FK_EvalRequestActionTransactionsLog_MinistryUsers_CreateById",
                table: "EvalRequestActionTransactionsLog");

            migrationBuilder.DropForeignKey(
                name: "FK_ExceptionLog_MinistryUsers_CreateById",
                table: "ExceptionLog");

            migrationBuilder.DropForeignKey(
                name: "FK_FieldValueTransactionsLog_MinistryUsers_CreateById",
                table: "FieldValueTransactionsLog");

            migrationBuilder.DropForeignKey(
                name: "FK_FieldVisabilityConfig_MinistryUsers_CreateById",
                table: "FieldVisabilityConfig");

            migrationBuilder.DropForeignKey(
                name: "FK_IntegrationPoint_MinistryUsers_CreateById",
                table: "IntegrationPoint");

            migrationBuilder.DropForeignKey(
                name: "FK_Navbar_MinistryUsers_CreateById",
                table: "Navbar");

            migrationBuilder.DropForeignKey(
                name: "FK_PlaceHolder_MinistryUsers_CreateById",
                table: "PlaceHolder");

            migrationBuilder.DropForeignKey(
                name: "FK_PlanServiceRequest_MinistryUsers_CreateById",
                table: "PlanServiceRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_RelatedFieldsGroup_MinistryUsers_CreateById",
                table: "RelatedFieldsGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_SideBar_MinistryUsers_CreateById",
                table: "SideBar");

            migrationBuilder.DropForeignKey(
                name: "FK_SiteContent_MinistryUsers_CreateById",
                table: "SiteContent");

            migrationBuilder.DropForeignKey(
                name: "FK_SiteDocument_MinistryUsers_CreateById",
                table: "SiteDocument");

            migrationBuilder.DropForeignKey(
                name: "FK_SMSLog_MinistryUsers_CreateById",
                table: "SMSLog");

            migrationBuilder.DropForeignKey(
                name: "FK_SystemSetting_MinistryUsers_CreateById",
                table: "SystemSetting");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionType_MinistryUsers_CreateById",
                table: "TransactionType");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPartyTypes_MinistryUsers_CreateById",
                table: "UserPartyTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_MinistryUsers_CreateById",
                table: "UserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTeamScope_MinistryUsers_CreateById",
                table: "UserTeamScope");

            migrationBuilder.DropForeignKey(
                name: "FK_UserToken_MinistryUsers_CreateById",
                table: "UserToken");

            migrationBuilder.DropForeignKey(
                name: "FK_UserType_MinistryUsers_CreateById",
                table: "UserType");

            migrationBuilder.DropForeignKey(
                name: "FK_VisitType_MinistryUsers_CreateById",
                table: "VisitType");

            migrationBuilder.DropIndex(
                name: "IX_Departments_CategoryId",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Departments_TargetOrgTreeId",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "TargetOrgTreeId",
                table: "Departments");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "WebsiteAttachment",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "WebsiteAttachment",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "WebsiteAttachment",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "WebGroups",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "WebGroups",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "WebGroups",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "VisitType",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "VisitType",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "VisitType",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "UserType",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "UserType",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "UserType",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "UserToken",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "UserToken",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "UserToken",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "UserTeamScope",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "UserTeamScope",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "UserTeamScope",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "UserTeams",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "UserTeams",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "UserTeams",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "UserRoles",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "UserRoles",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "UserRoles",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "UserPartyTypeSignatures",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "UserPartyTypeSignatures",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "UserPartyTypeSignatures",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "UserPartyTypes",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "UserPartyTypes",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "UserPartyTypes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "UserLoginLogs",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "UserLoginLogs",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "UserLoginLogs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "UserGender",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "UserGender",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "UserGender",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "UserDepartment",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "UserDepartment",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "UserDepartment",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "UiControls",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "UiControls",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "UiControls",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "TransactionType",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "TransactionType",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "TransactionType",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "TemplateGenrationTypies",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "TemplateGenrationTypies",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "TemplateGenrationTypies",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "TemplateDocuments",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "TemplateDocuments",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "TemplateDocuments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Team",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Team",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "Team",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "SystemSetting",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "SystemSetting",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "SystemSetting",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "SystemModuleTypes",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "SystemModuleTypes",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "SystemModuleTypes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "SystemModules",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "SystemModules",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "SystemModules",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "SystemAttributes",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "SystemAttributes",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "SystemAttributes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "SubFormItemValues",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "SubFormItemValues",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "SubFormItemValues",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "SubFormItems",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "SubFormItems",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "SubFormItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "SMSTemplates",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "SMSTemplates",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "SMSTemplates",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "SMSProfiles",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "SMSProfiles",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "SMSProfiles",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "SMSLog",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "SMSLog",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "SMSLog",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "SiteDocument",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "SiteDocument",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "SiteDocument",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "SiteContent",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "SiteContent",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "SiteContent",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "SideBar",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "SideBar",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "SideBar",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "ServiceStatusPreventPartyType",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "ServiceStatusPreventPartyType",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "ServiceStatusPreventPartyType",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "ServiceStatusPartyTypeDisplayName",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "ServiceStatusPartyTypeDisplayName",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "ServiceStatusPartyTypeDisplayName",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "ServiceStatus",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "ServiceStatus",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "ServiceStatus",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Services",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Services",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "Services",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "ServiceRequestShowPartyType",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "ServiceRequestShowPartyType",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "ServiceRequestShowPartyType",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "ServiceRequests",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "ServiceRequests",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "ServiceRequests",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "ServiceRequestFieldsValue",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "ServiceRequestFieldsValue",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "ServiceRequestFieldsValue",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "ServiceInitiatorPartyType",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "ServiceInitiatorPartyType",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "ServiceInitiatorPartyType",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "ServiceAction",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "ServiceAction",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "ServiceAction",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Semesters",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Semesters",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "Semesters",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "ScopeTypes",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "ScopeTypes",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "ScopeTypes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Scopes",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Scopes",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "Scopes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "ScopeAcademicYears",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "ScopeAcademicYears",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "ScopeAcademicYears",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "SchoolTypes",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "SchoolTypes",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "SchoolTypes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "SchoolLevel",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "SchoolLevel",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "SchoolLevel",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Roles",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Roles",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "Roles",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "RolePermissions",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "RolePermissions",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "RolePermissions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "RequestAssignmentScopes",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "RequestAssignmentScopes",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "RequestAssignmentScopes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "RequestAssignment",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "RequestAssignment",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "RequestAssignment",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "RelatedFieldsGroup",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "RelatedFieldsGroup",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "RelatedFieldsGroup",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "PlanTypeDep",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "PlanTypeDep",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "PlanTypeDep",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "PlanType",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "PlanType",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "PlanType",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "PlanStatuses",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "PlanStatuses",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "PlanStatuses",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "PlanServiceRequest",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "PlanServiceRequest",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "PlanServiceRequest",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Plans",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Plans",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "Plans",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "PlanHistory",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "PlanHistory",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "PlanHistory",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "PlaceHolder",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "PlaceHolder",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "PlaceHolder",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Permissions",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Permissions",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "Permissions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "PartyTypes",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "PartyTypes",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "PartyTypes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Pages",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Pages",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "Pages",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "PagePermissions",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "PagePermissions",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "PagePermissions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "OrgTypes",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "OrgTypes",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "OrgTypes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "OrgTree",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "OrgTree",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "OrgTree",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "OrgEvalResults",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "OrgEvalResults",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "OrgEvalResults",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "OrgClass",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "OrgClass",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "OrgClass",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "OrgAcademicYears",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "OrgAcademicYears",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "OrgAcademicYears",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "NotificationTemplates",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "NotificationTemplates",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "NotificationTemplates",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Notifications",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Notifications",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "Notifications",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "NdaStatusDepartments",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "NdaStatusDepartments",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "NdaStatusDepartments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "NdaStatus",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "NdaStatus",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "NdaStatus",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Navbar",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Navbar",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "Navbar",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MinistryUsers",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "MinistryUsers",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "MinistryUsers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "JobTitle",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "JobTitle",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "JobTitle",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "IntegrationPoint",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "IntegrationPoint",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "IntegrationPoint",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "FormStatus",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "FormStatus",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "FormStatus",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "FormScopes",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "FormScopes",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "FormScopes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "FormItemValues",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "FormItemValues",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "FormItemValues",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "FormItemRelated",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "FormItemRelated",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "FormItemRelated",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "FormItem",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "FormItem",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "FormItem",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "FormGroupType",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "FormGroupType",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "FormGroupType",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "FormGroupCustomList",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "FormGroupCustomList",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "FormGroupCustomList",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "FormGroup",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "FormGroup",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "FormGroup",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "FormEvalMatrixValue",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "FormEvalMatrixValue",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "FormEvalMatrixValue",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "FormEvalMatrix",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "FormEvalMatrix",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "FormEvalMatrix",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "FieldVisabilityConfig",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "FieldVisabilityConfig",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "FieldVisabilityConfig",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "FieldViewCondition",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "FieldViewCondition",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "FieldViewCondition",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "FieldValueTransactionsLog",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "FieldValueTransactionsLog",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "FieldValueTransactionsLog",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "FieldTypeShow",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "FieldTypeShow",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "FieldTypeShow",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "FieldType",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "FieldType",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "FieldType",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "FieldPartyType",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "FieldPartyType",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "FieldPartyType",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "FieldDropDownValue",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "FieldDropDownValue",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "FieldDropDownValue",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "FieldAttributeValue",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "FieldAttributeValue",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "FieldAttributeValue",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Field",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Field",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "Field",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "ExceptionLog",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "ExceptionLog",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "ExceptionLog",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "EvaluationType",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "EvaluationType",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "EvaluationType",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "EvaluationRequestHistoryFieldsValue",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "EvaluationRequestHistoryFieldsValue",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "EvaluationRequestHistoryFieldsValue",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "EvaluationRequestHistory",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "EvaluationRequestHistory",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "EvaluationRequestHistory",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "EvaluationRequestFieldsValue",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "EvaluationRequestFieldsValue",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "EvaluationRequestFieldsValue",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "EvaluationRequestAssignment",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "EvaluationRequestAssignment",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "EvaluationRequestAssignment",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "EvaluationRequest",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "EvaluationRequest",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "EvaluationRequest",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "EvaluationParties",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "EvaluationParties",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "EvaluationParties",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "EvalRequestAssignmentScope",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "EvalRequestAssignmentScope",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "EvalRequestAssignmentScope",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "EvalRequestActionTransactionsLog",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "EvalRequestActionTransactionsLog",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "EvalRequestActionTransactionsLog",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "EvalFormType",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "EvalFormType",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "EvalFormType",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "EvalForms",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "EvalForms",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "EvalForms",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "EvalAttachments",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "EvalAttachments",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "EvalAttachments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "EmailTemplates",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "EmailTemplates",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "EmailTemplates",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "EmailTemplateDocuments",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "EmailTemplateDocuments",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "EmailTemplateDocuments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "EmailProfile",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "EmailProfile",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "EmailProfile",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "EmailLog",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "EmailLog",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "EmailLog",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "EducationLevel",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "EducationLevel",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "EducationLevel",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "DropDownType",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "DropDownType",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "DropDownType",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "DepWebGroup",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "DepWebGroup",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "DepWebGroup",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "DepEvaluationType",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "DepEvaluationType",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "DepEvaluationType",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "DepEvalMatrixs",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "DepEvalMatrixs",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "DepEvalMatrixs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Departments",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Departments",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "Departments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "DepartmentRoleAttributeValue",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "DepartmentRoleAttributeValue",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "DepartmentRoleAttributeValue",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "DepartmentOrgTrees",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "DepartmentOrgTrees",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "DepartmentOrgTrees",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "DepartmentHolidays",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "DepartmentHolidays",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "DepartmentHolidays",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "DepartmentEvaluationParty",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "DepartmentEvaluationParty",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "DepartmentEvaluationParty",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "CssClass",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "CssClass",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "CssClass",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "CssApplyType",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "CssApplyType",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "CssApplyType",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Countries",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Countries",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "Countries",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "ControlValidations",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "ControlValidations",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "ControlValidations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "ChangeRequestStatus",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "ChangeRequestStatus",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "ChangeRequestStatus",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Category",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Category",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "Category",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "CalcMethods",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "CalcMethods",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "CalcMethods",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Banner",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Banner",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "Banner",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "AuditLogs",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "AuditLogs",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "AuditLogs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Attribute",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Attribute",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "Attribute",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "ActionType",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "ActionType",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "ActionType",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "ActionTransactionsLog",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "ActionTransactionsLog",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "ActionTransactionsLog",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "ActionTemplateDoc",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "ActionTemplateDoc",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "ActionTemplateDoc",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "ActionStatusConfiguration",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "ActionStatusConfiguration",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "ActionStatusConfiguration",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "ActionStatusConfigNotification",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "ActionStatusConfigNotification",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "ActionStatusConfigNotification",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "ActionShowLogPartyType",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "ActionShowLogPartyType",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "ActionShowLogPartyType",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "ActionPartyType",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "ActionPartyType",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "ActionPartyType",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "ActionFieldAttribute",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "ActionFieldAttribute",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "ActionFieldAttribute",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "ActionField",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "ActionField",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "ActionField",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "ActionCondition",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "ActionCondition",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "ActionCondition",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "ActionAssignPartyType",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "ActionAssignPartyType",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "ActionAssignPartyType",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "AcademicYearScope",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "AcademicYearScope",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "AcademicYearScope",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "AcademicYears",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "0");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "AcademicYears",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "AcademicYears",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "DepTargetOrgTree",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TargetOrgTreeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PredicateFuncion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DepTargetConfig = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_DepTargetOrgTree", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DepTargetOrgTree_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DepTargetOrgTree_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DepTargetOrgTree_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DepTargetOrgTree_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DepTargetOrgTree_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DepTargetOrgTree_OrgTree_TargetOrgTreeId",
                        column: x => x.TargetOrgTreeId,
                        principalTable: "OrgTree",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DepTargetOrgTree_CategoryId",
                table: "DepTargetOrgTree",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_DepTargetOrgTree_CreateById",
                table: "DepTargetOrgTree",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_DepTargetOrgTree_DeleteById",
                table: "DepTargetOrgTree",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_DepTargetOrgTree_DepartmentId",
                table: "DepTargetOrgTree",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_DepTargetOrgTree_TargetOrgTreeId",
                table: "DepTargetOrgTree",
                column: "TargetOrgTreeId");

            migrationBuilder.CreateIndex(
                name: "IX_DepTargetOrgTree_UpdateById",
                table: "DepTargetOrgTree",
                column: "UpdateById");

            migrationBuilder.AddForeignKey(
                name: "FK_AcademicYearScope_MinistryUsers_CreateById",
                table: "AcademicYearScope",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ActionAssignPartyType_MinistryUsers_CreateById",
                table: "ActionAssignPartyType",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ActionCondition_MinistryUsers_CreateById",
                table: "ActionCondition",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ActionPartyType_MinistryUsers_CreateById",
                table: "ActionPartyType",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Attribute_MinistryUsers_CreateById",
                table: "Attribute",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Banner_MinistryUsers_CreateById",
                table: "Banner",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChangeRequestStatus_MinistryUsers_CreateById",
                table: "ChangeRequestStatus",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CssApplyType_MinistryUsers_CreateById",
                table: "CssApplyType",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CssApplyType_MinistryUsers_DeleteById",
                table: "CssApplyType",
                column: "DeleteById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CssApplyType_MinistryUsers_UpdateById",
                table: "CssApplyType",
                column: "UpdateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CssClass_CssApplyType_ApplyTypeId",
                table: "CssClass",
                column: "ApplyTypeId",
                principalTable: "CssApplyType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CssClass_MinistryUsers_CreateById",
                table: "CssClass",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CssClass_MinistryUsers_DeleteById",
                table: "CssClass",
                column: "DeleteById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CssClass_MinistryUsers_UpdateById",
                table: "CssClass",
                column: "UpdateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentEvaluationParty_MinistryUsers_CreateById",
                table: "DepartmentEvaluationParty",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmailLog_MinistryUsers_CreateById",
                table: "EmailLog",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EvalRequestActionTransactionsLog_MinistryUsers_CreateById",
                table: "EvalRequestActionTransactionsLog",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExceptionLog_MinistryUsers_CreateById",
                table: "ExceptionLog",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FieldValueTransactionsLog_MinistryUsers_CreateById",
                table: "FieldValueTransactionsLog",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FieldVisabilityConfig_MinistryUsers_CreateById",
                table: "FieldVisabilityConfig",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IntegrationPoint_MinistryUsers_CreateById",
                table: "IntegrationPoint",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Navbar_MinistryUsers_CreateById",
                table: "Navbar",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlaceHolder_MinistryUsers_CreateById",
                table: "PlaceHolder",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlanServiceRequest_MinistryUsers_CreateById",
                table: "PlanServiceRequest",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RelatedFieldsGroup_MinistryUsers_CreateById",
                table: "RelatedFieldsGroup",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SideBar_MinistryUsers_CreateById",
                table: "SideBar",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SiteContent_MinistryUsers_CreateById",
                table: "SiteContent",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SiteDocument_MinistryUsers_CreateById",
                table: "SiteDocument",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SMSLog_MinistryUsers_CreateById",
                table: "SMSLog",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SystemSetting_MinistryUsers_CreateById",
                table: "SystemSetting",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionType_MinistryUsers_CreateById",
                table: "TransactionType",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPartyTypes_MinistryUsers_CreateById",
                table: "UserPartyTypes",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_MinistryUsers_CreateById",
                table: "UserRoles",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTeamScope_MinistryUsers_CreateById",
                table: "UserTeamScope",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserToken_MinistryUsers_CreateById",
                table: "UserToken",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserType_MinistryUsers_CreateById",
                table: "UserType",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VisitType_MinistryUsers_CreateById",
                table: "VisitType",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AcademicYearScope_MinistryUsers_CreateById",
                table: "AcademicYearScope");

            migrationBuilder.DropForeignKey(
                name: "FK_ActionAssignPartyType_MinistryUsers_CreateById",
                table: "ActionAssignPartyType");

            migrationBuilder.DropForeignKey(
                name: "FK_ActionCondition_MinistryUsers_CreateById",
                table: "ActionCondition");

            migrationBuilder.DropForeignKey(
                name: "FK_ActionPartyType_MinistryUsers_CreateById",
                table: "ActionPartyType");

            migrationBuilder.DropForeignKey(
                name: "FK_Attribute_MinistryUsers_CreateById",
                table: "Attribute");

            migrationBuilder.DropForeignKey(
                name: "FK_Banner_MinistryUsers_CreateById",
                table: "Banner");

            migrationBuilder.DropForeignKey(
                name: "FK_ChangeRequestStatus_MinistryUsers_CreateById",
                table: "ChangeRequestStatus");

            migrationBuilder.DropForeignKey(
                name: "FK_CssApplyType_MinistryUsers_CreateById",
                table: "CssApplyType");

            migrationBuilder.DropForeignKey(
                name: "FK_CssApplyType_MinistryUsers_DeleteById",
                table: "CssApplyType");

            migrationBuilder.DropForeignKey(
                name: "FK_CssApplyType_MinistryUsers_UpdateById",
                table: "CssApplyType");

            migrationBuilder.DropForeignKey(
                name: "FK_CssClass_CssApplyType_ApplyTypeId",
                table: "CssClass");

            migrationBuilder.DropForeignKey(
                name: "FK_CssClass_MinistryUsers_CreateById",
                table: "CssClass");

            migrationBuilder.DropForeignKey(
                name: "FK_CssClass_MinistryUsers_DeleteById",
                table: "CssClass");

            migrationBuilder.DropForeignKey(
                name: "FK_CssClass_MinistryUsers_UpdateById",
                table: "CssClass");

            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentEvaluationParty_MinistryUsers_CreateById",
                table: "DepartmentEvaluationParty");

            migrationBuilder.DropForeignKey(
                name: "FK_EmailLog_MinistryUsers_CreateById",
                table: "EmailLog");

            migrationBuilder.DropForeignKey(
                name: "FK_EvalRequestActionTransactionsLog_MinistryUsers_CreateById",
                table: "EvalRequestActionTransactionsLog");

            migrationBuilder.DropForeignKey(
                name: "FK_ExceptionLog_MinistryUsers_CreateById",
                table: "ExceptionLog");

            migrationBuilder.DropForeignKey(
                name: "FK_FieldValueTransactionsLog_MinistryUsers_CreateById",
                table: "FieldValueTransactionsLog");

            migrationBuilder.DropForeignKey(
                name: "FK_FieldVisabilityConfig_MinistryUsers_CreateById",
                table: "FieldVisabilityConfig");

            migrationBuilder.DropForeignKey(
                name: "FK_IntegrationPoint_MinistryUsers_CreateById",
                table: "IntegrationPoint");

            migrationBuilder.DropForeignKey(
                name: "FK_Navbar_MinistryUsers_CreateById",
                table: "Navbar");

            migrationBuilder.DropForeignKey(
                name: "FK_PlaceHolder_MinistryUsers_CreateById",
                table: "PlaceHolder");

            migrationBuilder.DropForeignKey(
                name: "FK_PlanServiceRequest_MinistryUsers_CreateById",
                table: "PlanServiceRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_RelatedFieldsGroup_MinistryUsers_CreateById",
                table: "RelatedFieldsGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_SideBar_MinistryUsers_CreateById",
                table: "SideBar");

            migrationBuilder.DropForeignKey(
                name: "FK_SiteContent_MinistryUsers_CreateById",
                table: "SiteContent");

            migrationBuilder.DropForeignKey(
                name: "FK_SiteDocument_MinistryUsers_CreateById",
                table: "SiteDocument");

            migrationBuilder.DropForeignKey(
                name: "FK_SMSLog_MinistryUsers_CreateById",
                table: "SMSLog");

            migrationBuilder.DropForeignKey(
                name: "FK_SystemSetting_MinistryUsers_CreateById",
                table: "SystemSetting");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionType_MinistryUsers_CreateById",
                table: "TransactionType");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPartyTypes_MinistryUsers_CreateById",
                table: "UserPartyTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_MinistryUsers_CreateById",
                table: "UserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTeamScope_MinistryUsers_CreateById",
                table: "UserTeamScope");

            migrationBuilder.DropForeignKey(
                name: "FK_UserToken_MinistryUsers_CreateById",
                table: "UserToken");

            migrationBuilder.DropForeignKey(
                name: "FK_UserType_MinistryUsers_CreateById",
                table: "UserType");

            migrationBuilder.DropForeignKey(
                name: "FK_VisitType_MinistryUsers_CreateById",
                table: "VisitType");

            migrationBuilder.DropTable(
                name: "DepTargetOrgTree");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "WebsiteAttachment",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "WebsiteAttachment",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "WebsiteAttachment",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "WebGroups",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "WebGroups",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "WebGroups",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "VisitType",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "VisitType",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "VisitType",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "UserType",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "UserType",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "UserType",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "UserToken",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "UserToken",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "UserToken",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "UserTeamScope",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "UserTeamScope",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "UserTeamScope",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "UserTeams",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "UserTeams",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "UserTeams",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "UserRoles",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "UserRoles",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "UserRoles",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "UserPartyTypeSignatures",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "UserPartyTypeSignatures",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "UserPartyTypeSignatures",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "UserPartyTypes",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "UserPartyTypes",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "UserPartyTypes",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "UserLoginLogs",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "UserLoginLogs",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "UserLoginLogs",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "UserGender",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "UserGender",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "UserGender",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "UserDepartment",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "UserDepartment",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "UserDepartment",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "UiControls",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "UiControls",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "UiControls",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "TransactionType",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "TransactionType",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "TransactionType",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "TemplateGenrationTypies",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "TemplateGenrationTypies",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "TemplateGenrationTypies",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "TemplateDocuments",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "TemplateDocuments",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "TemplateDocuments",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Team",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Team",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "Team",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "SystemSetting",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "SystemSetting",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "SystemSetting",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "SystemModuleTypes",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "SystemModuleTypes",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "SystemModuleTypes",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "SystemModules",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "SystemModules",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "SystemModules",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "SystemAttributes",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "SystemAttributes",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "SystemAttributes",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "SubFormItemValues",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "SubFormItemValues",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "SubFormItemValues",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "SubFormItems",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "SubFormItems",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "SubFormItems",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "SMSTemplates",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "SMSTemplates",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "SMSTemplates",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "SMSProfiles",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "SMSProfiles",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "SMSProfiles",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "SMSLog",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "SMSLog",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "SMSLog",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "SiteDocument",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "SiteDocument",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "SiteDocument",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "SiteContent",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "SiteContent",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "SiteContent",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "SideBar",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "SideBar",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "SideBar",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "ServiceStatusPreventPartyType",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "ServiceStatusPreventPartyType",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "ServiceStatusPreventPartyType",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "ServiceStatusPartyTypeDisplayName",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "ServiceStatusPartyTypeDisplayName",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "ServiceStatusPartyTypeDisplayName",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "ServiceStatus",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "ServiceStatus",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "ServiceStatus",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Services",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Services",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "Services",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "ServiceRequestShowPartyType",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "ServiceRequestShowPartyType",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "ServiceRequestShowPartyType",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "ServiceRequests",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "ServiceRequests",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "ServiceRequests",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "ServiceRequestFieldsValue",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "ServiceRequestFieldsValue",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "ServiceRequestFieldsValue",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "ServiceInitiatorPartyType",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "ServiceInitiatorPartyType",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "ServiceInitiatorPartyType",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "ServiceAction",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "ServiceAction",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "ServiceAction",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Semesters",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Semesters",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "Semesters",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "ScopeTypes",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "ScopeTypes",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "ScopeTypes",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Scopes",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Scopes",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "Scopes",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "ScopeAcademicYears",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "ScopeAcademicYears",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "ScopeAcademicYears",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "SchoolTypes",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "SchoolTypes",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "SchoolTypes",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "SchoolLevel",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "SchoolLevel",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "SchoolLevel",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Roles",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Roles",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "Roles",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "RolePermissions",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "RolePermissions",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "RolePermissions",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "RequestAssignmentScopes",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "RequestAssignmentScopes",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "RequestAssignmentScopes",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "RequestAssignment",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "RequestAssignment",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "RequestAssignment",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "RelatedFieldsGroup",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "RelatedFieldsGroup",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "RelatedFieldsGroup",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "PlanTypeDep",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "PlanTypeDep",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "PlanTypeDep",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "PlanType",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "PlanType",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "PlanType",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "PlanStatuses",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "PlanStatuses",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "PlanStatuses",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "PlanServiceRequest",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "PlanServiceRequest",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "PlanServiceRequest",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Plans",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Plans",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "Plans",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "PlanHistory",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "PlanHistory",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "PlanHistory",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "PlaceHolder",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "PlaceHolder",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "PlaceHolder",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Permissions",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Permissions",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "Permissions",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "PartyTypes",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "PartyTypes",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "PartyTypes",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Pages",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Pages",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "Pages",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "PagePermissions",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "PagePermissions",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "PagePermissions",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "OrgTypes",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "OrgTypes",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "OrgTypes",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "OrgTree",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "OrgTree",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "OrgTree",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "OrgEvalResults",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "OrgEvalResults",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "OrgEvalResults",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "OrgClass",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "OrgClass",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "OrgClass",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "OrgAcademicYears",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "OrgAcademicYears",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "OrgAcademicYears",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "NotificationTemplates",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "NotificationTemplates",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "NotificationTemplates",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Notifications",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Notifications",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "Notifications",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "NdaStatusDepartments",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "NdaStatusDepartments",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "NdaStatusDepartments",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "NdaStatus",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "NdaStatus",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "NdaStatus",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Navbar",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Navbar",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "Navbar",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MinistryUsers",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "MinistryUsers",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "MinistryUsers",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "JobTitle",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "JobTitle",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "JobTitle",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "IntegrationPoint",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "IntegrationPoint",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "IntegrationPoint",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "FormStatus",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "FormStatus",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "FormStatus",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "FormScopes",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "FormScopes",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "FormScopes",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "FormItemValues",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "FormItemValues",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "FormItemValues",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "FormItemRelated",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "FormItemRelated",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "FormItemRelated",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "FormItem",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "FormItem",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "FormItem",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "FormGroupType",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "FormGroupType",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "FormGroupType",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "FormGroupCustomList",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "FormGroupCustomList",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "FormGroupCustomList",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "FormGroup",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "FormGroup",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "FormGroup",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "FormEvalMatrixValue",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "FormEvalMatrixValue",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "FormEvalMatrixValue",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "FormEvalMatrix",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "FormEvalMatrix",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "FormEvalMatrix",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "FieldVisabilityConfig",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "FieldVisabilityConfig",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "FieldVisabilityConfig",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "FieldViewCondition",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "FieldViewCondition",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "FieldViewCondition",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "FieldValueTransactionsLog",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "FieldValueTransactionsLog",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "FieldValueTransactionsLog",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "FieldTypeShow",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "FieldTypeShow",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "FieldTypeShow",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "FieldType",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "FieldType",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "FieldType",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "FieldPartyType",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "FieldPartyType",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "FieldPartyType",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "FieldDropDownValue",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "FieldDropDownValue",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "FieldDropDownValue",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "FieldAttributeValue",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "FieldAttributeValue",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "FieldAttributeValue",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Field",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Field",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "Field",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "ExceptionLog",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "ExceptionLog",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "ExceptionLog",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "EvaluationType",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "EvaluationType",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "EvaluationType",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "EvaluationRequestHistoryFieldsValue",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "EvaluationRequestHistoryFieldsValue",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "EvaluationRequestHistoryFieldsValue",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "EvaluationRequestHistory",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "EvaluationRequestHistory",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "EvaluationRequestHistory",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "EvaluationRequestFieldsValue",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "EvaluationRequestFieldsValue",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "EvaluationRequestFieldsValue",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "EvaluationRequestAssignment",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "EvaluationRequestAssignment",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "EvaluationRequestAssignment",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "EvaluationRequest",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "EvaluationRequest",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "EvaluationRequest",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "EvaluationParties",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "EvaluationParties",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "EvaluationParties",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "EvalRequestAssignmentScope",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "EvalRequestAssignmentScope",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "EvalRequestAssignmentScope",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "EvalRequestActionTransactionsLog",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "EvalRequestActionTransactionsLog",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "EvalRequestActionTransactionsLog",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "EvalFormType",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "EvalFormType",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "EvalFormType",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "EvalForms",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "EvalForms",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "EvalForms",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "EvalAttachments",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "EvalAttachments",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "EvalAttachments",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "EmailTemplates",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "EmailTemplates",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "EmailTemplates",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "EmailTemplateDocuments",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "EmailTemplateDocuments",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "EmailTemplateDocuments",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "EmailProfile",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "EmailProfile",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "EmailProfile",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "EmailLog",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "EmailLog",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "EmailLog",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "EducationLevel",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "EducationLevel",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "EducationLevel",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "DropDownType",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "DropDownType",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "DropDownType",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "DepWebGroup",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "DepWebGroup",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "DepWebGroup",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "DepEvaluationType",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "DepEvaluationType",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "DepEvaluationType",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "DepEvalMatrixs",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "DepEvalMatrixs",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "DepEvalMatrixs",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Departments",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Departments",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "Departments",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "CategoryId",
                table: "Departments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TargetOrgTreeId",
                table: "Departments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "DepartmentRoleAttributeValue",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "DepartmentRoleAttributeValue",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "DepartmentRoleAttributeValue",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "DepartmentOrgTrees",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "DepartmentOrgTrees",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "DepartmentOrgTrees",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "DepartmentHolidays",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "DepartmentHolidays",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "DepartmentHolidays",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "DepartmentEvaluationParty",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "DepartmentEvaluationParty",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "DepartmentEvaluationParty",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "CssClass",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "CssClass",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "CssClass",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "CssApplyType",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "CssApplyType",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "CssApplyType",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Countries",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Countries",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "Countries",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "ControlValidations",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "ControlValidations",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "ControlValidations",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "ChangeRequestStatus",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "ChangeRequestStatus",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "ChangeRequestStatus",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Category",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Category",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "Category",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "CalcMethods",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "CalcMethods",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "CalcMethods",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Banner",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Banner",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "Banner",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "AuditLogs",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "AuditLogs",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "AuditLogs",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Attribute",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Attribute",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "Attribute",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "ActionType",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "ActionType",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "ActionType",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "ActionTransactionsLog",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "ActionTransactionsLog",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "ActionTransactionsLog",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "ActionTemplateDoc",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "ActionTemplateDoc",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "ActionTemplateDoc",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "ActionStatusConfiguration",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "ActionStatusConfiguration",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "ActionStatusConfiguration",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "ActionStatusConfigNotification",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "ActionStatusConfigNotification",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "ActionStatusConfigNotification",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "ActionShowLogPartyType",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "ActionShowLogPartyType",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "ActionShowLogPartyType",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "ActionPartyType",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "ActionPartyType",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "ActionPartyType",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "ActionFieldAttribute",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "ActionFieldAttribute",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "ActionFieldAttribute",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "ActionField",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "ActionField",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "ActionField",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "ActionCondition",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "ActionCondition",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "ActionCondition",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "ActionAssignPartyType",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "ActionAssignPartyType",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "ActionAssignPartyType",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "AcademicYearScope",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "AcademicYearScope",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "AcademicYearScope",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "AcademicYears",
                type: "bit",
                nullable: true,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "AcademicYears",
                type: "bit",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateById",
                table: "AcademicYears",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_CategoryId",
                table: "Departments",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_TargetOrgTreeId",
                table: "Departments",
                column: "TargetOrgTreeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AcademicYearScope_MinistryUsers_CreateById",
                table: "AcademicYearScope",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ActionAssignPartyType_MinistryUsers_CreateById",
                table: "ActionAssignPartyType",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ActionCondition_MinistryUsers_CreateById",
                table: "ActionCondition",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ActionPartyType_MinistryUsers_CreateById",
                table: "ActionPartyType",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Attribute_MinistryUsers_CreateById",
                table: "Attribute",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Banner_MinistryUsers_CreateById",
                table: "Banner",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChangeRequestStatus_MinistryUsers_CreateById",
                table: "ChangeRequestStatus",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CssApplyType_MinistryUsers_CreateById",
                table: "CssApplyType",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CssApplyType_MinistryUsers_DeleteById",
                table: "CssApplyType",
                column: "DeleteById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CssApplyType_MinistryUsers_UpdateById",
                table: "CssApplyType",
                column: "UpdateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CssClass_CssApplyType_ApplyTypeId",
                table: "CssClass",
                column: "ApplyTypeId",
                principalTable: "CssApplyType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CssClass_MinistryUsers_CreateById",
                table: "CssClass",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CssClass_MinistryUsers_DeleteById",
                table: "CssClass",
                column: "DeleteById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CssClass_MinistryUsers_UpdateById",
                table: "CssClass",
                column: "UpdateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentEvaluationParty_MinistryUsers_CreateById",
                table: "DepartmentEvaluationParty",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Category_CategoryId",
                table: "Departments",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_OrgTree_TargetOrgTreeId",
                table: "Departments",
                column: "TargetOrgTreeId",
                principalTable: "OrgTree",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EmailLog_MinistryUsers_CreateById",
                table: "EmailLog",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EvalRequestActionTransactionsLog_MinistryUsers_CreateById",
                table: "EvalRequestActionTransactionsLog",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExceptionLog_MinistryUsers_CreateById",
                table: "ExceptionLog",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FieldValueTransactionsLog_MinistryUsers_CreateById",
                table: "FieldValueTransactionsLog",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FieldVisabilityConfig_MinistryUsers_CreateById",
                table: "FieldVisabilityConfig",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_IntegrationPoint_MinistryUsers_CreateById",
                table: "IntegrationPoint",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Navbar_MinistryUsers_CreateById",
                table: "Navbar",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlaceHolder_MinistryUsers_CreateById",
                table: "PlaceHolder",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlanServiceRequest_MinistryUsers_CreateById",
                table: "PlanServiceRequest",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RelatedFieldsGroup_MinistryUsers_CreateById",
                table: "RelatedFieldsGroup",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SideBar_MinistryUsers_CreateById",
                table: "SideBar",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SiteContent_MinistryUsers_CreateById",
                table: "SiteContent",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SiteDocument_MinistryUsers_CreateById",
                table: "SiteDocument",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SMSLog_MinistryUsers_CreateById",
                table: "SMSLog",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SystemSetting_MinistryUsers_CreateById",
                table: "SystemSetting",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionType_MinistryUsers_CreateById",
                table: "TransactionType",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPartyTypes_MinistryUsers_CreateById",
                table: "UserPartyTypes",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_MinistryUsers_CreateById",
                table: "UserRoles",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserTeamScope_MinistryUsers_CreateById",
                table: "UserTeamScope",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserToken_MinistryUsers_CreateById",
                table: "UserToken",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserType_MinistryUsers_CreateById",
                table: "UserType",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VisitType_MinistryUsers_CreateById",
                table: "VisitType",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");
        }
    }
}
