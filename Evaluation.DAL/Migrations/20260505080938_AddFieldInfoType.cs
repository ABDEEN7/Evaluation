using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evaluation.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldInfoType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FieldInfoTypeId",
                table: "Field",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FieldInfoType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BackendName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderNo = table.Column<int>(type: "int", nullable: true),
                    SystemModuleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_FieldInfoType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FieldInfoType_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FieldInfoType_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FieldInfoType_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FieldInfoType_SystemModules_SystemModuleId",
                        column: x => x.SystemModuleId,
                        principalTable: "SystemModules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Field_FieldInfoTypeId",
                table: "Field",
                column: "FieldInfoTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldInfoType_CreateById",
                table: "FieldInfoType",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_FieldInfoType_DeleteById",
                table: "FieldInfoType",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_FieldInfoType_SystemModuleId",
                table: "FieldInfoType",
                column: "SystemModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldInfoType_UpdateById",
                table: "FieldInfoType",
                column: "UpdateById");

            migrationBuilder.AddForeignKey(
                name: "FK_Field_FieldInfoType_FieldInfoTypeId",
                table: "Field",
                column: "FieldInfoTypeId",
                principalTable: "FieldInfoType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Field_FieldInfoType_FieldInfoTypeId",
                table: "Field");

            migrationBuilder.DropTable(
                name: "FieldInfoType");

            migrationBuilder.DropIndex(
                name: "IX_Field_FieldInfoTypeId",
                table: "Field");

            migrationBuilder.DropColumn(
                name: "FieldInfoTypeId",
                table: "Field");
        }
    }
}
