using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evaluation.DAL.Migrations
{
    /// <inheritdoc />
    public partial class RemoveOrgTypeIdFromOrganization : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_SchoolTypes_TypeId",
                table: "Organizations");

            migrationBuilder.DropIndex(
                name: "IX_Organizations_TypeId",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "Organizations");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "EstablishmentDate",
                table: "Organizations",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateOnly>(
                name: "EstablishmentDate",
                table: "Organizations",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1),
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TypeId",
                table: "Organizations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_TypeId",
                table: "Organizations",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_SchoolTypes_TypeId",
                table: "Organizations",
                column: "TypeId",
                principalTable: "SchoolTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
