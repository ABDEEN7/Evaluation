using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evaluation.DAL.Migrations
{
    /// <inheritdoc />
    public partial class EnhanceEvaluationRequestAndActionLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentRoleAttributeValue_DepartmentRoleAttribute_DepartmentRoleAttributeId",
                table: "DepartmentRoleAttributeValue");

            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentRoleAttributeValue_MinistryUsers_CreateById",
                table: "DepartmentRoleAttributeValue");

            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentRoleAttributeValue_MinistryUsers_DeleteById",
                table: "DepartmentRoleAttributeValue");

            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentRoleAttributeValue_MinistryUsers_UpdateById",
                table: "DepartmentRoleAttributeValue");

            migrationBuilder.DropTable(
                name: "DepartmentRoleAttribute");

            migrationBuilder.DropColumn(
                name: "OrderNo",
                table: "FormItem");

            migrationBuilder.RenameColumn(
                name: "DepartmentRoleAttributeId",
                table: "DepartmentRoleAttributeValue",
                newName: "SystemAttributeId");

            migrationBuilder.RenameIndex(
                name: "IX_DepartmentRoleAttributeValue_DepartmentRoleAttributeId",
                table: "DepartmentRoleAttributeValue",
                newName: "IX_DepartmentRoleAttributeValue_SystemAttributeId");

            migrationBuilder.RenameColumn(
                name: "MyProperty",
                table: "DepartmentEvaluationParty",
                newName: "DepartmentId");

            migrationBuilder.AddColumn<Guid>(
                name: "DepartmentId",
                table: "SystemSetting",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "OrgTreeId",
                table: "ServiceRequests",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<string>(
                name: "RequestNumber",
                table: "EvaluationRequest",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Sequence",
                table: "EvaluationRequest",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<Guid>(
                name: "EvalRequestActionTransactionsLogId",
                table: "EvalAttachments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "DepartmentHolidays",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "CronExpression",
                table: "DepartmentHolidays",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "OrderNo",
                table: "DepartmentEvaluationParty",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "EvalRequestActionTransactionsLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceActionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EvaluationRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RefId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SystemModuleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PreviousStatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NextStatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_EvalRequestActionTransactionsLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EvalRequestActionTransactionsLog_EvaluationRequest_EvaluationRequestId",
                        column: x => x.EvaluationRequestId,
                        principalTable: "EvaluationRequest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EvalRequestActionTransactionsLog_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EvalRequestActionTransactionsLog_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EvalRequestActionTransactionsLog_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EvalRequestActionTransactionsLog_ServiceAction_ServiceActionId",
                        column: x => x.ServiceActionId,
                        principalTable: "ServiceAction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EvalRequestActionTransactionsLog_SystemModules_SystemModuleId",
                        column: x => x.SystemModuleId,
                        principalTable: "SystemModules",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SystemAttributes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BackendName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    table.PrimaryKey("PK_SystemAttributes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SystemAttributes_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SystemAttributes_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SystemAttributes_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SystemSetting_DepartmentId",
                table: "SystemSetting",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_EvalAttachments_EvalRequestActionTransactionsLogId",
                table: "EvalAttachments",
                column: "EvalRequestActionTransactionsLogId");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentRoleAttributeValue_DepartmentId",
                table: "DepartmentRoleAttributeValue",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentEvaluationParty_DepartmentId",
                table: "DepartmentEvaluationParty",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_EvalRequestActionTransactionsLog_CreateById",
                table: "EvalRequestActionTransactionsLog",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_EvalRequestActionTransactionsLog_DeleteById",
                table: "EvalRequestActionTransactionsLog",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_EvalRequestActionTransactionsLog_EvaluationRequestId",
                table: "EvalRequestActionTransactionsLog",
                column: "EvaluationRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_EvalRequestActionTransactionsLog_ServiceActionId",
                table: "EvalRequestActionTransactionsLog",
                column: "ServiceActionId");

            migrationBuilder.CreateIndex(
                name: "IX_EvalRequestActionTransactionsLog_SystemModuleId",
                table: "EvalRequestActionTransactionsLog",
                column: "SystemModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_EvalRequestActionTransactionsLog_UpdateById",
                table: "EvalRequestActionTransactionsLog",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_SystemAttributes_CreateById",
                table: "SystemAttributes",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_SystemAttributes_DeleteById",
                table: "SystemAttributes",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_SystemAttributes_UpdateById",
                table: "SystemAttributes",
                column: "UpdateById");

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentEvaluationParty_Departments_DepartmentId",
                table: "DepartmentEvaluationParty",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentRoleAttributeValue_Departments_DepartmentId",
                table: "DepartmentRoleAttributeValue",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentRoleAttributeValue_MinistryUsers_CreateById",
                table: "DepartmentRoleAttributeValue",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentRoleAttributeValue_MinistryUsers_DeleteById",
                table: "DepartmentRoleAttributeValue",
                column: "DeleteById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentRoleAttributeValue_MinistryUsers_UpdateById",
                table: "DepartmentRoleAttributeValue",
                column: "UpdateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentRoleAttributeValue_SystemAttributes_SystemAttributeId",
                table: "DepartmentRoleAttributeValue",
                column: "SystemAttributeId",
                principalTable: "SystemAttributes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EvalAttachments_EvalRequestActionTransactionsLog_EvalRequestActionTransactionsLogId",
                table: "EvalAttachments",
                column: "EvalRequestActionTransactionsLogId",
                principalTable: "EvalRequestActionTransactionsLog",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SystemSetting_Departments_DepartmentId",
                table: "SystemSetting",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentEvaluationParty_Departments_DepartmentId",
                table: "DepartmentEvaluationParty");

            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentRoleAttributeValue_Departments_DepartmentId",
                table: "DepartmentRoleAttributeValue");

            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentRoleAttributeValue_MinistryUsers_CreateById",
                table: "DepartmentRoleAttributeValue");

            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentRoleAttributeValue_MinistryUsers_DeleteById",
                table: "DepartmentRoleAttributeValue");

            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentRoleAttributeValue_MinistryUsers_UpdateById",
                table: "DepartmentRoleAttributeValue");

            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentRoleAttributeValue_SystemAttributes_SystemAttributeId",
                table: "DepartmentRoleAttributeValue");

            migrationBuilder.DropForeignKey(
                name: "FK_EvalAttachments_EvalRequestActionTransactionsLog_EvalRequestActionTransactionsLogId",
                table: "EvalAttachments");

            migrationBuilder.DropForeignKey(
                name: "FK_SystemSetting_Departments_DepartmentId",
                table: "SystemSetting");

            migrationBuilder.DropTable(
                name: "EvalRequestActionTransactionsLog");

            migrationBuilder.DropTable(
                name: "SystemAttributes");

            migrationBuilder.DropIndex(
                name: "IX_SystemSetting_DepartmentId",
                table: "SystemSetting");

            migrationBuilder.DropIndex(
                name: "IX_EvalAttachments_EvalRequestActionTransactionsLogId",
                table: "EvalAttachments");

            migrationBuilder.DropIndex(
                name: "IX_DepartmentRoleAttributeValue_DepartmentId",
                table: "DepartmentRoleAttributeValue");

            migrationBuilder.DropIndex(
                name: "IX_DepartmentEvaluationParty_DepartmentId",
                table: "DepartmentEvaluationParty");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "SystemSetting");

            migrationBuilder.DropColumn(
                name: "RequestNumber",
                table: "EvaluationRequest");

            migrationBuilder.DropColumn(
                name: "Sequence",
                table: "EvaluationRequest");

            migrationBuilder.DropColumn(
                name: "EvalRequestActionTransactionsLogId",
                table: "EvalAttachments");

            migrationBuilder.DropColumn(
                name: "OrderNo",
                table: "DepartmentEvaluationParty");

            migrationBuilder.RenameColumn(
                name: "SystemAttributeId",
                table: "DepartmentRoleAttributeValue",
                newName: "DepartmentRoleAttributeId");

            migrationBuilder.RenameIndex(
                name: "IX_DepartmentRoleAttributeValue_SystemAttributeId",
                table: "DepartmentRoleAttributeValue",
                newName: "IX_DepartmentRoleAttributeValue_DepartmentRoleAttributeId");

            migrationBuilder.RenameColumn(
                name: "DepartmentId",
                table: "DepartmentEvaluationParty",
                newName: "MyProperty");

            migrationBuilder.AlterColumn<Guid>(
                name: "OrgTreeId",
                table: "ServiceRequests",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderNo",
                table: "FormItem",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "DepartmentHolidays",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CronExpression",
                table: "DepartmentHolidays",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "DepartmentRoleAttribute",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BackendName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepartmentRoleAttribute", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DepartmentRoleAttribute_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DepartmentRoleAttribute_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DepartmentRoleAttribute_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentRoleAttribute_CreateById",
                table: "DepartmentRoleAttribute",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentRoleAttribute_DeleteById",
                table: "DepartmentRoleAttribute",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentRoleAttribute_UpdateById",
                table: "DepartmentRoleAttribute",
                column: "UpdateById");

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentRoleAttributeValue_DepartmentRoleAttribute_DepartmentRoleAttributeId",
                table: "DepartmentRoleAttributeValue",
                column: "DepartmentRoleAttributeId",
                principalTable: "DepartmentRoleAttribute",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentRoleAttributeValue_MinistryUsers_CreateById",
                table: "DepartmentRoleAttributeValue",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentRoleAttributeValue_MinistryUsers_DeleteById",
                table: "DepartmentRoleAttributeValue",
                column: "DeleteById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentRoleAttributeValue_MinistryUsers_UpdateById",
                table: "DepartmentRoleAttributeValue",
                column: "UpdateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");
        }
    }
}
