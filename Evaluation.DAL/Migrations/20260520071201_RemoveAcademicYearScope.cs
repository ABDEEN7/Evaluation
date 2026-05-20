using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evaluation.DAL.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAcademicYearScope : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AcademicYearScope");

            migrationBuilder.AddColumn<int>(
                name: "OrderNo",
                table: "ScopeAcademicYears",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderNo",
                table: "ScopeAcademicYears");

            migrationBuilder.CreateTable(
                name: "AcademicYearScope",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AcademicYearId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ScopeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademicYearScope", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AcademicYearScope_AcademicYears_AcademicYearId",
                        column: x => x.AcademicYearId,
                        principalTable: "AcademicYears",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AcademicYearScope_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AcademicYearScope_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AcademicYearScope_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AcademicYearScope_Scopes_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Scopes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AcademicYearScope_Scopes_ScopeId",
                        column: x => x.ScopeId,
                        principalTable: "Scopes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AcademicYearScope_AcademicYearId",
                table: "AcademicYearScope",
                column: "AcademicYearId");

            migrationBuilder.CreateIndex(
                name: "IX_AcademicYearScope_CreateById",
                table: "AcademicYearScope",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_AcademicYearScope_DeleteById",
                table: "AcademicYearScope",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_AcademicYearScope_ParentId",
                table: "AcademicYearScope",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_AcademicYearScope_ScopeId",
                table: "AcademicYearScope",
                column: "ScopeId");

            migrationBuilder.CreateIndex(
                name: "IX_AcademicYearScope_UpdateById",
                table: "AcademicYearScope",
                column: "UpdateById");
        }
    }
}
