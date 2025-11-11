using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evaluation.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddSchoolLevel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EducationLevel_MinistryUsers_CreateById",
                table: "EducationLevel");

            migrationBuilder.DropForeignKey(
                name: "FK_EducationLevel_MinistryUsers_DeleteById",
                table: "EducationLevel");

            migrationBuilder.DropForeignKey(
                name: "FK_EducationLevel_MinistryUsers_UpdateById",
                table: "EducationLevel");

            migrationBuilder.CreateTable(
                name: "SchoolLevel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    SchoolId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EducationLevelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_SchoolLevel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchoolLevel_EducationLevel_EducationLevelId",
                        column: x => x.EducationLevelId,
                        principalTable: "EducationLevel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SchoolLevel_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SchoolLevel_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SchoolLevel_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SchoolLevel_OrgTree_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "OrgTree",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SchoolLevel_CreateById",
                table: "SchoolLevel",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolLevel_DeleteById",
                table: "SchoolLevel",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolLevel_EducationLevelId",
                table: "SchoolLevel",
                column: "EducationLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolLevel_SchoolId",
                table: "SchoolLevel",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolLevel_UpdateById",
                table: "SchoolLevel",
                column: "UpdateById");

            migrationBuilder.AddForeignKey(
                name: "FK_EducationLevel_MinistryUsers_CreateById",
                table: "EducationLevel",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EducationLevel_MinistryUsers_DeleteById",
                table: "EducationLevel",
                column: "DeleteById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EducationLevel_MinistryUsers_UpdateById",
                table: "EducationLevel",
                column: "UpdateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EducationLevel_MinistryUsers_CreateById",
                table: "EducationLevel");

            migrationBuilder.DropForeignKey(
                name: "FK_EducationLevel_MinistryUsers_DeleteById",
                table: "EducationLevel");

            migrationBuilder.DropForeignKey(
                name: "FK_EducationLevel_MinistryUsers_UpdateById",
                table: "EducationLevel");

            migrationBuilder.DropTable(
                name: "SchoolLevel");

            migrationBuilder.AddForeignKey(
                name: "FK_EducationLevel_MinistryUsers_CreateById",
                table: "EducationLevel",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EducationLevel_MinistryUsers_DeleteById",
                table: "EducationLevel",
                column: "DeleteById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EducationLevel_MinistryUsers_UpdateById",
                table: "EducationLevel",
                column: "UpdateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");
        }
    }
}
