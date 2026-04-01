using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evaluation.DAL.Migrations
{
    public partial class addnewattributes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "ServiceStatusTypeId",
                table: "ServiceStatus",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            // =========================
            // ServiceRequests
            // =========================
            migrationBuilder.DropColumn(
                name: "Sequence",
                table: "ServiceRequests");

            migrationBuilder.AddColumn<long>(
                name: "Sequence",
                table: "ServiceRequests",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            // =========================
            // EvaluationRequest
            // =========================
            migrationBuilder.DropColumn(
                name: "Sequence",
                table: "EvaluationRequest");

            migrationBuilder.AddColumn<long>(
                name: "Sequence",
                table: "EvaluationRequest",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // الرجوع ServiceStatusTypeId
            migrationBuilder.AlterColumn<Guid>(
                name: "ServiceStatusTypeId",
                table: "ServiceStatus",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            // =========================
            // ServiceRequests (إزالة Identity)
            // =========================
            migrationBuilder.DropColumn(
                name: "Sequence",
                table: "ServiceRequests");

            migrationBuilder.AddColumn<long>(
                name: "Sequence",
                table: "ServiceRequests",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            // =========================
            // EvaluationRequest
            // =========================
            migrationBuilder.DropColumn(
                name: "Sequence",
                table: "EvaluationRequest");

            migrationBuilder.AddColumn<long>(
                name: "Sequence",
                table: "EvaluationRequest",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}