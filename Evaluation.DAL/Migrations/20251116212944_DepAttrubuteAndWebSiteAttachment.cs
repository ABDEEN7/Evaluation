using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evaluation.DAL.Migrations
{
    /// <inheritdoc />
    public partial class DepAttrubuteAndWebSiteAttachment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RoleAttributeId",
                table: "DepartmentRoleAttributeValue",
                newName: "DepartmentRoleAttributeId");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "DepartmentRoleAttributeValue",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "DepartmentRoleAttribute",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BackendName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    table.PrimaryKey("PK_DepartmentRoleAttribute", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DepartmentRoleAttribute_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DepartmentRoleAttribute_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DepartmentRoleAttribute_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentRoleAttributeValue_DepartmentRoleAttributeId",
                table: "DepartmentRoleAttributeValue",
                column: "DepartmentRoleAttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentRoleAttribute_CreateById",
                table: "DepartmentRoleAttribute",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentRoleAttribute_DeleteById",
                table: "DepartmentRoleAttribute",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentRoleAttribute_UpdateById",
                table: "DepartmentRoleAttribute",
                column: "UpdateById");

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentRoleAttributeValue_DepartmentRoleAttribute_DepartmentRoleAttributeId",
                table: "DepartmentRoleAttributeValue",
                column: "DepartmentRoleAttributeId",
                principalTable: "DepartmentRoleAttribute",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentRoleAttributeValue_DepartmentRoleAttribute_DepartmentRoleAttributeId",
                table: "DepartmentRoleAttributeValue");

            migrationBuilder.DropTable(
                name: "DepartmentRoleAttribute");

            migrationBuilder.DropIndex(
                name: "IX_DepartmentRoleAttributeValue_DepartmentRoleAttributeId",
                table: "DepartmentRoleAttributeValue");

            migrationBuilder.RenameColumn(
                name: "DepartmentRoleAttributeId",
                table: "DepartmentRoleAttributeValue",
                newName: "RoleAttributeId");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "DepartmentRoleAttributeValue",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
