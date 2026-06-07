using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evaluation.DAL.Migrations
{
    /// <inheritdoc />
    public partial class EnhanceSchoolIntegration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SchoolTerm_SchoolClass_SchoolClassId",
                table: "SchoolTerm");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRequests_SchoolClass_SchoolClassId",
                table: "ServiceRequests");

            migrationBuilder.DropTable(
                name: "SchoolClass");

            migrationBuilder.DropIndex(
                name: "IX_ServiceRequests_SchoolClassId",
                table: "ServiceRequests");

            migrationBuilder.DropIndex(
                name: "IX_SchoolTerm_SchoolClassId",
                table: "SchoolTerm");

            migrationBuilder.DropColumn(
                name: "SchoolClassId",
                table: "SchoolTerm");

            migrationBuilder.DropColumn(
                name: "SchoolYear",
                table: "SchoolTerm");

            migrationBuilder.DropColumn(
                name: "NSISCode",
                table: "SchoolLevel");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "SchoolCourse");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "SchoolTerm",
                newName: "NameEn");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "SchoolTerm",
                newName: "NameAr");

            migrationBuilder.RenameColumn(
                name: "Year",
                table: "SchoolLevel",
                newName: "AcademicYear");

            migrationBuilder.AddColumn<string>(
                name: "BackendName",
                table: "SchoolTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IntegrationCode",
                table: "SchoolTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AcademicYear",
                table: "SchoolTerm",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TermType",
                table: "SchoolTerm",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "CloseDate",
                table: "Schools",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LATITUDE",
                table: "Schools",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LONGITUDE",
                table: "Schools",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SchoolCapacity",
                table: "Schools",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolEmpGenderId",
                table: "Schools",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolGenderId",
                table: "Schools",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolModelId",
                table: "Schools",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolProgramId",
                table: "Schools",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolStatusId",
                table: "Schools",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "URL",
                table: "Schools",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IntegrationCode",
                table: "SchoolCourse",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderNo",
                table: "SchoolCourse",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolGenderId",
                table: "OrgAcademicYears",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolModelId",
                table: "OrgAcademicYears",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolProgramId",
                table: "OrgAcademicYears",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IntegrationCode",
                table: "EducationLevel",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SchoolGender",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BackendName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderNo = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_SchoolGender", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchoolGender_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SchoolGender_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SchoolGender_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SchoolGradeSction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SchoolLevelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Grade = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_SchoolGradeSction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchoolGradeSction_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SchoolGradeSction_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SchoolGradeSction_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SchoolGradeSction_SchoolLevel_SchoolLevelId",
                        column: x => x.SchoolLevelId,
                        principalTable: "SchoolLevel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SchoolModel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BackendName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderNo = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_SchoolModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchoolModel_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SchoolModel_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SchoolModel_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SchoolProgram",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BackendName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderNo = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_SchoolProgram", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchoolProgram_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SchoolProgram_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SchoolProgram_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SchoolStatus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BackendName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderNo = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_SchoolStatus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchoolStatus_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SchoolStatus_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SchoolStatus_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SchoolGradeSectionCourse",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SchoolGradeSctionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SchoolCourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SchoolTermId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                    table.PrimaryKey("PK_SchoolGradeSectionCourse", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchoolGradeSectionCourse_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SchoolGradeSectionCourse_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SchoolGradeSectionCourse_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SchoolGradeSectionCourse_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SchoolGradeSectionCourse_SchoolCourse_SchoolCourseId",
                        column: x => x.SchoolCourseId,
                        principalTable: "SchoolCourse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SchoolGradeSectionCourse_SchoolGradeSction_SchoolGradeSctionId",
                        column: x => x.SchoolGradeSctionId,
                        principalTable: "SchoolGradeSction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SchoolGradeSectionCourse_SchoolTerm_SchoolTermId",
                        column: x => x.SchoolTermId,
                        principalTable: "SchoolTerm",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Schools_SchoolEmpGenderId",
                table: "Schools",
                column: "SchoolEmpGenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Schools_SchoolGenderId",
                table: "Schools",
                column: "SchoolGenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Schools_SchoolModelId",
                table: "Schools",
                column: "SchoolModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Schools_SchoolProgramId",
                table: "Schools",
                column: "SchoolProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_Schools_SchoolStatusId",
                table: "Schools",
                column: "SchoolStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_OrgAcademicYears_SchoolGenderId",
                table: "OrgAcademicYears",
                column: "SchoolGenderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrgAcademicYears_SchoolModelId",
                table: "OrgAcademicYears",
                column: "SchoolModelId");

            migrationBuilder.CreateIndex(
                name: "IX_OrgAcademicYears_SchoolProgramId",
                table: "OrgAcademicYears",
                column: "SchoolProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolGender_CreateById",
                table: "SchoolGender",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolGender_DeleteById",
                table: "SchoolGender",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolGender_UpdateById",
                table: "SchoolGender",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolGradeSction_CreateById",
                table: "SchoolGradeSction",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolGradeSction_DeleteById",
                table: "SchoolGradeSction",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolGradeSction_SchoolLevelId",
                table: "SchoolGradeSction",
                column: "SchoolLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolGradeSction_UpdateById",
                table: "SchoolGradeSction",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolGradeSectionCourse_CreateById",
                table: "SchoolGradeSectionCourse",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolGradeSectionCourse_DeleteById",
                table: "SchoolGradeSectionCourse",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolGradeSectionCourse_EmployeeId",
                table: "SchoolGradeSectionCourse",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolGradeSectionCourse_SchoolCourseId",
                table: "SchoolGradeSectionCourse",
                column: "SchoolCourseId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolGradeSectionCourse_SchoolGradeSctionId",
                table: "SchoolGradeSectionCourse",
                column: "SchoolGradeSctionId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolGradeSectionCourse_SchoolTermId",
                table: "SchoolGradeSectionCourse",
                column: "SchoolTermId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolGradeSectionCourse_UpdateById",
                table: "SchoolGradeSectionCourse",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolModel_CreateById",
                table: "SchoolModel",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolModel_DeleteById",
                table: "SchoolModel",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolModel_UpdateById",
                table: "SchoolModel",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolProgram_CreateById",
                table: "SchoolProgram",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolProgram_DeleteById",
                table: "SchoolProgram",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolProgram_UpdateById",
                table: "SchoolProgram",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolStatus_CreateById",
                table: "SchoolStatus",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolStatus_DeleteById",
                table: "SchoolStatus",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolStatus_UpdateById",
                table: "SchoolStatus",
                column: "UpdateById");

            migrationBuilder.AddForeignKey(
                name: "FK_OrgAcademicYears_SchoolGender_SchoolGenderId",
                table: "OrgAcademicYears",
                column: "SchoolGenderId",
                principalTable: "SchoolGender",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrgAcademicYears_SchoolModel_SchoolModelId",
                table: "OrgAcademicYears",
                column: "SchoolModelId",
                principalTable: "SchoolModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrgAcademicYears_SchoolProgram_SchoolProgramId",
                table: "OrgAcademicYears",
                column: "SchoolProgramId",
                principalTable: "SchoolProgram",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Schools_SchoolGender_SchoolEmpGenderId",
                table: "Schools",
                column: "SchoolEmpGenderId",
                principalTable: "SchoolGender",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Schools_SchoolGender_SchoolGenderId",
                table: "Schools",
                column: "SchoolGenderId",
                principalTable: "SchoolGender",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Schools_SchoolModel_SchoolModelId",
                table: "Schools",
                column: "SchoolModelId",
                principalTable: "SchoolModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Schools_SchoolProgram_SchoolProgramId",
                table: "Schools",
                column: "SchoolProgramId",
                principalTable: "SchoolProgram",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Schools_SchoolStatus_SchoolStatusId",
                table: "Schools",
                column: "SchoolStatusId",
                principalTable: "SchoolStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrgAcademicYears_SchoolGender_SchoolGenderId",
                table: "OrgAcademicYears");

            migrationBuilder.DropForeignKey(
                name: "FK_OrgAcademicYears_SchoolModel_SchoolModelId",
                table: "OrgAcademicYears");

            migrationBuilder.DropForeignKey(
                name: "FK_OrgAcademicYears_SchoolProgram_SchoolProgramId",
                table: "OrgAcademicYears");

            migrationBuilder.DropForeignKey(
                name: "FK_Schools_SchoolGender_SchoolEmpGenderId",
                table: "Schools");

            migrationBuilder.DropForeignKey(
                name: "FK_Schools_SchoolGender_SchoolGenderId",
                table: "Schools");

            migrationBuilder.DropForeignKey(
                name: "FK_Schools_SchoolModel_SchoolModelId",
                table: "Schools");

            migrationBuilder.DropForeignKey(
                name: "FK_Schools_SchoolProgram_SchoolProgramId",
                table: "Schools");

            migrationBuilder.DropForeignKey(
                name: "FK_Schools_SchoolStatus_SchoolStatusId",
                table: "Schools");

            migrationBuilder.DropTable(
                name: "SchoolGender");

            migrationBuilder.DropTable(
                name: "SchoolGradeSectionCourse");

            migrationBuilder.DropTable(
                name: "SchoolModel");

            migrationBuilder.DropTable(
                name: "SchoolProgram");

            migrationBuilder.DropTable(
                name: "SchoolStatus");

            migrationBuilder.DropTable(
                name: "SchoolGradeSction");

            migrationBuilder.DropIndex(
                name: "IX_Schools_SchoolEmpGenderId",
                table: "Schools");

            migrationBuilder.DropIndex(
                name: "IX_Schools_SchoolGenderId",
                table: "Schools");

            migrationBuilder.DropIndex(
                name: "IX_Schools_SchoolModelId",
                table: "Schools");

            migrationBuilder.DropIndex(
                name: "IX_Schools_SchoolProgramId",
                table: "Schools");

            migrationBuilder.DropIndex(
                name: "IX_Schools_SchoolStatusId",
                table: "Schools");

            migrationBuilder.DropIndex(
                name: "IX_OrgAcademicYears_SchoolGenderId",
                table: "OrgAcademicYears");

            migrationBuilder.DropIndex(
                name: "IX_OrgAcademicYears_SchoolModelId",
                table: "OrgAcademicYears");

            migrationBuilder.DropIndex(
                name: "IX_OrgAcademicYears_SchoolProgramId",
                table: "OrgAcademicYears");

            migrationBuilder.DropColumn(
                name: "BackendName",
                table: "SchoolTypes");

            migrationBuilder.DropColumn(
                name: "IntegrationCode",
                table: "SchoolTypes");

            migrationBuilder.DropColumn(
                name: "AcademicYear",
                table: "SchoolTerm");

            migrationBuilder.DropColumn(
                name: "TermType",
                table: "SchoolTerm");

            migrationBuilder.DropColumn(
                name: "CloseDate",
                table: "Schools");

            migrationBuilder.DropColumn(
                name: "LATITUDE",
                table: "Schools");

            migrationBuilder.DropColumn(
                name: "LONGITUDE",
                table: "Schools");

            migrationBuilder.DropColumn(
                name: "SchoolCapacity",
                table: "Schools");

            migrationBuilder.DropColumn(
                name: "SchoolEmpGenderId",
                table: "Schools");

            migrationBuilder.DropColumn(
                name: "SchoolGenderId",
                table: "Schools");

            migrationBuilder.DropColumn(
                name: "SchoolModelId",
                table: "Schools");

            migrationBuilder.DropColumn(
                name: "SchoolProgramId",
                table: "Schools");

            migrationBuilder.DropColumn(
                name: "SchoolStatusId",
                table: "Schools");

            migrationBuilder.DropColumn(
                name: "URL",
                table: "Schools");

            migrationBuilder.DropColumn(
                name: "IntegrationCode",
                table: "SchoolCourse");

            migrationBuilder.DropColumn(
                name: "OrderNo",
                table: "SchoolCourse");

            migrationBuilder.DropColumn(
                name: "SchoolGenderId",
                table: "OrgAcademicYears");

            migrationBuilder.DropColumn(
                name: "SchoolModelId",
                table: "OrgAcademicYears");

            migrationBuilder.DropColumn(
                name: "SchoolProgramId",
                table: "OrgAcademicYears");

            migrationBuilder.DropColumn(
                name: "IntegrationCode",
                table: "EducationLevel");

            migrationBuilder.RenameColumn(
                name: "NameEn",
                table: "SchoolTerm",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "NameAr",
                table: "SchoolTerm",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "AcademicYear",
                table: "SchoolLevel",
                newName: "Year");

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolClassId",
                table: "SchoolTerm",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SchoolYear",
                table: "SchoolTerm",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NSISCode",
                table: "SchoolLevel",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "SchoolCourse",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "SchoolClass",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SchoolId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    Grade = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolClass", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchoolClass_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SchoolClass_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SchoolClass_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SchoolClass_SchoolCourse_CourseId",
                        column: x => x.CourseId,
                        principalTable: "SchoolCourse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SchoolClass_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequests_SchoolClassId",
                table: "ServiceRequests",
                column: "SchoolClassId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolTerm_SchoolClassId",
                table: "SchoolTerm",
                column: "SchoolClassId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolClass_CourseId",
                table: "SchoolClass",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolClass_CreateById",
                table: "SchoolClass",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolClass_DeleteById",
                table: "SchoolClass",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolClass_SchoolId",
                table: "SchoolClass",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolClass_UpdateById",
                table: "SchoolClass",
                column: "UpdateById");

            migrationBuilder.AddForeignKey(
                name: "FK_SchoolTerm_SchoolClass_SchoolClassId",
                table: "SchoolTerm",
                column: "SchoolClassId",
                principalTable: "SchoolClass",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceRequests_SchoolClass_SchoolClassId",
                table: "ServiceRequests",
                column: "SchoolClassId",
                principalTable: "SchoolClass",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
