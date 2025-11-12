using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evaluation.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ChnagePlanSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Plans_PlanType_PlanTypeId",
                table: "Plans");

            migrationBuilder.DropForeignKey(
                name: "FK_PlanTypeDepartment_Departments_DepartmentId",
                table: "PlanTypeDepartment");

            migrationBuilder.DropForeignKey(
                name: "FK_PlanTypeDepartment_MinistryUsers_CreateById",
                table: "PlanTypeDepartment");

            migrationBuilder.DropForeignKey(
                name: "FK_PlanTypeDepartment_MinistryUsers_DeleteById",
                table: "PlanTypeDepartment");

            migrationBuilder.DropForeignKey(
                name: "FK_PlanTypeDepartment_MinistryUsers_UpdateById",
                table: "PlanTypeDepartment");

            migrationBuilder.DropForeignKey(
                name: "FK_PlanTypeDepartment_PlanType_PlanTypeId",
                table: "PlanTypeDepartment");

            migrationBuilder.DropColumn(
                name: "OrderNo",
                table: "PlanTypeDepartment");

            migrationBuilder.RenameColumn(
                name: "PlanTypeId",
                table: "Plans",
                newName: "PlanTypeDepartmentId");

            migrationBuilder.RenameColumn(
                name: "ExpectedListJson",
                table: "Plans",
                newName: "PlanJsonValue");

            migrationBuilder.RenameIndex(
                name: "IX_Plans_PlanTypeId",
                table: "Plans",
                newName: "IX_Plans_PlanTypeDepartmentId");

            migrationBuilder.AddColumn<string>(
                name: "NameAr",
                table: "PlanTypeDepartment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NameEn",
                table: "PlanTypeDepartment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "SemesterId",
                table: "Plans",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Semesters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AcademicYearId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                    table.PrimaryKey("PK_Semesters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Semesters_AcademicYears_AcademicYearId",
                        column: x => x.AcademicYearId,
                        principalTable: "AcademicYears",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Semesters_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Semesters_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Semesters_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Plans_SemesterId",
                table: "Plans",
                column: "SemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_Semesters_AcademicYearId",
                table: "Semesters",
                column: "AcademicYearId");

            migrationBuilder.CreateIndex(
                name: "IX_Semesters_CreateById",
                table: "Semesters",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_Semesters_DeleteById",
                table: "Semesters",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_Semesters_UpdateById",
                table: "Semesters",
                column: "UpdateById");

            migrationBuilder.AddForeignKey(
                name: "FK_Plans_PlanTypeDepartment_PlanTypeDepartmentId",
                table: "Plans",
                column: "PlanTypeDepartmentId",
                principalTable: "PlanTypeDepartment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Plans_Semesters_SemesterId",
                table: "Plans",
                column: "SemesterId",
                principalTable: "Semesters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PlanTypeDepartment_Departments_DepartmentId",
                table: "PlanTypeDepartment",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PlanTypeDepartment_MinistryUsers_CreateById",
                table: "PlanTypeDepartment",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PlanTypeDepartment_MinistryUsers_DeleteById",
                table: "PlanTypeDepartment",
                column: "DeleteById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PlanTypeDepartment_MinistryUsers_UpdateById",
                table: "PlanTypeDepartment",
                column: "UpdateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PlanTypeDepartment_PlanType_PlanTypeId",
                table: "PlanTypeDepartment",
                column: "PlanTypeId",
                principalTable: "PlanType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Plans_PlanTypeDepartment_PlanTypeDepartmentId",
                table: "Plans");

            migrationBuilder.DropForeignKey(
                name: "FK_Plans_Semesters_SemesterId",
                table: "Plans");

            migrationBuilder.DropForeignKey(
                name: "FK_PlanTypeDepartment_Departments_DepartmentId",
                table: "PlanTypeDepartment");

            migrationBuilder.DropForeignKey(
                name: "FK_PlanTypeDepartment_MinistryUsers_CreateById",
                table: "PlanTypeDepartment");

            migrationBuilder.DropForeignKey(
                name: "FK_PlanTypeDepartment_MinistryUsers_DeleteById",
                table: "PlanTypeDepartment");

            migrationBuilder.DropForeignKey(
                name: "FK_PlanTypeDepartment_MinistryUsers_UpdateById",
                table: "PlanTypeDepartment");

            migrationBuilder.DropForeignKey(
                name: "FK_PlanTypeDepartment_PlanType_PlanTypeId",
                table: "PlanTypeDepartment");

            migrationBuilder.DropTable(
                name: "Semesters");

            migrationBuilder.DropIndex(
                name: "IX_Plans_SemesterId",
                table: "Plans");

            migrationBuilder.DropColumn(
                name: "NameAr",
                table: "PlanTypeDepartment");

            migrationBuilder.DropColumn(
                name: "NameEn",
                table: "PlanTypeDepartment");

            migrationBuilder.DropColumn(
                name: "SemesterId",
                table: "Plans");

            migrationBuilder.RenameColumn(
                name: "PlanTypeDepartmentId",
                table: "Plans",
                newName: "PlanTypeId");

            migrationBuilder.RenameColumn(
                name: "PlanJsonValue",
                table: "Plans",
                newName: "ExpectedListJson");

            migrationBuilder.RenameIndex(
                name: "IX_Plans_PlanTypeDepartmentId",
                table: "Plans",
                newName: "IX_Plans_PlanTypeId");

            migrationBuilder.AddColumn<Guid>(
                name: "OrderNo",
                table: "PlanTypeDepartment",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddForeignKey(
                name: "FK_Plans_PlanType_PlanTypeId",
                table: "Plans",
                column: "PlanTypeId",
                principalTable: "PlanType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PlanTypeDepartment_Departments_DepartmentId",
                table: "PlanTypeDepartment",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlanTypeDepartment_MinistryUsers_CreateById",
                table: "PlanTypeDepartment",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlanTypeDepartment_MinistryUsers_DeleteById",
                table: "PlanTypeDepartment",
                column: "DeleteById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlanTypeDepartment_MinistryUsers_UpdateById",
                table: "PlanTypeDepartment",
                column: "UpdateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlanTypeDepartment_PlanType_PlanTypeId",
                table: "PlanTypeDepartment",
                column: "PlanTypeId",
                principalTable: "PlanType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
