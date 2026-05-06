using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evaluation.DAL.Migrations
{
    /// <inheritdoc />
    public partial class EnhanceIntegrationAndRemoveIsEvalFromEvalForm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IntegrationPoint_MinistryUsers_CreateById",
                table: "IntegrationPoint");

            migrationBuilder.DropForeignKey(
                name: "FK_IntegrationPoint_MinistryUsers_DeleteById",
                table: "IntegrationPoint");

            migrationBuilder.DropForeignKey(
                name: "FK_IntegrationPoint_MinistryUsers_UpdateById",
                table: "IntegrationPoint");

            migrationBuilder.DropColumn(
                name: "IsEvaluation",
                table: "FormItem");

            migrationBuilder.DropColumn(
                name: "HasEvaluation",
                table: "EvalForms");

            migrationBuilder.AddColumn<string>(
                name: "Config",
                table: "IntegrationPoint",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EvaluationRequestId",
                table: "FormItemValues",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ServiceRequestId",
                table: "FormItemValues",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RequestNumber",
                table: "EvaluationRequest",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "IntegrationPointLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IntegrationPointId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                    table.PrimaryKey("PK_IntegrationPointLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IntegrationPointLog_IntegrationPoint_IntegrationPointId",
                        column: x => x.IntegrationPointId,
                        principalTable: "IntegrationPoint",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IntegrationPointLog_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IntegrationPointLog_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IntegrationPointLog_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IntegrationPointDataLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IntegrationPointLogId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrgTreeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DataResponse = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    table.PrimaryKey("PK_IntegrationPointDataLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IntegrationPointDataLog_IntegrationPointLog_IntegrationPointLogId",
                        column: x => x.IntegrationPointLogId,
                        principalTable: "IntegrationPointLog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IntegrationPointDataLog_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IntegrationPointDataLog_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IntegrationPointDataLog_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IntegrationPointDataLog_OrgTree_OrgTreeId",
                        column: x => x.OrgTreeId,
                        principalTable: "OrgTree",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FormItemValues_EvaluationRequestId",
                table: "FormItemValues",
                column: "EvaluationRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_FormItemValues_ServiceRequestId",
                table: "FormItemValues",
                column: "ServiceRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_IntegrationPointDataLog_CreateById",
                table: "IntegrationPointDataLog",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_IntegrationPointDataLog_DeleteById",
                table: "IntegrationPointDataLog",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_IntegrationPointDataLog_IntegrationPointLogId",
                table: "IntegrationPointDataLog",
                column: "IntegrationPointLogId");

            migrationBuilder.CreateIndex(
                name: "IX_IntegrationPointDataLog_OrgTreeId",
                table: "IntegrationPointDataLog",
                column: "OrgTreeId");

            migrationBuilder.CreateIndex(
                name: "IX_IntegrationPointDataLog_UpdateById",
                table: "IntegrationPointDataLog",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_IntegrationPointLog_CreateById",
                table: "IntegrationPointLog",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_IntegrationPointLog_DeleteById",
                table: "IntegrationPointLog",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_IntegrationPointLog_IntegrationPointId",
                table: "IntegrationPointLog",
                column: "IntegrationPointId");

            migrationBuilder.CreateIndex(
                name: "IX_IntegrationPointLog_UpdateById",
                table: "IntegrationPointLog",
                column: "UpdateById");

            migrationBuilder.AddForeignKey(
                name: "FK_FormItemValues_EvaluationRequest_EvaluationRequestId",
                table: "FormItemValues",
                column: "EvaluationRequestId",
                principalTable: "EvaluationRequest",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FormItemValues_ServiceRequests_ServiceRequestId",
                table: "FormItemValues",
                column: "ServiceRequestId",
                principalTable: "ServiceRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IntegrationPoint_MinistryUsers_CreateById",
                table: "IntegrationPoint",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IntegrationPoint_MinistryUsers_DeleteById",
                table: "IntegrationPoint",
                column: "DeleteById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IntegrationPoint_MinistryUsers_UpdateById",
                table: "IntegrationPoint",
                column: "UpdateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormItemValues_EvaluationRequest_EvaluationRequestId",
                table: "FormItemValues");

            migrationBuilder.DropForeignKey(
                name: "FK_FormItemValues_ServiceRequests_ServiceRequestId",
                table: "FormItemValues");

            migrationBuilder.DropForeignKey(
                name: "FK_IntegrationPoint_MinistryUsers_CreateById",
                table: "IntegrationPoint");

            migrationBuilder.DropForeignKey(
                name: "FK_IntegrationPoint_MinistryUsers_DeleteById",
                table: "IntegrationPoint");

            migrationBuilder.DropForeignKey(
                name: "FK_IntegrationPoint_MinistryUsers_UpdateById",
                table: "IntegrationPoint");

            migrationBuilder.DropTable(
                name: "IntegrationPointDataLog");

            migrationBuilder.DropTable(
                name: "IntegrationPointLog");

            migrationBuilder.DropIndex(
                name: "IX_FormItemValues_EvaluationRequestId",
                table: "FormItemValues");

            migrationBuilder.DropIndex(
                name: "IX_FormItemValues_ServiceRequestId",
                table: "FormItemValues");

            migrationBuilder.DropColumn(
                name: "Config",
                table: "IntegrationPoint");

            migrationBuilder.DropColumn(
                name: "EvaluationRequestId",
                table: "FormItemValues");

            migrationBuilder.DropColumn(
                name: "ServiceRequestId",
                table: "FormItemValues");

            migrationBuilder.AddColumn<bool>(
                name: "IsEvaluation",
                table: "FormItem",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "RequestNumber",
                table: "EvaluationRequest",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<bool>(
                name: "HasEvaluation",
                table: "EvalForms",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_IntegrationPoint_MinistryUsers_CreateById",
                table: "IntegrationPoint",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IntegrationPoint_MinistryUsers_DeleteById",
                table: "IntegrationPoint",
                column: "DeleteById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_IntegrationPoint_MinistryUsers_UpdateById",
                table: "IntegrationPoint",
                column: "UpdateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");
        }
    }
}
