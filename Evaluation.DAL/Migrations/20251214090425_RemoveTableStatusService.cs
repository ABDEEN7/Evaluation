using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evaluation.DAL.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTableStatusService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EvaluationRequest_StatusService_StatusServiceId",
                table: "EvaluationRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_EvaluationRequestHistory_StatusService_StatusServiceId",
                table: "EvaluationRequestHistory");

            migrationBuilder.DropTable(
                name: "StatusService");

            migrationBuilder.RenameColumn(
                name: "StatusServiceId",
                table: "EvaluationRequestHistory",
                newName: "ServiceStatusId");

            migrationBuilder.RenameIndex(
                name: "IX_EvaluationRequestHistory_StatusServiceId",
                table: "EvaluationRequestHistory",
                newName: "IX_EvaluationRequestHistory_ServiceStatusId");

            migrationBuilder.RenameColumn(
                name: "StatusServiceId",
                table: "EvaluationRequest",
                newName: "ServiceStatusId");

            migrationBuilder.RenameIndex(
                name: "IX_EvaluationRequest_StatusServiceId",
                table: "EvaluationRequest",
                newName: "IX_EvaluationRequest_ServiceStatusId");

            migrationBuilder.AddColumn<Guid>(
                name: "PlanId1",
                table: "EvaluationRequest",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EvaluationRequest_PlanId1",
                table: "EvaluationRequest",
                column: "PlanId1");

            migrationBuilder.AddForeignKey(
                name: "FK_EvaluationRequest_Plans_PlanId1",
                table: "EvaluationRequest",
                column: "PlanId1",
                principalTable: "Plans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EvaluationRequest_ServiceStatus_ServiceStatusId",
                table: "EvaluationRequest",
                column: "ServiceStatusId",
                principalTable: "ServiceStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EvaluationRequestHistory_ServiceStatus_ServiceStatusId",
                table: "EvaluationRequestHistory",
                column: "ServiceStatusId",
                principalTable: "ServiceStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EvaluationRequest_Plans_PlanId1",
                table: "EvaluationRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_EvaluationRequest_ServiceStatus_ServiceStatusId",
                table: "EvaluationRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_EvaluationRequestHistory_ServiceStatus_ServiceStatusId",
                table: "EvaluationRequestHistory");

            migrationBuilder.DropIndex(
                name: "IX_EvaluationRequest_PlanId1",
                table: "EvaluationRequest");

            migrationBuilder.DropColumn(
                name: "PlanId1",
                table: "EvaluationRequest");

            migrationBuilder.RenameColumn(
                name: "ServiceStatusId",
                table: "EvaluationRequestHistory",
                newName: "StatusServiceId");

            migrationBuilder.RenameIndex(
                name: "IX_EvaluationRequestHistory_ServiceStatusId",
                table: "EvaluationRequestHistory",
                newName: "IX_EvaluationRequestHistory_StatusServiceId");

            migrationBuilder.RenameColumn(
                name: "ServiceStatusId",
                table: "EvaluationRequest",
                newName: "StatusServiceId");

            migrationBuilder.RenameIndex(
                name: "IX_EvaluationRequest_ServiceStatusId",
                table: "EvaluationRequest",
                newName: "IX_EvaluationRequest_StatusServiceId");

            migrationBuilder.CreateTable(
                name: "StatusService",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                    table.PrimaryKey("PK_StatusService", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StatusService_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StatusService_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StatusService_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StatusService_CreateById",
                table: "StatusService",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_StatusService_DeleteById",
                table: "StatusService",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_StatusService_UpdateById",
                table: "StatusService",
                column: "UpdateById");

            migrationBuilder.AddForeignKey(
                name: "FK_EvaluationRequest_StatusService_StatusServiceId",
                table: "EvaluationRequest",
                column: "StatusServiceId",
                principalTable: "StatusService",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EvaluationRequestHistory_StatusService_StatusServiceId",
                table: "EvaluationRequestHistory",
                column: "StatusServiceId",
                principalTable: "StatusService",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
