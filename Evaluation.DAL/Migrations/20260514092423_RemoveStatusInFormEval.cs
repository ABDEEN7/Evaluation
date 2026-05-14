using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evaluation.DAL.Migrations
{
    /// <inheritdoc />
    public partial class RemoveStatusInFormEval : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EvalForms_FormStatus_FormStatusId",
                table: "EvalForms");

            migrationBuilder.DropTable(
                name: "FormStatus");

            migrationBuilder.DropIndex(
                name: "IX_EvalForms_FormStatusId",
                table: "EvalForms");

            migrationBuilder.DropColumn(
                name: "FormStatusId",
                table: "EvalForms");

            migrationBuilder.AlterColumn<Guid>(
                name: "PartyTypeId",
                table: "FormItemConfig",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<decimal>(
                name: "FinalEvalValue",
                table: "EvaluationRequest",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DepartmentId",
                table: "EvalFormType",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EvalFormType_DepartmentId",
                table: "EvalFormType",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_EvalFormType_Departments_DepartmentId",
                table: "EvalFormType",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EvalFormType_Departments_DepartmentId",
                table: "EvalFormType");

            migrationBuilder.DropIndex(
                name: "IX_EvalFormType_DepartmentId",
                table: "EvalFormType");

            migrationBuilder.DropColumn(
                name: "FinalEvalValue",
                table: "EvaluationRequest");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "EvalFormType");

            migrationBuilder.AlterColumn<Guid>(
                name: "PartyTypeId",
                table: "FormItemConfig",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "FormStatusId",
                table: "EvalForms",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "FormStatus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BackendName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormStatus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormStatus_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FormStatus_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FormStatus_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EvalForms_FormStatusId",
                table: "EvalForms",
                column: "FormStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_FormStatus_CreateById",
                table: "FormStatus",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_FormStatus_DeleteById",
                table: "FormStatus",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_FormStatus_UpdateById",
                table: "FormStatus",
                column: "UpdateById");

            migrationBuilder.AddForeignKey(
                name: "FK_EvalForms_FormStatus_FormStatusId",
                table: "EvalForms",
                column: "FormStatusId",
                principalTable: "FormStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
