using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evaluation.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddNotificationCountAndSchoolLevel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EducationLevelId",
                table: "ServiceRequests",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolClassId",
                table: "ServiceRequests",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolCourseId",
                table: "ServiceRequests",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "VisitorUserId",
                table: "ServiceRequests",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EmailMinsFromAction",
                table: "ActionStatusConfigNotification",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NotificationMinsFromAction",
                table: "ActionStatusConfigNotification",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SMSMinsFromAction",
                table: "ActionStatusConfigNotification",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequests_EducationLevelId",
                table: "ServiceRequests",
                column: "EducationLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequests_SchoolClassId",
                table: "ServiceRequests",
                column: "SchoolClassId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequests_SchoolCourseId",
                table: "ServiceRequests",
                column: "SchoolCourseId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequests_VisitorUserId",
                table: "ServiceRequests",
                column: "VisitorUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceRequests_EducationLevel_EducationLevelId",
                table: "ServiceRequests",
                column: "EducationLevelId",
                principalTable: "EducationLevel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceRequests_MinistryUsers_VisitorUserId",
                table: "ServiceRequests",
                column: "VisitorUserId",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceRequests_SchoolClass_SchoolClassId",
                table: "ServiceRequests",
                column: "SchoolClassId",
                principalTable: "SchoolClass",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceRequests_SchoolCourse_SchoolCourseId",
                table: "ServiceRequests",
                column: "SchoolCourseId",
                principalTable: "SchoolCourse",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRequests_EducationLevel_EducationLevelId",
                table: "ServiceRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRequests_MinistryUsers_VisitorUserId",
                table: "ServiceRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRequests_SchoolClass_SchoolClassId",
                table: "ServiceRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRequests_SchoolCourse_SchoolCourseId",
                table: "ServiceRequests");

            migrationBuilder.DropIndex(
                name: "IX_ServiceRequests_EducationLevelId",
                table: "ServiceRequests");

            migrationBuilder.DropIndex(
                name: "IX_ServiceRequests_SchoolClassId",
                table: "ServiceRequests");

            migrationBuilder.DropIndex(
                name: "IX_ServiceRequests_SchoolCourseId",
                table: "ServiceRequests");

            migrationBuilder.DropIndex(
                name: "IX_ServiceRequests_VisitorUserId",
                table: "ServiceRequests");

            migrationBuilder.DropColumn(
                name: "EducationLevelId",
                table: "ServiceRequests");

            migrationBuilder.DropColumn(
                name: "SchoolClassId",
                table: "ServiceRequests");

            migrationBuilder.DropColumn(
                name: "SchoolCourseId",
                table: "ServiceRequests");

            migrationBuilder.DropColumn(
                name: "VisitorUserId",
                table: "ServiceRequests");

            migrationBuilder.DropColumn(
                name: "EmailMinsFromAction",
                table: "ActionStatusConfigNotification");

            migrationBuilder.DropColumn(
                name: "NotificationMinsFromAction",
                table: "ActionStatusConfigNotification");

            migrationBuilder.DropColumn(
                name: "SMSMinsFromAction",
                table: "ActionStatusConfigNotification");
        }
    }
}
