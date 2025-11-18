using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evaluation.DAL.Migrations
{
    /// <inheritdoc />
    public partial class addderivedtables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrgTree_JobTitle_JobTitleId",
                table: "OrgTree");

            migrationBuilder.DropForeignKey(
                name: "FK_OrgTree_SchoolTypes_SchoolTypeId",
                table: "OrgTree");

            migrationBuilder.DropForeignKey(
                name: "FK_OrgTree_SchoolTypes_SchoolTypeId1",
                table: "OrgTree");

            migrationBuilder.DropForeignKey(
                name: "FK_OrgTree_UserGender_UserGenderId",
                table: "OrgTree");

            migrationBuilder.DropIndex(
                name: "IX_OrgTree_JobTitleId",
                table: "OrgTree");

            migrationBuilder.DropIndex(
                name: "IX_OrgTree_SchoolTypeId",
                table: "OrgTree");

            migrationBuilder.DropIndex(
                name: "IX_OrgTree_SchoolTypeId1",
                table: "OrgTree");

            migrationBuilder.DropIndex(
                name: "IX_OrgTree_UserGenderId",
                table: "OrgTree");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "OrgTree");

            migrationBuilder.DropColumn(
                name: "BirthDate",
                table: "OrgTree");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "OrgTree");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "OrgTree");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "OrgTree");

            migrationBuilder.DropColumn(
                name: "EmployeeNo",
                table: "OrgTree");

            migrationBuilder.DropColumn(
                name: "EstablishmentDate",
                table: "OrgTree");

            migrationBuilder.DropColumn(
                name: "JobTitleId",
                table: "OrgTree");

            migrationBuilder.DropColumn(
                name: "JoinDate",
                table: "OrgTree");

            migrationBuilder.DropColumn(
                name: "ManageEmail",
                table: "OrgTree");

            migrationBuilder.DropColumn(
                name: "ManagerQID",
                table: "OrgTree");

            migrationBuilder.DropColumn(
                name: "Mobile",
                table: "OrgTree");

            migrationBuilder.DropColumn(
                name: "NationalityCode",
                table: "OrgTree");

            migrationBuilder.DropColumn(
                name: "OrgEmail",
                table: "OrgTree");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "OrgTree");

            migrationBuilder.DropColumn(
                name: "QID",
                table: "OrgTree");

            migrationBuilder.DropColumn(
                name: "Region",
                table: "OrgTree");

            migrationBuilder.DropColumn(
                name: "SchoolTypeId",
                table: "OrgTree");

            migrationBuilder.DropColumn(
                name: "SchoolTypeId1",
                table: "OrgTree");

            migrationBuilder.DropColumn(
                name: "School_Address",
                table: "OrgTree");

            migrationBuilder.DropColumn(
                name: "School_EstablishmentDate",
                table: "OrgTree");

            migrationBuilder.DropColumn(
                name: "School_ManageEmail",
                table: "OrgTree");

            migrationBuilder.DropColumn(
                name: "School_ManagerQID",
                table: "OrgTree");

            migrationBuilder.DropColumn(
                name: "School_Mobile",
                table: "OrgTree");

            migrationBuilder.DropColumn(
                name: "School_OrgEmail",
                table: "OrgTree");

            migrationBuilder.DropColumn(
                name: "School_Phone",
                table: "OrgTree");

            migrationBuilder.DropColumn(
                name: "School_TypeId",
                table: "OrgTree");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "OrgTree");

            migrationBuilder.DropColumn(
                name: "UserGenderId",
                table: "OrgTree");

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserGenderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BirthDate = table.Column<DateOnly>(type: "date", nullable: false),
                    NationalityCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JoinDate = table.Column<DateOnly>(type: "date", nullable: false),
                    JobTitleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QID = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_JobTitle_JobTitleId",
                        column: x => x.JobTitleId,
                        principalTable: "JobTitle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employees_OrgTree_Id",
                        column: x => x.Id,
                        principalTable: "OrgTree",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Employees_UserGender_UserGenderId",
                        column: x => x.UserGenderId,
                        principalTable: "UserGender",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EstablishmentDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ManagerQID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ManageEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrgEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrgTypeId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Organizations_OrgTree_Id",
                        column: x => x.Id,
                        principalTable: "OrgTree",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Organizations_OrgTypes_OrgTypeId1",
                        column: x => x.OrgTypeId1,
                        principalTable: "OrgTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Organizations_SchoolTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "SchoolTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Schools",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EstablishmentDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ManagerQID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ManageEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrgEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Region = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schools", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Schools_OrgTree_Id",
                        column: x => x.Id,
                        principalTable: "OrgTree",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Schools_SchoolTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "SchoolTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_JobTitleId",
                table: "Employees",
                column: "JobTitleId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_UserGenderId",
                table: "Employees",
                column: "UserGenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_OrgTypeId1",
                table: "Organizations",
                column: "OrgTypeId1");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_TypeId",
                table: "Organizations",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Schools_TypeId",
                table: "Schools",
                column: "TypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Organizations");

            migrationBuilder.DropTable(
                name: "Schools");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "OrgTree",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "BirthDate",
                table: "OrgTree",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "OrgTree",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "OrgTree",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "OrgTree",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmployeeNo",
                table: "OrgTree",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "EstablishmentDate",
                table: "OrgTree",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "JobTitleId",
                table: "OrgTree",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "JoinDate",
                table: "OrgTree",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ManageEmail",
                table: "OrgTree",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ManagerQID",
                table: "OrgTree",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Mobile",
                table: "OrgTree",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NationalityCode",
                table: "OrgTree",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrgEmail",
                table: "OrgTree",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "OrgTree",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QID",
                table: "OrgTree",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Region",
                table: "OrgTree",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolTypeId",
                table: "OrgTree",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolTypeId1",
                table: "OrgTree",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "School_Address",
                table: "OrgTree",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "School_EstablishmentDate",
                table: "OrgTree",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "School_ManageEmail",
                table: "OrgTree",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "School_ManagerQID",
                table: "OrgTree",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "School_Mobile",
                table: "OrgTree",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "School_OrgEmail",
                table: "OrgTree",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "School_Phone",
                table: "OrgTree",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "School_TypeId",
                table: "OrgTree",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TypeId",
                table: "OrgTree",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserGenderId",
                table: "OrgTree",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrgTree_JobTitleId",
                table: "OrgTree",
                column: "JobTitleId");

            migrationBuilder.CreateIndex(
                name: "IX_OrgTree_SchoolTypeId",
                table: "OrgTree",
                column: "SchoolTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_OrgTree_SchoolTypeId1",
                table: "OrgTree",
                column: "SchoolTypeId1");

            migrationBuilder.CreateIndex(
                name: "IX_OrgTree_UserGenderId",
                table: "OrgTree",
                column: "UserGenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrgTree_JobTitle_JobTitleId",
                table: "OrgTree",
                column: "JobTitleId",
                principalTable: "JobTitle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrgTree_SchoolTypes_SchoolTypeId",
                table: "OrgTree",
                column: "SchoolTypeId",
                principalTable: "SchoolTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrgTree_SchoolTypes_SchoolTypeId1",
                table: "OrgTree",
                column: "SchoolTypeId1",
                principalTable: "SchoolTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrgTree_UserGender_UserGenderId",
                table: "OrgTree",
                column: "UserGenderId",
                principalTable: "UserGender",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
