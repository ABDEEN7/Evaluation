using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evaluation.DAL.Migrations
{
    /// <inheritdoc />
    public partial class EnhanceTemplates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DepartmentId",
                table: "SMSTemplates",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ServiceId",
                table: "SMSTemplates",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DepartmentId",
                table: "NotificationTemplates",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ServiceId",
                table: "NotificationTemplates",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DepartmentId",
                table: "EmailTemplates",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SMSTemplates_DepartmentId",
                table: "SMSTemplates",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_SMSTemplates_ServiceId",
                table: "SMSTemplates",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationTemplates_DepartmentId",
                table: "NotificationTemplates",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationTemplates_ServiceId",
                table: "NotificationTemplates",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailTemplates_DepartmentId",
                table: "EmailTemplates",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailTemplates_ServiceId",
                table: "EmailTemplates",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmailTemplates_Departments_DepartmentId",
                table: "EmailTemplates",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EmailTemplates_Services_ServiceId",
                table: "EmailTemplates",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationTemplates_Departments_DepartmentId",
                table: "NotificationTemplates",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationTemplates_Services_ServiceId",
                table: "NotificationTemplates",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SMSTemplates_Departments_DepartmentId",
                table: "SMSTemplates",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SMSTemplates_Services_ServiceId",
                table: "SMSTemplates",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailTemplates_Departments_DepartmentId",
                table: "EmailTemplates");

            migrationBuilder.DropForeignKey(
                name: "FK_EmailTemplates_Services_ServiceId",
                table: "EmailTemplates");

            migrationBuilder.DropForeignKey(
                name: "FK_NotificationTemplates_Departments_DepartmentId",
                table: "NotificationTemplates");

            migrationBuilder.DropForeignKey(
                name: "FK_NotificationTemplates_Services_ServiceId",
                table: "NotificationTemplates");

            migrationBuilder.DropForeignKey(
                name: "FK_SMSTemplates_Departments_DepartmentId",
                table: "SMSTemplates");

            migrationBuilder.DropForeignKey(
                name: "FK_SMSTemplates_Services_ServiceId",
                table: "SMSTemplates");

            migrationBuilder.DropIndex(
                name: "IX_SMSTemplates_DepartmentId",
                table: "SMSTemplates");

            migrationBuilder.DropIndex(
                name: "IX_SMSTemplates_ServiceId",
                table: "SMSTemplates");

            migrationBuilder.DropIndex(
                name: "IX_NotificationTemplates_DepartmentId",
                table: "NotificationTemplates");

            migrationBuilder.DropIndex(
                name: "IX_NotificationTemplates_ServiceId",
                table: "NotificationTemplates");

            migrationBuilder.DropIndex(
                name: "IX_EmailTemplates_DepartmentId",
                table: "EmailTemplates");

            migrationBuilder.DropIndex(
                name: "IX_EmailTemplates_ServiceId",
                table: "EmailTemplates");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "SMSTemplates");

            migrationBuilder.DropColumn(
                name: "ServiceId",
                table: "SMSTemplates");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "NotificationTemplates");

            migrationBuilder.DropColumn(
                name: "ServiceId",
                table: "NotificationTemplates");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "EmailTemplates");
        }
    }
}
