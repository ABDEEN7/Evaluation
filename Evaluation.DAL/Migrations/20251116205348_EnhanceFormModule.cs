using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evaluation.DAL.Migrations
{
    /// <inheritdoc />
    public partial class EnhanceFormModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FieldDropDownValue_DropDownType_DropDownTypeId",
                table: "FieldDropDownValue");

            migrationBuilder.DropForeignKey(
                name: "FK_FieldDropDownValue_FieldDropDownValue_ParentDropDownId",
                table: "FieldDropDownValue");

            migrationBuilder.DropForeignKey(
                name: "FK_FieldDropDownValue_MinistryUsers_CreateById",
                table: "FieldDropDownValue");

            migrationBuilder.DropForeignKey(
                name: "FK_FieldDropDownValue_MinistryUsers_DeleteById",
                table: "FieldDropDownValue");

            migrationBuilder.DropForeignKey(
                name: "FK_FieldDropDownValue_MinistryUsers_UpdateById",
                table: "FieldDropDownValue");

            migrationBuilder.DropForeignKey(
                name: "FK_FormScopes_Forms_FormId",
                table: "FormScopes");

            migrationBuilder.DropTable(
                name: "Forms");

            migrationBuilder.DropTable(
                name: "ItemValues");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.RenameColumn(
                name: "FormId",
                table: "FormScopes",
                newName: "EvalFormId");

            migrationBuilder.RenameIndex(
                name: "IX_FormScopes_FormId",
                table: "FormScopes",
                newName: "IX_FormScopes_EvalFormId");

            migrationBuilder.AddColumn<Guid>(
                name: "DepartmentId",
                table: "UiControls",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BackendName",
                table: "FormStatus",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<decimal>(
                name: "Wegiht",
                table: "FormScopes",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "EvalFormType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BackendName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    table.PrimaryKey("PK_EvalFormType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EvalFormType_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EvalFormType_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EvalFormType_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FormEvalMarix",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BackenName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Startdate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: true),
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
                    table.PrimaryKey("PK_FormEvalMarix", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormEvalMarix_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FormEvalMarix_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FormEvalMarix_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FormEvalMarix_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EvalForms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EvalFormTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FormEvalMarixId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRopric = table.Column<bool>(type: "bit", nullable: false),
                    HasOneValue = table.Column<bool>(type: "bit", nullable: false),
                    EvaluationPartyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HasEvaluation = table.Column<bool>(type: "bit", nullable: false),
                    CalcMethodId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FormStatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_EvalForms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EvalForms_CalcMethods_CalcMethodId",
                        column: x => x.CalcMethodId,
                        principalTable: "CalcMethods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EvalForms_EvalFormType_EvalFormTypeId",
                        column: x => x.EvalFormTypeId,
                        principalTable: "EvalFormType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EvalForms_EvaluationParties_EvaluationPartyId",
                        column: x => x.EvaluationPartyId,
                        principalTable: "EvaluationParties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EvalForms_FormEvalMarix_FormEvalMarixId",
                        column: x => x.FormEvalMarixId,
                        principalTable: "FormEvalMarix",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EvalForms_FormStatus_FormStatusId",
                        column: x => x.FormStatusId,
                        principalTable: "FormStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EvalForms_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EvalForms_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EvalForms_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FormEvalMarixValue",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FormEvalMarixid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MinValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaxValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
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
                    table.PrimaryKey("PK_FormEvalMarixValue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormEvalMarixValue_FormEvalMarix_FormEvalMarixid",
                        column: x => x.FormEvalMarixid,
                        principalTable: "FormEvalMarix",
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

            migrationBuilder.CreateTable(
                name: "FormItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Min = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Max = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsEvaluation = table.Column<bool>(type: "bit", nullable: false),
                    Weight = table.Column<int>(type: "int", nullable: false),
                    EvalFormId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScopeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CalcMethodId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_FormItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormItem_CalcMethods_CalcMethodId",
                        column: x => x.CalcMethodId,
                        principalTable: "CalcMethods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FormItem_EvalForms_EvalFormId",
                        column: x => x.EvalFormId,
                        principalTable: "EvalForms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FormItem_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FormItem_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FormItem_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FormItem_Scopes_ScopeId",
                        column: x => x.ScopeId,
                        principalTable: "Scopes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FormItemValues",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FormItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_FormItemValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormItemValues_FormItem_FormItemId",
                        column: x => x.FormItemId,
                        principalTable: "FormItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FormItemValues_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FormItemValues_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FormItemValues_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FormItemValues_MinistryUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubFormItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FormItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsOption = table.Column<bool>(type: "bit", nullable: false),
                    DropDownTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                    table.PrimaryKey("PK_SubFormItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubFormItems_DropDownType_DropDownTypeId",
                        column: x => x.DropDownTypeId,
                        principalTable: "DropDownType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubFormItems_FormItem_FormItemId",
                        column: x => x.FormItemId,
                        principalTable: "FormItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubFormItems_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubFormItems_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubFormItems_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubFormItemValues",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubFormItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FieldDropDownValueId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_SubFormItemValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubFormItemValues_FieldDropDownValue_FieldDropDownValueId",
                        column: x => x.FieldDropDownValueId,
                        principalTable: "FieldDropDownValue",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubFormItemValues_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubFormItemValues_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubFormItemValues_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubFormItemValues_MinistryUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubFormItemValues_SubFormItems_SubFormItemId",
                        column: x => x.SubFormItemId,
                        principalTable: "SubFormItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UiControls_DepartmentId",
                table: "UiControls",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_EvalForms_CalcMethodId",
                table: "EvalForms",
                column: "CalcMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_EvalForms_CreateById",
                table: "EvalForms",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_EvalForms_DeleteById",
                table: "EvalForms",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_EvalForms_EvalFormTypeId",
                table: "EvalForms",
                column: "EvalFormTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_EvalForms_EvaluationPartyId",
                table: "EvalForms",
                column: "EvaluationPartyId");

            migrationBuilder.CreateIndex(
                name: "IX_EvalForms_FormEvalMarixId",
                table: "EvalForms",
                column: "FormEvalMarixId");

            migrationBuilder.CreateIndex(
                name: "IX_EvalForms_FormStatusId",
                table: "EvalForms",
                column: "FormStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_EvalForms_UpdateById",
                table: "EvalForms",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_EvalFormType_CreateById",
                table: "EvalFormType",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_EvalFormType_DeleteById",
                table: "EvalFormType",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_EvalFormType_UpdateById",
                table: "EvalFormType",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_FormEvalMarix_CreateById",
                table: "FormEvalMarix",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_FormEvalMarix_DeleteById",
                table: "FormEvalMarix",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_FormEvalMarix_DepartmentId",
                table: "FormEvalMarix",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_FormEvalMarix_UpdateById",
                table: "FormEvalMarix",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_FormEvalMarixValue_CreateById",
                table: "FormEvalMarixValue",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_FormEvalMarixValue_DeleteById",
                table: "FormEvalMarixValue",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_FormEvalMarixValue_FormEvalMarixid",
                table: "FormEvalMarixValue",
                column: "FormEvalMarixid");

            migrationBuilder.CreateIndex(
                name: "IX_FormEvalMarixValue_UpdateById",
                table: "FormEvalMarixValue",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_FormItem_CalcMethodId",
                table: "FormItem",
                column: "CalcMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_FormItem_CreateById",
                table: "FormItem",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_FormItem_DeleteById",
                table: "FormItem",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_FormItem_EvalFormId",
                table: "FormItem",
                column: "EvalFormId");

            migrationBuilder.CreateIndex(
                name: "IX_FormItem_ScopeId",
                table: "FormItem",
                column: "ScopeId");

            migrationBuilder.CreateIndex(
                name: "IX_FormItem_UpdateById",
                table: "FormItem",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_FormItemValues_CreateById",
                table: "FormItemValues",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_FormItemValues_DeleteById",
                table: "FormItemValues",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_FormItemValues_FormItemId",
                table: "FormItemValues",
                column: "FormItemId");

            migrationBuilder.CreateIndex(
                name: "IX_FormItemValues_UpdateById",
                table: "FormItemValues",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_FormItemValues_UserId",
                table: "FormItemValues",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SubFormItems_CreateById",
                table: "SubFormItems",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_SubFormItems_DeleteById",
                table: "SubFormItems",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_SubFormItems_DropDownTypeId",
                table: "SubFormItems",
                column: "DropDownTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SubFormItems_FormItemId",
                table: "SubFormItems",
                column: "FormItemId");

            migrationBuilder.CreateIndex(
                name: "IX_SubFormItems_UpdateById",
                table: "SubFormItems",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_SubFormItemValues_CreateById",
                table: "SubFormItemValues",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_SubFormItemValues_DeleteById",
                table: "SubFormItemValues",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_SubFormItemValues_FieldDropDownValueId",
                table: "SubFormItemValues",
                column: "FieldDropDownValueId");

            migrationBuilder.CreateIndex(
                name: "IX_SubFormItemValues_SubFormItemId",
                table: "SubFormItemValues",
                column: "SubFormItemId");

            migrationBuilder.CreateIndex(
                name: "IX_SubFormItemValues_UpdateById",
                table: "SubFormItemValues",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_SubFormItemValues_UserId",
                table: "SubFormItemValues",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_FieldDropDownValue_DropDownType_DropDownTypeId",
                table: "FieldDropDownValue",
                column: "DropDownTypeId",
                principalTable: "DropDownType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FieldDropDownValue_FieldDropDownValue_ParentDropDownId",
                table: "FieldDropDownValue",
                column: "ParentDropDownId",
                principalTable: "FieldDropDownValue",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FieldDropDownValue_MinistryUsers_CreateById",
                table: "FieldDropDownValue",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FieldDropDownValue_MinistryUsers_DeleteById",
                table: "FieldDropDownValue",
                column: "DeleteById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FieldDropDownValue_MinistryUsers_UpdateById",
                table: "FieldDropDownValue",
                column: "UpdateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FormScopes_EvalForms_EvalFormId",
                table: "FormScopes",
                column: "EvalFormId",
                principalTable: "EvalForms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UiControls_Departments_DepartmentId",
                table: "UiControls",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FieldDropDownValue_DropDownType_DropDownTypeId",
                table: "FieldDropDownValue");

            migrationBuilder.DropForeignKey(
                name: "FK_FieldDropDownValue_FieldDropDownValue_ParentDropDownId",
                table: "FieldDropDownValue");

            migrationBuilder.DropForeignKey(
                name: "FK_FieldDropDownValue_MinistryUsers_CreateById",
                table: "FieldDropDownValue");

            migrationBuilder.DropForeignKey(
                name: "FK_FieldDropDownValue_MinistryUsers_DeleteById",
                table: "FieldDropDownValue");

            migrationBuilder.DropForeignKey(
                name: "FK_FieldDropDownValue_MinistryUsers_UpdateById",
                table: "FieldDropDownValue");

            migrationBuilder.DropForeignKey(
                name: "FK_FormScopes_EvalForms_EvalFormId",
                table: "FormScopes");

            migrationBuilder.DropForeignKey(
                name: "FK_UiControls_Departments_DepartmentId",
                table: "UiControls");

            migrationBuilder.DropTable(
                name: "FormEvalMarixValue");

            migrationBuilder.DropTable(
                name: "FormItemValues");

            migrationBuilder.DropTable(
                name: "SubFormItemValues");

            migrationBuilder.DropTable(
                name: "SubFormItems");

            migrationBuilder.DropTable(
                name: "FormItem");

            migrationBuilder.DropTable(
                name: "EvalForms");

            migrationBuilder.DropTable(
                name: "EvalFormType");

            migrationBuilder.DropTable(
                name: "FormEvalMarix");

            migrationBuilder.DropIndex(
                name: "IX_UiControls_DepartmentId",
                table: "UiControls");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "UiControls");

            migrationBuilder.DropColumn(
                name: "BackendName",
                table: "FormStatus");

            migrationBuilder.RenameColumn(
                name: "EvalFormId",
                table: "FormScopes",
                newName: "FormId");

            migrationBuilder.RenameIndex(
                name: "IX_FormScopes_EvalFormId",
                table: "FormScopes",
                newName: "IX_FormScopes_FormId");

            migrationBuilder.AlterColumn<int>(
                name: "Wegiht",
                table: "FormScopes",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.CreateTable(
                name: "Forms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CalcMethodId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EvaluationPartyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FormStatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HasEvaluation = table.Column<bool>(type: "bit", nullable: false),
                    HasOneValue = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0"),
                    IsRopric = table.Column<bool>(type: "bit", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Forms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Forms_CalcMethods_CalcMethodId",
                        column: x => x.CalcMethodId,
                        principalTable: "CalcMethods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Forms_EvaluationParties_EvaluationPartyId",
                        column: x => x.EvaluationPartyId,
                        principalTable: "EvaluationParties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Forms_FormStatus_FormStatusId",
                        column: x => x.FormStatusId,
                        principalTable: "FormStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Forms_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Forms_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Forms_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CalcMethodId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DropDownType = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FormId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0"),
                    IsEvaluation = table.Column<bool>(type: "bit", nullable: false),
                    Max = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Min = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ScopeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Weight = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Items_CalcMethods_CalcMethodId",
                        column: x => x.CalcMethodId,
                        principalTable: "CalcMethods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Items_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Items_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Items_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ItemValues",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0"),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemValues_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItemValues_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItemValues_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItemValues_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItemValues_MinistryUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Forms_CalcMethodId",
                table: "Forms",
                column: "CalcMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_Forms_CreateById",
                table: "Forms",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_Forms_DeleteById",
                table: "Forms",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_Forms_EvaluationPartyId",
                table: "Forms",
                column: "EvaluationPartyId");

            migrationBuilder.CreateIndex(
                name: "IX_Forms_FormStatusId",
                table: "Forms",
                column: "FormStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Forms_UpdateById",
                table: "Forms",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_Items_CalcMethodId",
                table: "Items",
                column: "CalcMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_CreateById",
                table: "Items",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_Items_DeleteById",
                table: "Items",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_Items_UpdateById",
                table: "Items",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_ItemValues_CreateById",
                table: "ItemValues",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_ItemValues_DeleteById",
                table: "ItemValues",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_ItemValues_ItemId",
                table: "ItemValues",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemValues_UpdateById",
                table: "ItemValues",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_ItemValues_UserId",
                table: "ItemValues",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_FieldDropDownValue_DropDownType_DropDownTypeId",
                table: "FieldDropDownValue",
                column: "DropDownTypeId",
                principalTable: "DropDownType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FieldDropDownValue_FieldDropDownValue_ParentDropDownId",
                table: "FieldDropDownValue",
                column: "ParentDropDownId",
                principalTable: "FieldDropDownValue",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FieldDropDownValue_MinistryUsers_CreateById",
                table: "FieldDropDownValue",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FieldDropDownValue_MinistryUsers_DeleteById",
                table: "FieldDropDownValue",
                column: "DeleteById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FieldDropDownValue_MinistryUsers_UpdateById",
                table: "FieldDropDownValue",
                column: "UpdateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FormScopes_Forms_FormId",
                table: "FormScopes",
                column: "FormId",
                principalTable: "Forms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
