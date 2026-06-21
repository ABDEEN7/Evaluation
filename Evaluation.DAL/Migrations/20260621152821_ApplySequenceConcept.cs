using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evaluation.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ApplySequenceConcept : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "EvaluationRequest_Sequence");

            migrationBuilder.CreateSequence(
                name: "Request_Sequence");

            //migrationBuilder.AlterColumn<long>(
            //    name: "Sequence",
            //    table: "ServiceRequests",
            //    type: "bigint",
            //    nullable: false,
            //    oldClrType: typeof(long),
            //    oldType: "bigint")
            //    .OldAnnotation("SqlServer:Identity", "1, 1");

            //migrationBuilder.AlterColumn<long>(
            //    name: "Sequence",
            //    table: "EvaluationRequest",
            //    type: "bigint",
            //    nullable: false,
            //    oldClrType: typeof(long),
            //    oldType: "bigint")
            //    .OldAnnotation("SqlServer:Identity", "1, 1");

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

            migrationBuilder.DropSequence(
                name: "EvaluationRequest_Sequence");

            migrationBuilder.DropSequence(
                name: "Request_Sequence");

            //migrationBuilder.AlterColumn<long>(
            //    name: "Sequence",
            //    table: "ServiceRequests",
            //    type: "bigint",
            //    nullable: false,
            //    oldClrType: typeof(long),
            //    oldType: "bigint")
            //    .Annotation("SqlServer:Identity", "1, 1");

            //migrationBuilder.AlterColumn<long>(
            //    name: "Sequence",
            //    table: "EvaluationRequest",
            //    type: "bigint",
            //    nullable: false,
            //    oldClrType: typeof(long),
            //    oldType: "bigint")
            //    .Annotation("SqlServer:Identity", "1, 1");
        }
    }
}
