using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evaluation.DAL.Migrations
{
    /// <inheritdoc />
    public partial class EnhanceEvalFormConfigAndValue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Percentage",
                table: "FormItemConfig",
                newName: "WeightPercentage");

            migrationBuilder.AddColumn<string>(
                name: "DescAr",
                table: "WebGroups",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescEn",
                table: "WebGroups",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ItemConfigWeight",
                table: "FormItemValues",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ItemWeight",
                table: "FormItemValues",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "CountOfColumnsValue",
                table: "EvalForms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "HasMuliEvaluation",
                table: "EvalForms",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescAr",
                table: "WebGroups");

            migrationBuilder.DropColumn(
                name: "DescEn",
                table: "WebGroups");

            migrationBuilder.DropColumn(
                name: "ItemConfigWeight",
                table: "FormItemValues");

            migrationBuilder.DropColumn(
                name: "ItemWeight",
                table: "FormItemValues");

            migrationBuilder.DropColumn(
                name: "CountOfColumnsValue",
                table: "EvalForms");

            migrationBuilder.DropColumn(
                name: "HasMuliEvaluation",
                table: "EvalForms");

            migrationBuilder.RenameColumn(
                name: "WeightPercentage",
                table: "FormItemConfig",
                newName: "Percentage");
        }
    }
}
