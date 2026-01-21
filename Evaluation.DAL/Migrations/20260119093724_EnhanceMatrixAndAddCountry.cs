using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evaluation.DAL.Migrations
{
    /// <inheritdoc />
    public partial class EnhanceMatrixAndAddCountry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FormEvalMarixValue");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FormItemRelated",
                table: "FormItemRelated");

            migrationBuilder.DropIndex(
                name: "IX_FormItemRelated_FormItemId",
                table: "FormItemRelated");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "FormItemValues",
                newName: "ActualValue");

            migrationBuilder.RenameColumn(
                name: "DepEvalMatrixValue",
                table: "FormItemValues",
                newName: "DepEvalMatrixValueName");

            migrationBuilder.AddColumn<Guid>(
                name: "FormEvalMatrixValueId",
                table: "FormItemValues",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FormItemRelated",
                table: "FormItemRelated",
                columns: new[] { "FormItemId", "RelatedItemId" });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CodeAlpha = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderNo = table.Column<int>(type: "int", nullable: true),
                    BackendName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ISOCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LogoAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LogoEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_Countries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Countries_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Countries_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Countries_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DepartmentWebGroup",
                columns: table => new
                {
                    DepartmentsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WebGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepartmentWebGroup", x => new { x.DepartmentsId, x.WebGroupId });
                    table.ForeignKey(
                        name: "FK_DepartmentWebGroup_Departments_DepartmentsId",
                        column: x => x.DepartmentsId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DepartmentWebGroup_WebGroups_WebGroupId",
                        column: x => x.WebGroupId,
                        principalTable: "WebGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FormEvalMatrixValue",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FormEvalMatrixId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MinValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaxValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ActualMatrixValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DescAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderNo = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_FormEvalMatrixValue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormEvalMatrixValue_FormEvalMatrix_FormEvalMatrixId",
                        column: x => x.FormEvalMatrixId,
                        principalTable: "FormEvalMatrix",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FormEvalMatrixValue_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FormEvalMatrixValue_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FormEvalMatrixValue_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FormItemValues_FormEvalMatrixValueId",
                table: "FormItemValues",
                column: "FormEvalMatrixValueId");

            migrationBuilder.CreateIndex(
                name: "IX_Countries_CreateById",
                table: "Countries",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_Countries_DeleteById",
                table: "Countries",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_Countries_UpdateById",
                table: "Countries",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentWebGroup_WebGroupId",
                table: "DepartmentWebGroup",
                column: "WebGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_FormEvalMatrixValue_CreateById",
                table: "FormEvalMatrixValue",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_FormEvalMatrixValue_DeleteById",
                table: "FormEvalMatrixValue",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_FormEvalMatrixValue_FormEvalMatrixId",
                table: "FormEvalMatrixValue",
                column: "FormEvalMatrixId");

            migrationBuilder.CreateIndex(
                name: "IX_FormEvalMatrixValue_UpdateById",
                table: "FormEvalMatrixValue",
                column: "UpdateById");

            migrationBuilder.AddForeignKey(
                name: "FK_FormItemValues_FormEvalMatrixValue_FormEvalMatrixValueId",
                table: "FormItemValues",
                column: "FormEvalMatrixValueId",
                principalTable: "FormEvalMatrixValue",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormItemValues_FormEvalMatrixValue_FormEvalMatrixValueId",
                table: "FormItemValues");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "DepartmentWebGroup");

            migrationBuilder.DropTable(
                name: "FormEvalMatrixValue");

            migrationBuilder.DropIndex(
                name: "IX_FormItemValues_FormEvalMatrixValueId",
                table: "FormItemValues");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FormItemRelated",
                table: "FormItemRelated");

            migrationBuilder.DropColumn(
                name: "FormEvalMatrixValueId",
                table: "FormItemValues");

            migrationBuilder.RenameColumn(
                name: "DepEvalMatrixValueName",
                table: "FormItemValues",
                newName: "DepEvalMatrixValue");

            migrationBuilder.RenameColumn(
                name: "ActualValue",
                table: "FormItemValues",
                newName: "Value");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FormItemRelated",
                table: "FormItemRelated",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "FormEvalMarixValue",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FormEvalMatrixId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DescAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0"),
                    MaxValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MinValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderNo = table.Column<int>(type: "int", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormEvalMarixValue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormEvalMarixValue_FormEvalMatrix_FormEvalMatrixId",
                        column: x => x.FormEvalMatrixId,
                        principalTable: "FormEvalMatrix",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FormEvalMarixValue_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FormEvalMarixValue_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FormEvalMarixValue_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FormItemRelated_FormItemId",
                table: "FormItemRelated",
                column: "FormItemId");

            migrationBuilder.CreateIndex(
                name: "IX_FormEvalMarixValue_CreateById",
                table: "FormEvalMarixValue",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_FormEvalMarixValue_DeleteById",
                table: "FormEvalMarixValue",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_FormEvalMarixValue_FormEvalMatrixId",
                table: "FormEvalMarixValue",
                column: "FormEvalMatrixId");

            migrationBuilder.CreateIndex(
                name: "IX_FormEvalMarixValue_UpdateById",
                table: "FormEvalMarixValue",
                column: "UpdateById");
        }
    }
}
