using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evaluation.DAL.Migrations
{
    /// <inheritdoc />
    public partial class EnhanceFormModuleAndDep : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EvalForms_FormEvalMarix_FormEvalMarixId",
                table: "EvalForms");

            migrationBuilder.DropForeignKey(
                name: "FK_FormEvalMarixValue_FormEvalMarix_FormEvalMarixid",
                table: "FormEvalMarixValue");

            migrationBuilder.DropForeignKey(
                name: "FK_WebsiteAttachment_MinistryUsers_CreateById",
                table: "WebsiteAttachment");

            migrationBuilder.DropForeignKey(
                name: "FK_WebsiteAttachment_MinistryUsers_DeleteById",
                table: "WebsiteAttachment");

            migrationBuilder.DropForeignKey(
                name: "FK_WebsiteAttachment_MinistryUsers_UpdateById",
                table: "WebsiteAttachment");

            migrationBuilder.DropTable(
                name: "FormEvalMarix");

            migrationBuilder.DropTable(
                name: "RequestAssign");

            migrationBuilder.RenameColumn(
                name: "FormEvalMarixid",
                table: "FormEvalMarixValue",
                newName: "FormEvalMatrixId");

            migrationBuilder.RenameIndex(
                name: "IX_FormEvalMarixValue_FormEvalMarixid",
                table: "FormEvalMarixValue",
                newName: "IX_FormEvalMarixValue_FormEvalMatrixId");

            migrationBuilder.RenameColumn(
                name: "FormEvalMarixId",
                table: "EvalForms",
                newName: "FormEvalMatrixId");

            migrationBuilder.RenameIndex(
                name: "IX_EvalForms_FormEvalMarixId",
                table: "EvalForms",
                newName: "IX_EvalForms_FormEvalMatrixId");

            migrationBuilder.AlterColumn<decimal>(
                name: "Weight",
                table: "FormItem",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<Guid>(
                name: "DropDownTypeId",
                table: "FormItem",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "WebsiteAttachmentId",
                table: "Departments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FormEvalMatrix",
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
                    table.PrimaryKey("PK_FormEvalMatrix", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormEvalMatrix_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FormEvalMatrix_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FormEvalMatrix_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FormEvalMatrix_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FormItem_DropDownTypeId",
                table: "FormItem",
                column: "DropDownTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_WebsiteAttachmentId",
                table: "Departments",
                column: "WebsiteAttachmentId");

            migrationBuilder.CreateIndex(
                name: "IX_FormEvalMatrix_CreateById",
                table: "FormEvalMatrix",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_FormEvalMatrix_DeleteById",
                table: "FormEvalMatrix",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_FormEvalMatrix_DepartmentId",
                table: "FormEvalMatrix",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_FormEvalMatrix_UpdateById",
                table: "FormEvalMatrix",
                column: "UpdateById");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_WebsiteAttachment_WebsiteAttachmentId",
                table: "Departments",
                column: "WebsiteAttachmentId",
                principalTable: "WebsiteAttachment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EvalForms_FormEvalMatrix_FormEvalMatrixId",
                table: "EvalForms",
                column: "FormEvalMatrixId",
                principalTable: "FormEvalMatrix",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FormEvalMarixValue_FormEvalMatrix_FormEvalMatrixId",
                table: "FormEvalMarixValue",
                column: "FormEvalMatrixId",
                principalTable: "FormEvalMatrix",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FormItem_DropDownType_DropDownTypeId",
                table: "FormItem",
                column: "DropDownTypeId",
                principalTable: "DropDownType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WebsiteAttachment_MinistryUsers_CreateById",
                table: "WebsiteAttachment",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WebsiteAttachment_MinistryUsers_DeleteById",
                table: "WebsiteAttachment",
                column: "DeleteById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WebsiteAttachment_MinistryUsers_UpdateById",
                table: "WebsiteAttachment",
                column: "UpdateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_WebsiteAttachment_WebsiteAttachmentId",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_EvalForms_FormEvalMatrix_FormEvalMatrixId",
                table: "EvalForms");

            migrationBuilder.DropForeignKey(
                name: "FK_FormEvalMarixValue_FormEvalMatrix_FormEvalMatrixId",
                table: "FormEvalMarixValue");

            migrationBuilder.DropForeignKey(
                name: "FK_FormItem_DropDownType_DropDownTypeId",
                table: "FormItem");

            migrationBuilder.DropForeignKey(
                name: "FK_WebsiteAttachment_MinistryUsers_CreateById",
                table: "WebsiteAttachment");

            migrationBuilder.DropForeignKey(
                name: "FK_WebsiteAttachment_MinistryUsers_DeleteById",
                table: "WebsiteAttachment");

            migrationBuilder.DropForeignKey(
                name: "FK_WebsiteAttachment_MinistryUsers_UpdateById",
                table: "WebsiteAttachment");

            migrationBuilder.DropTable(
                name: "FormEvalMatrix");

            migrationBuilder.DropIndex(
                name: "IX_FormItem_DropDownTypeId",
                table: "FormItem");

            migrationBuilder.DropIndex(
                name: "IX_Departments_WebsiteAttachmentId",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "DropDownTypeId",
                table: "FormItem");

            migrationBuilder.DropColumn(
                name: "WebsiteAttachmentId",
                table: "Departments");

            migrationBuilder.RenameColumn(
                name: "FormEvalMatrixId",
                table: "FormEvalMarixValue",
                newName: "FormEvalMarixid");

            migrationBuilder.RenameIndex(
                name: "IX_FormEvalMarixValue_FormEvalMatrixId",
                table: "FormEvalMarixValue",
                newName: "IX_FormEvalMarixValue_FormEvalMarixid");

            migrationBuilder.RenameColumn(
                name: "FormEvalMatrixId",
                table: "EvalForms",
                newName: "FormEvalMarixId");

            migrationBuilder.RenameIndex(
                name: "IX_EvalForms_FormEvalMatrixId",
                table: "EvalForms",
                newName: "IX_EvalForms_FormEvalMarixId");

            migrationBuilder.AlterColumn<int>(
                name: "Weight",
                table: "FormItem",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.CreateTable(
                name: "FormEvalMarix",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BackenName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderNo = table.Column<int>(type: "int", nullable: false),
                    Startdate = table.Column<DateOnly>(type: "date", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
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
                name: "RequestAssign",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0"),
                    IsLeader = table.Column<bool>(type: "bit", nullable: false),
                    RequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestAssign", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestAssign_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RequestAssign_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RequestAssign_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                });

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
                name: "IX_RequestAssign_CreateById",
                table: "RequestAssign",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_RequestAssign_DeleteById",
                table: "RequestAssign",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_RequestAssign_UpdateById",
                table: "RequestAssign",
                column: "UpdateById");

            migrationBuilder.AddForeignKey(
                name: "FK_EvalForms_FormEvalMarix_FormEvalMarixId",
                table: "EvalForms",
                column: "FormEvalMarixId",
                principalTable: "FormEvalMarix",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FormEvalMarixValue_FormEvalMarix_FormEvalMarixid",
                table: "FormEvalMarixValue",
                column: "FormEvalMarixid",
                principalTable: "FormEvalMarix",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WebsiteAttachment_MinistryUsers_CreateById",
                table: "WebsiteAttachment",
                column: "CreateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WebsiteAttachment_MinistryUsers_DeleteById",
                table: "WebsiteAttachment",
                column: "DeleteById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WebsiteAttachment_MinistryUsers_UpdateById",
                table: "WebsiteAttachment",
                column: "UpdateById",
                principalTable: "MinistryUsers",
                principalColumn: "Id");
        }
    }
}
