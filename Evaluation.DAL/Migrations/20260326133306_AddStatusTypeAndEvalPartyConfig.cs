using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evaluation.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusTypeAndEvalPartyConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DepartmentEvaluationParty");

            migrationBuilder.DropTable(
                name: "VisitType");

            migrationBuilder.DropColumn(
                name: "IsOpen",
                table: "ServiceStatus");

            migrationBuilder.RenameColumn(
                name: "IsRopric",
                table: "EvalForms",
                newName: "IsFinalEval");

            migrationBuilder.AddColumn<Guid>(
                name: "ServiceStatusTypeId",
                table: "ServiceStatus",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CalcMethodId",
                table: "FormItemValues",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NextEvalDays",
                table: "FormEvalMatrixValue",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EvalDays",
                table: "EvaluationRequest",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "EvaluationDate",
                table: "EvaluationRequest",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "FormEvalMatrixValueId",
                table: "EvaluationRequest",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "NextEvaluationDate",
                table: "EvaluationRequest",
                type: "date",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "FormEvalMatrixId",
                table: "EvalForms",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsEvaluated",
                table: "Departments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "PartyTypeEvalParty",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PartyTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EvaluationPartyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_PartyTypeEvalParty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PartyTypeEvalParty_EvaluationParties_EvaluationPartyId",
                        column: x => x.EvaluationPartyId,
                        principalTable: "EvaluationParties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PartyTypeEvalParty_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PartyTypeEvalParty_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PartyTypeEvalParty_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PartyTypeEvalParty_PartyTypes_PartyTypeId",
                        column: x => x.PartyTypeId,
                        principalTable: "PartyTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ServiceStatusType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BackendName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsOpen = table.Column<bool>(type: "bit", nullable: false),
                    ColorCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderNo = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_ServiceStatusType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceStatusType_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServiceStatusType_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServiceStatusType_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PartyTypeEvalPartyStatus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PartyTypeEvalPartyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceStatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_PartyTypeEvalPartyStatus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PartyTypeEvalPartyStatus_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PartyTypeEvalPartyStatus_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PartyTypeEvalPartyStatus_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PartyTypeEvalPartyStatus_PartyTypeEvalParty_PartyTypeEvalPartyId",
                        column: x => x.PartyTypeEvalPartyId,
                        principalTable: "PartyTypeEvalParty",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PartyTypeEvalPartyStatus_ServiceStatus_ServiceStatusId",
                        column: x => x.ServiceStatusId,
                        principalTable: "ServiceStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceStatus_ServiceStatusTypeId",
                table: "ServiceStatus",
                column: "ServiceStatusTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FormItemValues_CalcMethodId",
                table: "FormItemValues",
                column: "CalcMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_EvaluationRequest_FormEvalMatrixValueId",
                table: "EvaluationRequest",
                column: "FormEvalMatrixValueId");

            migrationBuilder.CreateIndex(
                name: "IX_PartyTypeEvalParty_CreateById",
                table: "PartyTypeEvalParty",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_PartyTypeEvalParty_DeleteById",
                table: "PartyTypeEvalParty",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_PartyTypeEvalParty_EvaluationPartyId",
                table: "PartyTypeEvalParty",
                column: "EvaluationPartyId");

            migrationBuilder.CreateIndex(
                name: "IX_PartyTypeEvalParty_PartyTypeId",
                table: "PartyTypeEvalParty",
                column: "PartyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PartyTypeEvalParty_UpdateById",
                table: "PartyTypeEvalParty",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_PartyTypeEvalPartyStatus_CreateById",
                table: "PartyTypeEvalPartyStatus",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_PartyTypeEvalPartyStatus_DeleteById",
                table: "PartyTypeEvalPartyStatus",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_PartyTypeEvalPartyStatus_PartyTypeEvalPartyId",
                table: "PartyTypeEvalPartyStatus",
                column: "PartyTypeEvalPartyId");

            migrationBuilder.CreateIndex(
                name: "IX_PartyTypeEvalPartyStatus_ServiceStatusId",
                table: "PartyTypeEvalPartyStatus",
                column: "ServiceStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_PartyTypeEvalPartyStatus_UpdateById",
                table: "PartyTypeEvalPartyStatus",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceStatusType_BackendName",
                table: "ServiceStatusType",
                column: "BackendName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServiceStatusType_CreateById",
                table: "ServiceStatusType",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceStatusType_DeleteById",
                table: "ServiceStatusType",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceStatusType_UpdateById",
                table: "ServiceStatusType",
                column: "UpdateById");

            migrationBuilder.AddForeignKey(
                name: "FK_EvaluationRequest_FormEvalMatrixValue_FormEvalMatrixValueId",
                table: "EvaluationRequest",
                column: "FormEvalMatrixValueId",
                principalTable: "FormEvalMatrixValue",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FormItemValues_CalcMethods_CalcMethodId",
                table: "FormItemValues",
                column: "CalcMethodId",
                principalTable: "CalcMethods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceStatus_ServiceStatusType_ServiceStatusTypeId",
                table: "ServiceStatus",
                column: "ServiceStatusTypeId",
                principalTable: "ServiceStatusType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EvaluationRequest_FormEvalMatrixValue_FormEvalMatrixValueId",
                table: "EvaluationRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_FormItemValues_CalcMethods_CalcMethodId",
                table: "FormItemValues");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceStatus_ServiceStatusType_ServiceStatusTypeId",
                table: "ServiceStatus");

            migrationBuilder.DropTable(
                name: "PartyTypeEvalPartyStatus");

            migrationBuilder.DropTable(
                name: "ServiceStatusType");

            migrationBuilder.DropTable(
                name: "PartyTypeEvalParty");

            migrationBuilder.DropIndex(
                name: "IX_ServiceStatus_ServiceStatusTypeId",
                table: "ServiceStatus");

            migrationBuilder.DropIndex(
                name: "IX_FormItemValues_CalcMethodId",
                table: "FormItemValues");

            migrationBuilder.DropIndex(
                name: "IX_EvaluationRequest_FormEvalMatrixValueId",
                table: "EvaluationRequest");

            migrationBuilder.DropColumn(
                name: "ServiceStatusTypeId",
                table: "ServiceStatus");

            migrationBuilder.DropColumn(
                name: "CalcMethodId",
                table: "FormItemValues");

            migrationBuilder.DropColumn(
                name: "NextEvalDays",
                table: "FormEvalMatrixValue");

            migrationBuilder.DropColumn(
                name: "EvalDays",
                table: "EvaluationRequest");

            migrationBuilder.DropColumn(
                name: "EvaluationDate",
                table: "EvaluationRequest");

            migrationBuilder.DropColumn(
                name: "FormEvalMatrixValueId",
                table: "EvaluationRequest");

            migrationBuilder.DropColumn(
                name: "NextEvaluationDate",
                table: "EvaluationRequest");

            migrationBuilder.DropColumn(
                name: "IsEvaluated",
                table: "Departments");

            migrationBuilder.RenameColumn(
                name: "IsFinalEval",
                table: "EvalForms",
                newName: "IsRopric");

            migrationBuilder.AddColumn<bool>(
                name: "IsOpen",
                table: "ServiceStatus",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<Guid>(
                name: "FormEvalMatrixId",
                table: "EvalForms",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateTable(
                name: "DepartmentEvaluationParty",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderNo = table.Column<int>(type: "int", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepartmentEvaluationParty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DepartmentEvaluationParty_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DepartmentEvaluationParty_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DepartmentEvaluationParty_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DepartmentEvaluationParty_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "VisitType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VisitType_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VisitType_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_VisitType_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentEvaluationParty_CreateById",
                table: "DepartmentEvaluationParty",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentEvaluationParty_DeleteById",
                table: "DepartmentEvaluationParty",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentEvaluationParty_DepartmentId",
                table: "DepartmentEvaluationParty",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentEvaluationParty_UpdateById",
                table: "DepartmentEvaluationParty",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_VisitType_CreateById",
                table: "VisitType",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_VisitType_DeleteById",
                table: "VisitType",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_VisitType_UpdateById",
                table: "VisitType",
                column: "UpdateById");
        }
    }
}
