using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evaluation.DAL.Migrations
{
    /// <inheritdoc />
    public partial class EnhanceSchoolCourseAndSection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SchoolGradeSectionCourse_Employees_EmployeeId",
                table: "SchoolGradeSectionCourse");

            migrationBuilder.DropForeignKey(
                name: "FK_SchoolGradeSectionCourse_SchoolGradeSction_SchoolGradeSctionId",
                table: "SchoolGradeSectionCourse");

            migrationBuilder.DropIndex(
                name: "IX_SchoolGradeSectionCourse_EmployeeId",
                table: "SchoolGradeSectionCourse");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "SchoolGradeSectionCourse");

            migrationBuilder.DropColumn(
                name: "Grade",
                table: "SchoolGradeSction");

            migrationBuilder.DropColumn(
                name: "IntegrationCode",
                table: "SchoolGradeSction");

            migrationBuilder.DropColumn(
                name: "NameAr",
                table: "SchoolGradeSction");

            migrationBuilder.DropColumn(
                name: "NameEn",
                table: "SchoolGradeSction");

            migrationBuilder.AddColumn<string>(
                name: "QID",
                table: "SchoolGradeSectionCourse",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "GradeLevelId",
                table: "SchoolGradeSction",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "GradeLevel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EducationLevelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Grade = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    orderNo = table.Column<int>(type: "int", nullable: false),
                    IntegrationCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_GradeLevel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GradeLevel_EducationLevel_EducationLevelId",
                        column: x => x.EducationLevelId,
                        principalTable: "EducationLevel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GradeLevel_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GradeLevel_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GradeLevel_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SchoolGradeSection",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SchoolGradeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SectionAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SectionEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IntegrationCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_SchoolGradeSection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchoolGradeSection_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SchoolGradeSection_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SchoolGradeSection_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SchoolGradeSection_SchoolGradeSction_SchoolGradeId",
                        column: x => x.SchoolGradeId,
                        principalTable: "SchoolGradeSction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SchoolGradeSction_GradeLevelId",
                table: "SchoolGradeSction",
                column: "GradeLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_GradeLevel_CreateById",
                table: "GradeLevel",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_GradeLevel_DeleteById",
                table: "GradeLevel",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_GradeLevel_EducationLevelId",
                table: "GradeLevel",
                column: "EducationLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_GradeLevel_UpdateById",
                table: "GradeLevel",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolGradeSection_CreateById",
                table: "SchoolGradeSection",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolGradeSection_DeleteById",
                table: "SchoolGradeSection",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolGradeSection_SchoolGradeId",
                table: "SchoolGradeSection",
                column: "SchoolGradeId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolGradeSection_UpdateById",
                table: "SchoolGradeSection",
                column: "UpdateById");

            migrationBuilder.AddForeignKey(
                name: "FK_SchoolGradeSction_GradeLevel_GradeLevelId",
                table: "SchoolGradeSction",
                column: "GradeLevelId",
                principalTable: "GradeLevel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SchoolGradeSectionCourse_SchoolGradeSection_SchoolGradeSctionId",
                table: "SchoolGradeSectionCourse",
                column: "SchoolGradeSctionId",
                principalTable: "SchoolGradeSection",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SchoolGradeSction_GradeLevel_GradeLevelId",
                table: "SchoolGradeSction");

            migrationBuilder.DropForeignKey(
                name: "FK_SchoolGradeSectionCourse_SchoolGradeSection_SchoolGradeSctionId",
                table: "SchoolGradeSectionCourse");

            migrationBuilder.DropTable(
                name: "GradeLevel");

            migrationBuilder.DropTable(
                name: "SchoolGradeSection");

            migrationBuilder.DropIndex(
                name: "IX_SchoolGradeSction_GradeLevelId",
                table: "SchoolGradeSction");

            migrationBuilder.DropColumn(
                name: "QID",
                table: "SchoolGradeSectionCourse");

            migrationBuilder.DropColumn(
                name: "GradeLevelId",
                table: "SchoolGradeSction");

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId",
                table: "SchoolGradeSectionCourse",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Grade",
                table: "SchoolGradeSction",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IntegrationCode",
                table: "SchoolGradeSction",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameAr",
                table: "SchoolGradeSction",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NameEn",
                table: "SchoolGradeSction",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolGradeSectionCourse_EmployeeId",
                table: "SchoolGradeSectionCourse",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_SchoolGradeSectionCourse_Employees_EmployeeId",
                table: "SchoolGradeSectionCourse",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SchoolGradeSectionCourse_SchoolGradeSction_SchoolGradeSctionId",
                table: "SchoolGradeSectionCourse",
                column: "SchoolGradeSctionId",
                principalTable: "SchoolGradeSction",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
