using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evaluation.DAL.Migrations
{
    /// <inheritdoc />
    public partial class EnhanceEmployeeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BackendName",
                table: "OrgTypes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "QID",
                table: "OrgTree",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HRCode",
                table: "OrgClass",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HRCode",
                table: "JobTitle",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BackendName",
                table: "OrgTypes");

            migrationBuilder.DropColumn(
                name: "QID",
                table: "OrgTree");

            migrationBuilder.DropColumn(
                name: "HRCode",
                table: "OrgClass");

            migrationBuilder.DropColumn(
                name: "HRCode",
                table: "JobTitle");
        }
    }
}
