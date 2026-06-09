using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evaluation.DAL.Migrations
{
    /// <inheritdoc />
    public partial class EnhnceIntegration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SchoolGradeSction_GradeLevel_GradeLevelId",
                table: "SchoolGradeSction");

            migrationBuilder.DropForeignKey(
                name: "FK_SchoolGradeSction_SchoolLevel_SchoolLevelId",
                table: "SchoolGradeSction");

            migrationBuilder.DropForeignKey(
                name: "FK_SchoolGradeSectionCourse_SchoolGradeSection_SchoolGradeSctionId",
                table: "SchoolGradeSectionCourse");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRequests_SchoolGradeSection_SchoolGradeSectionId",
                table: "ServiceRequests");

            migrationBuilder.DropTable(
                name: "SchoolGradeSection");

            migrationBuilder.DropIndex(
                name: "IX_SchoolGradeSction_GradeLevelId",
                table: "SchoolGradeSction");

            migrationBuilder.DropColumn(
                name: "GradeLevelId",
                table: "SchoolGradeSction");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "SchoolGradeSction",
                newName: "IntegrationCode");

            migrationBuilder.RenameColumn(
                name: "SchoolLevelId",
                table: "SchoolGradeSction",
                newName: "SchoolGradeId");

            migrationBuilder.RenameIndex(
                name: "IX_SchoolGradeSction_SchoolLevelId",
                table: "SchoolGradeSction",
                newName: "IX_SchoolGradeSction_SchoolGradeId");

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "SchoolGradeSectionCourse",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                table: "SchoolGradeSction",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SectionAr",
                table: "SchoolGradeSction",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SectionEn",
                table: "SchoolGradeSction",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "SchoolGrade",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SchoolLevelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GradeLevelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_SchoolGrade", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchoolGrade_GradeLevel_GradeLevelId",
                        column: x => x.GradeLevelId,
                        principalTable: "GradeLevel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SchoolGrade_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SchoolGrade_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SchoolGrade_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SchoolGrade_SchoolLevel_SchoolLevelId",
                        column: x => x.SchoolLevelId,
                        principalTable: "SchoolLevel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SchoolGradeSectionCourse_SchoolId",
                table: "SchoolGradeSectionCourse",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolGradeSction_SchoolId",
                table: "SchoolGradeSction",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolGrade_CreateById",
                table: "SchoolGrade",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolGrade_DeleteById",
                table: "SchoolGrade",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolGrade_GradeLevelId",
                table: "SchoolGrade",
                column: "GradeLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolGrade_SchoolLevelId",
                table: "SchoolGrade",
                column: "SchoolLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolGrade_UpdateById",
                table: "SchoolGrade",
                column: "UpdateById");

            migrationBuilder.AddForeignKey(
                name: "FK_SchoolGradeSction_SchoolGrade_SchoolGradeId",
                table: "SchoolGradeSction",
                column: "SchoolGradeId",
                principalTable: "SchoolGrade",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SchoolGradeSction_Schools_SchoolId",
                table: "SchoolGradeSction",
                column: "SchoolId",
                principalTable: "Schools",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SchoolGradeSectionCourse_SchoolGradeSction_SchoolGradeSctionId",
                table: "SchoolGradeSectionCourse",
                column: "SchoolGradeSctionId",
                principalTable: "SchoolGradeSction",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SchoolGradeSectionCourse_Schools_SchoolId",
                table: "SchoolGradeSectionCourse",
                column: "SchoolId",
                principalTable: "Schools",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceRequests_SchoolGradeSction_SchoolGradeSectionId",
                table: "ServiceRequests",
                column: "SchoolGradeSectionId",
                principalTable: "SchoolGradeSction",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SchoolGradeSction_SchoolGrade_SchoolGradeId",
                table: "SchoolGradeSction");

            migrationBuilder.DropForeignKey(
                name: "FK_SchoolGradeSction_Schools_SchoolId",
                table: "SchoolGradeSction");

            migrationBuilder.DropForeignKey(
                name: "FK_SchoolGradeSectionCourse_SchoolGradeSction_SchoolGradeSctionId",
                table: "SchoolGradeSectionCourse");

            migrationBuilder.DropForeignKey(
                name: "FK_SchoolGradeSectionCourse_Schools_SchoolId",
                table: "SchoolGradeSectionCourse");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRequests_SchoolGradeSction_SchoolGradeSectionId",
                table: "ServiceRequests");

            migrationBuilder.DropTable(
                name: "SchoolGrade");

            migrationBuilder.DropIndex(
                name: "IX_SchoolGradeSectionCourse_SchoolId",
                table: "SchoolGradeSectionCourse");

            migrationBuilder.DropIndex(
                name: "IX_SchoolGradeSction_SchoolId",
                table: "SchoolGradeSction");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "SchoolGradeSectionCourse");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "SchoolGradeSction");

            migrationBuilder.DropColumn(
                name: "SectionAr",
                table: "SchoolGradeSction");

            migrationBuilder.DropColumn(
                name: "SectionEn",
                table: "SchoolGradeSction");

            migrationBuilder.RenameColumn(
                name: "SchoolGradeId",
                table: "SchoolGradeSction",
                newName: "SchoolLevelId");

            migrationBuilder.RenameColumn(
                name: "IntegrationCode",
                table: "SchoolGradeSction",
                newName: "Type");

            migrationBuilder.RenameIndex(
                name: "IX_SchoolGradeSction_SchoolGradeId",
                table: "SchoolGradeSction",
                newName: "IX_SchoolGradeSction_SchoolLevelId");

            migrationBuilder.AddColumn<Guid>(
                name: "GradeLevelId",
                table: "SchoolGradeSction",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "SchoolGradeSection",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SchoolGradeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IntegrationCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    SectionAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SectionEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
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
                name: "FK_SchoolGradeSction_SchoolLevel_SchoolLevelId",
                table: "SchoolGradeSction",
                column: "SchoolLevelId",
                principalTable: "SchoolLevel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SchoolGradeSectionCourse_SchoolGradeSection_SchoolGradeSctionId",
                table: "SchoolGradeSectionCourse",
                column: "SchoolGradeSctionId",
                principalTable: "SchoolGradeSection",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceRequests_SchoolGradeSection_SchoolGradeSectionId",
                table: "ServiceRequests",
                column: "SchoolGradeSectionId",
                principalTable: "SchoolGradeSection",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
