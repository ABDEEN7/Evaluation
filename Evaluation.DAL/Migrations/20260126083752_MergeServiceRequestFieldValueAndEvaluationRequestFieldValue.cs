using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evaluation.DAL.Migrations
{
    /// <inheritdoc />
    public partial class MergeServiceRequestFieldValueAndEvaluationRequestFieldValue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActionTransactionsLog_ServiceRequests_ServiceRequestId",
                table: "ActionTransactionsLog");

            migrationBuilder.DropTable(
                name: "EvaluationRequestFieldsValue");

            migrationBuilder.DropIndex(
                name: "IX_ActionTransactionsLog_ServiceRequestId",
                table: "ActionTransactionsLog");

            migrationBuilder.DropColumn(
                name: "ServiceRequestId",
                table: "ActionTransactionsLog");

            migrationBuilder.AlterColumn<Guid>(
                name: "ServiceRequestId",
                table: "ServiceRequestFieldsValue",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "EvaluationRequestId",
                table: "ServiceRequestFieldsValue",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RefId",
                table: "ServiceRequestFieldsValue",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "RequestType",
                table: "ServiceRequestFieldsValue",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RequestType",
                table: "ActionTransactionsLog",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequestFieldsValue_EvaluationRequestId",
                table: "ServiceRequestFieldsValue",
                column: "EvaluationRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceRequestFieldsValue_EvaluationRequest_EvaluationRequestId",
                table: "ServiceRequestFieldsValue",
                column: "EvaluationRequestId",
                principalTable: "EvaluationRequest",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRequestFieldsValue_EvaluationRequest_EvaluationRequestId",
                table: "ServiceRequestFieldsValue");

            migrationBuilder.DropIndex(
                name: "IX_ServiceRequestFieldsValue_EvaluationRequestId",
                table: "ServiceRequestFieldsValue");

            migrationBuilder.DropColumn(
                name: "EvaluationRequestId",
                table: "ServiceRequestFieldsValue");

            migrationBuilder.DropColumn(
                name: "RefId",
                table: "ServiceRequestFieldsValue");

            migrationBuilder.DropColumn(
                name: "RequestType",
                table: "ServiceRequestFieldsValue");

            migrationBuilder.DropColumn(
                name: "RequestType",
                table: "ActionTransactionsLog");

            migrationBuilder.AlterColumn<Guid>(
                name: "ServiceRequestId",
                table: "ServiceRequestFieldsValue",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ServiceRequestId",
                table: "ActionTransactionsLog",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "EvaluationRequestFieldsValue",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EvaluationRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FieldId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsMissing = table.Column<bool>(type: "bit", nullable: true),
                    ServiceRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvaluationRequestFieldsValue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EvaluationRequestFieldsValue_EvaluationRequest_EvaluationRequestId",
                        column: x => x.EvaluationRequestId,
                        principalTable: "EvaluationRequest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EvaluationRequestFieldsValue_Field_FieldId",
                        column: x => x.FieldId,
                        principalTable: "Field",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EvaluationRequestFieldsValue_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EvaluationRequestFieldsValue_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EvaluationRequestFieldsValue_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EvaluationRequestFieldsValue_ServiceRequests_ServiceRequestId",
                        column: x => x.ServiceRequestId,
                        principalTable: "ServiceRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActionTransactionsLog_ServiceRequestId",
                table: "ActionTransactionsLog",
                column: "ServiceRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_EvaluationRequestFieldsValue_CreateById",
                table: "EvaluationRequestFieldsValue",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_EvaluationRequestFieldsValue_DeleteById",
                table: "EvaluationRequestFieldsValue",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_EvaluationRequestFieldsValue_EvaluationRequestId",
                table: "EvaluationRequestFieldsValue",
                column: "EvaluationRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_EvaluationRequestFieldsValue_FieldId",
                table: "EvaluationRequestFieldsValue",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_EvaluationRequestFieldsValue_ServiceRequestId",
                table: "EvaluationRequestFieldsValue",
                column: "ServiceRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_EvaluationRequestFieldsValue_UpdateById",
                table: "EvaluationRequestFieldsValue",
                column: "UpdateById");

            migrationBuilder.AddForeignKey(
                name: "FK_ActionTransactionsLog_ServiceRequests_ServiceRequestId",
                table: "ActionTransactionsLog",
                column: "ServiceRequestId",
                principalTable: "ServiceRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
