using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evaluation.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddSequance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Sequence",
                table: "ServiceRequests",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Sequence",
                table: "EvaluationRequest",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_ServiceRequests_Sequence",
                table: "ServiceRequests",
                column: "Sequence");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_EvaluationRequest_Sequence",
                table: "EvaluationRequest",
                column: "Sequence");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_ServiceRequests_Sequence",
                table: "ServiceRequests");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_EvaluationRequest_Sequence",
                table: "EvaluationRequest");

            migrationBuilder.DropColumn(
                name: "Sequence",
                table: "ServiceRequests");

            migrationBuilder.DropColumn(
                name: "Sequence",
                table: "EvaluationRequest");
        }
    }
}
