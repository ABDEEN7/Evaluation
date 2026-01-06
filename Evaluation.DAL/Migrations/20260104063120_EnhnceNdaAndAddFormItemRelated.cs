using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evaluation.DAL.Migrations
{
    /// <inheritdoc />
    public partial class EnhnceNdaAndAddFormItemRelated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NDAApproveDate",
                table: "EvaluationRequestAssignment");

            migrationBuilder.DropColumn(
                name: "NDAStatusId",
                table: "EvaluationRequestAssignment");

            migrationBuilder.CreateTable(
                name: "FormItemRelated",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FormItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RelatedItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_FormItemRelated", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormItemRelated_FormItem_FormItemId",
                        column: x => x.FormItemId,
                        principalTable: "FormItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FormItemRelated_FormItem_RelatedItemId",
                        column: x => x.RelatedItemId,
                        principalTable: "FormItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FormItemRelated_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FormItemRelated_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FormItemRelated_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NdaStatus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BackendName = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    table.PrimaryKey("PK_NdaStatus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NdaStatus_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NdaStatus_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NdaStatus_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FormItemRelated_CreateById",
                table: "FormItemRelated",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_FormItemRelated_DeleteById",
                table: "FormItemRelated",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_FormItemRelated_FormItemId",
                table: "FormItemRelated",
                column: "FormItemId");

            migrationBuilder.CreateIndex(
                name: "IX_FormItemRelated_RelatedItemId",
                table: "FormItemRelated",
                column: "RelatedItemId");

            migrationBuilder.CreateIndex(
                name: "IX_FormItemRelated_UpdateById",
                table: "FormItemRelated",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_NdaStatus_CreateById",
                table: "NdaStatus",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_NdaStatus_DeleteById",
                table: "NdaStatus",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_NdaStatus_UpdateById",
                table: "NdaStatus",
                column: "UpdateById");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FormItemRelated");

            migrationBuilder.DropTable(
                name: "NdaStatus");

            migrationBuilder.AddColumn<bool>(
                name: "NDAApproveDate",
                table: "EvaluationRequestAssignment",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "NDAStatusId",
                table: "EvaluationRequestAssignment",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
