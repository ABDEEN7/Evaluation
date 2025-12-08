using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evaluation.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnsInFormItemAndOrderNo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<bool>(
            //    name: "HasNote",
            //    table: "FormItem",
            //    type: "bit",
            //    nullable: false,
            //    defaultValue: false);

            //migrationBuilder.AddColumn<int>(
            //    name: "OrderNo",
            //    table: "FormItem",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "OrderNo",
                table: "EvalFormType",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
            //    name: "HasNote",
            //    table: "FormItem");

            //migrationBuilder.DropColumn(
            //    name: "OrderNo",
            //    table: "FormItem");

            migrationBuilder.AlterColumn<string>(
                name: "OrderNo",
                table: "EvalFormType",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
