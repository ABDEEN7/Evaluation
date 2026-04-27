using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evaluation.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddBackendToCalcMethod : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BackendName",
                table: "CalcMethods",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CalcMethods_BackendName",
                table: "CalcMethods",
                column: "BackendName",
                unique: true,
                filter: "[BackendName] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CalcMethods_BackendName",
                table: "CalcMethods");

            migrationBuilder.DropColumn(
                name: "BackendName",
                table: "CalcMethods");
        }
    }
}
