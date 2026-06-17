using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evaluation.DAL.Migrations
{
    /// <inheritdoc />
    public partial class EnhanceEvalPartyCategoryANDService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceType_MinistryUsers_CreateById",
                table: "ServiceType");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceType_MinistryUsers_DeleteById",
                table: "ServiceType");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceType_MinistryUsers_UpdateById",
                table: "ServiceType");

            migrationBuilder.AddColumn<int>(
                name: "OrderNo",
                table: "ServiceType",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "ServiceTypeId",
                table: "Services",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ColorCode",
                table: "PlanStatuses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EvalPartyCategoryId",
                table: "EvaluationParties",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EvalPartyCategory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BackendName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderNo = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_EvalPartyCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EvalPartyCategory_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EvalPartyCategory_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EvalPartyCategory_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Services_ServiceTypeId",
                table: "Services",
                column: "ServiceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_EvaluationParties_EvalPartyCategoryId",
                table: "EvaluationParties",
                column: "EvalPartyCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_EvalPartyCategory_CreateById",
                table: "EvalPartyCategory",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_EvalPartyCategory_DeleteById",
                table: "EvalPartyCategory",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_EvalPartyCategory_UpdateById",
                table: "EvalPartyCategory",
                column: "UpdateById");

            migrationBuilder.AddForeignKey(
                name: "FK_EvaluationParties_EvalPartyCategory_EvalPartyCategoryId",
                table: "EvaluationParties",
                column: "EvalPartyCategoryId",
                principalTable: "EvalPartyCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Services_ServiceType_ServiceTypeId",
                table: "Services",
                column: "ServiceTypeId",
                principalTable: "ServiceType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceType_MinistryUsers_CreateById",
                table: "ServiceType",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceType_MinistryUsers_DeleteById",
                table: "ServiceType",
                column: "DeleteById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceType_MinistryUsers_UpdateById",
                table: "ServiceType",
                column: "UpdateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EvaluationParties_EvalPartyCategory_EvalPartyCategoryId",
                table: "EvaluationParties");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_ServiceType_ServiceTypeId",
                table: "Services");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceType_MinistryUsers_CreateById",
                table: "ServiceType");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceType_MinistryUsers_DeleteById",
                table: "ServiceType");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceType_MinistryUsers_UpdateById",
                table: "ServiceType");

            migrationBuilder.DropTable(
                name: "EvalPartyCategory");

            migrationBuilder.DropIndex(
                name: "IX_Services_ServiceTypeId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_EvaluationParties_EvalPartyCategoryId",
                table: "EvaluationParties");

            migrationBuilder.DropColumn(
                name: "OrderNo",
                table: "ServiceType");

            migrationBuilder.DropColumn(
                name: "ServiceTypeId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "ColorCode",
                table: "PlanStatuses");

            migrationBuilder.DropColumn(
                name: "EvalPartyCategoryId",
                table: "EvaluationParties");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceType_MinistryUsers_CreateById",
                table: "ServiceType",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceType_MinistryUsers_DeleteById",
                table: "ServiceType",
                column: "DeleteById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceType_MinistryUsers_UpdateById",
                table: "ServiceType",
                column: "UpdateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");
        }
    }
}
