using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evaluation.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddAPIValueAndHasMultiValue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ServiceRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAPI",
                table: "ServiceRequestFieldsValue",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasMuliValue",
                table: "FormItem",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ServiceRequestFieldsValueHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RefId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequestType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FieldId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsMissing = table.Column<bool>(type: "bit", nullable: true),
                    IsAPI = table.Column<bool>(type: "bit", nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("PK_ServiceRequestFieldsValueHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceRequestFieldsValueHistory_Field_FieldId",
                        column: x => x.FieldId,
                        principalTable: "Field",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceRequestFieldsValueHistory_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceRequestFieldsValueHistory_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ServiceRequestFieldsValueHistory_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequestFieldsValueHistory_CreateById",
                table: "ServiceRequestFieldsValueHistory",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequestFieldsValueHistory_DeleteById",
                table: "ServiceRequestFieldsValueHistory",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequestFieldsValueHistory_FieldId",
                table: "ServiceRequestFieldsValueHistory",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequestFieldsValueHistory_UpdateById",
                table: "ServiceRequestFieldsValueHistory",
                column: "UpdateById");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServiceRequestFieldsValueHistory");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "ServiceRequests");

            migrationBuilder.DropColumn(
                name: "IsAPI",
                table: "ServiceRequestFieldsValue");

            migrationBuilder.DropColumn(
                name: "HasMuliValue",
                table: "FormItem");
        }
    }
}
