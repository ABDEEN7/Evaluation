using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evaluation.DAL.Migrations
{
    /// <inheritdoc />
    public partial class EnhanceFormItemAndRenameItemValue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormItem_CalcMethods_CalcMethodId",
                table: "FormItem");

            migrationBuilder.DropIndex(
                name: "IX_FormItem_CalcMethodId",
                table: "FormItem");

            migrationBuilder.DropColumn(
                name: "CalcMethodId",
                table: "FormItem");

            migrationBuilder.AddColumn<string>(
                name: "ColorCode",
                table: "ScopeTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ColorCode",
                table: "Scopes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "FormItemConfigId",
                table: "FormItemValues",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RenameItem",
                table: "FormItemValues",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ColorCode",
                table: "FormItem",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AllowRename",
                table: "EvalForms",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "FormItemConfig",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EvalFormId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FormItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PartyTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CalcMethodId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Percentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
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
                    table.PrimaryKey("PK_FormItemConfig", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormItemConfig_CalcMethods_CalcMethodId",
                        column: x => x.CalcMethodId,
                        principalTable: "CalcMethods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FormItemConfig_EvalForms_EvalFormId",
                        column: x => x.EvalFormId,
                        principalTable: "EvalForms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FormItemConfig_FormItem_FormItemId",
                        column: x => x.FormItemId,
                        principalTable: "FormItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FormItemConfig_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FormItemConfig_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FormItemConfig_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FormItemConfig_PartyTypes_PartyTypeId",
                        column: x => x.PartyTypeId,
                        principalTable: "PartyTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ServiceType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BackendName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    table.PrimaryKey("PK_ServiceType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceType_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceType_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ServiceType_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FormItemValues_FormItemConfigId",
                table: "FormItemValues",
                column: "FormItemConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_FormItemConfig_CalcMethodId",
                table: "FormItemConfig",
                column: "CalcMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_FormItemConfig_CreateById",
                table: "FormItemConfig",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_FormItemConfig_DeleteById",
                table: "FormItemConfig",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_FormItemConfig_EvalFormId",
                table: "FormItemConfig",
                column: "EvalFormId");

            migrationBuilder.CreateIndex(
                name: "IX_FormItemConfig_FormItemId",
                table: "FormItemConfig",
                column: "FormItemId");

            migrationBuilder.CreateIndex(
                name: "IX_FormItemConfig_PartyTypeId",
                table: "FormItemConfig",
                column: "PartyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FormItemConfig_UpdateById",
                table: "FormItemConfig",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceType_CreateById",
                table: "ServiceType",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceType_DeleteById",
                table: "ServiceType",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceType_UpdateById",
                table: "ServiceType",
                column: "UpdateById");

            migrationBuilder.AddForeignKey(
                name: "FK_FormItemValues_FormItemConfig_FormItemConfigId",
                table: "FormItemValues",
                column: "FormItemConfigId",
                principalTable: "FormItemConfig",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormItemValues_FormItemConfig_FormItemConfigId",
                table: "FormItemValues");

            migrationBuilder.DropTable(
                name: "FormItemConfig");

            migrationBuilder.DropTable(
                name: "ServiceType");

            migrationBuilder.DropIndex(
                name: "IX_FormItemValues_FormItemConfigId",
                table: "FormItemValues");

            migrationBuilder.DropColumn(
                name: "ColorCode",
                table: "ScopeTypes");

            migrationBuilder.DropColumn(
                name: "ColorCode",
                table: "Scopes");

            migrationBuilder.DropColumn(
                name: "FormItemConfigId",
                table: "FormItemValues");

            migrationBuilder.DropColumn(
                name: "RenameItem",
                table: "FormItemValues");

            migrationBuilder.DropColumn(
                name: "ColorCode",
                table: "FormItem");

            migrationBuilder.DropColumn(
                name: "AllowRename",
                table: "EvalForms");

            migrationBuilder.AddColumn<Guid>(
                name: "CalcMethodId",
                table: "FormItem",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_FormItem_CalcMethodId",
                table: "FormItem",
                column: "CalcMethodId");

            migrationBuilder.AddForeignKey(
                name: "FK_FormItem_CalcMethods_CalcMethodId",
                table: "FormItem",
                column: "CalcMethodId",
                principalTable: "CalcMethods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
