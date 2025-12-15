using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evaluation.DAL.Migrations
{
    /// <inheritdoc />
    public partial class EnhanceOrgtreeAndSchool : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EvaluationRequest_Plans_PlanId1",
                table: "EvaluationRequest");

            migrationBuilder.DropIndex(
                name: "IX_EvaluationRequest_PlanId1",
                table: "EvaluationRequest");

            migrationBuilder.DropColumn(
                name: "AcceditedDate",
                table: "OrgTree");

            migrationBuilder.DropColumn(
                name: "IsAccredited",
                table: "OrgTree");

            migrationBuilder.DropColumn(
                name: "SupportIdentity",
                table: "OrgTree");

            migrationBuilder.DropColumn(
                name: "SupportIdentityDate",
                table: "OrgTree");

            migrationBuilder.DropColumn(
                name: "PlanId1",
                table: "EvaluationRequest");

            migrationBuilder.AddColumn<DateOnly>(
                name: "AcceditedDate",
                table: "Schools",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAccredited",
                table: "Schools",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SupportIdentity",
                table: "Schools",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateOnly>(
                name: "SupportIdentityDate",
                table: "Schools",
                type: "date",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BackendName",
                table: "PlanStatuses",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Value",
                table: "FormItemValues",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<Guid>(
                name: "DepEvalMatrixId",
                table: "FormItemValues",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DepEvalMatrixValue",
                table: "FormItemValues",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ItemValue",
                table: "DepEvalMatrixs",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlanStatuses_BackendName",
                table: "PlanStatuses",
                column: "BackendName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FormItemValues_DepEvalMatrixId",
                table: "FormItemValues",
                column: "DepEvalMatrixId");

            migrationBuilder.AddForeignKey(
                name: "FK_FormItemValues_DepEvalMatrixs_DepEvalMatrixId",
                table: "FormItemValues",
                column: "DepEvalMatrixId",
                principalTable: "DepEvalMatrixs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormItemValues_DepEvalMatrixs_DepEvalMatrixId",
                table: "FormItemValues");

            migrationBuilder.DropIndex(
                name: "IX_PlanStatuses_BackendName",
                table: "PlanStatuses");

            migrationBuilder.DropIndex(
                name: "IX_FormItemValues_DepEvalMatrixId",
                table: "FormItemValues");

            migrationBuilder.DropColumn(
                name: "AcceditedDate",
                table: "Schools");

            migrationBuilder.DropColumn(
                name: "IsAccredited",
                table: "Schools");

            migrationBuilder.DropColumn(
                name: "SupportIdentity",
                table: "Schools");

            migrationBuilder.DropColumn(
                name: "SupportIdentityDate",
                table: "Schools");

            migrationBuilder.DropColumn(
                name: "DepEvalMatrixId",
                table: "FormItemValues");

            migrationBuilder.DropColumn(
                name: "DepEvalMatrixValue",
                table: "FormItemValues");

            migrationBuilder.DropColumn(
                name: "ItemValue",
                table: "DepEvalMatrixs");

            migrationBuilder.AlterColumn<string>(
                name: "BackendName",
                table: "PlanStatuses",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<DateOnly>(
                name: "AcceditedDate",
                table: "OrgTree",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAccredited",
                table: "OrgTree",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SupportIdentity",
                table: "OrgTree",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateOnly>(
                name: "SupportIdentityDate",
                table: "OrgTree",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AlterColumn<decimal>(
                name: "Value",
                table: "FormItemValues",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

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
        }
    }
}
