using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evaluation.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MinistryUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NationalityCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastLoginDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PreferredLanguage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JobTitleEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JobTitleAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JobTitleCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DirectManagerQId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmployeeNo = table.Column<int>(type: "int", nullable: false),
                    OrganizationNo = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MinistryUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MinistryUsers_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MinistryUsers_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MinistryUsers_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ActionType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TitleAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TitleEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BackendName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OrderNo = table.Column<int>(type: "int", nullable: true),
                    IsRequiredValidation = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActionType_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActionType_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActionType_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Attribute",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attribute", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attribute_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Attribute_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Attribute_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TableName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RefId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ColumnName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OldValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditLogs_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AuditLogs_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AuditLogs_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Banner",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TitleAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TitleEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TitleColor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SummaryAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SummaryEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SummaryColor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UrlAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UrlEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImgNameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImgNameAr_UiFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImgNameAr_BlobURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImgNameAr_FileExt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImgNameAr_Size = table.Column<long>(type: "bigint", nullable: true),
                    ImgNameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImgNameEn_UiFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImgNameEn_BlobURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImgNameEn_FileExt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImgNameEn_Size = table.Column<long>(type: "bigint", nullable: true),
                    BtnNameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BtnNameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ShowAr = table.Column<bool>(type: "bit", nullable: false),
                    ShowEn = table.Column<bool>(type: "bit", nullable: false),
                    Target = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderNo = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banner", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Banner_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Banner_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Banner_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Category_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Category_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Category_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChangeRequestStatus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangeRequestStatus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChangeRequestStatus_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChangeRequestStatus_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ChangeRequestStatus_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CssApplyType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BackendName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CssApplyType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CssApplyType_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CssApplyType_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CssApplyType_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DepartmentEvaluationParty",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MyProperty = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepartmentEvaluationParty", x => x.Id);
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
                name: "DepartmentRoleAttributeValue",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleAttributeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepartmentRoleAttributeValue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DepartmentRoleAttributeValue_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DepartmentRoleAttributeValue_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DepartmentRoleAttributeValue_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DropDownType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BackendName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TitleAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TitleEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataSourceTable = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DropDownType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DropDownType_DropDownType_ParentId",
                        column: x => x.ParentId,
                        principalTable: "DropDownType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DropDownType_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DropDownType_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DropDownType_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmailLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Emails = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CCc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Sent = table.Column<bool>(type: "bit", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailLog_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmailLog_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EmailLog_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EmailTemplates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TitleAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TitleEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TemplateSubject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TemplateBody = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BackendName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EmailProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FielsFromRequest = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FielsFromEvaluation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailTemplates_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmailTemplates_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmailTemplates_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExceptionLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExceptionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StackTrace = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    JsonParameter = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Context = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExceptionLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExceptionLog_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExceptionLog_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ExceptionLog_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FieldTypeShow",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BackendName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldTypeShow", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FieldTypeShow_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FieldTypeShow_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FieldTypeShow_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FormGroupCustomList",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BackendName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Schema = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormGroupCustomList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormGroupCustomList_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FormGroupCustomList_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FormGroupCustomList_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FormGroupType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BackendName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormGroupType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormGroupType_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FormGroupType_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FormGroupType_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FormStatus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormStatus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormStatus_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FormStatus_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FormStatus_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Level",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Level", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Level_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Level_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Level_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ModuleType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModuleType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModuleType_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ModuleType_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ModuleType_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ModuleType_ModuleType_ParentId",
                        column: x => x.ParentId,
                        principalTable: "ModuleType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrganizationTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrganizationTypes_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrganizationTypes_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrganizationTypes_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrgTreeClass",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BackendName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrgTreeClass", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrgTreeClass_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrgTreeClass_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrgTreeClass_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrgTreeClass_OrgTreeClass_ParentId",
                        column: x => x.ParentId,
                        principalTable: "OrgTreeClass",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Pages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BackendName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pages_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pages_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pages_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PartyTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BackendName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsEmployeePartyType = table.Column<bool>(type: "bit", nullable: false),
                    CanViewAllRequests = table.Column<bool>(type: "bit", nullable: false),
                    CanViewAllEvaluations = table.Column<bool>(type: "bit", nullable: false),
                    CanViewEntityEvaluation = table.Column<bool>(type: "bit", nullable: false),
                    SystemModuleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartyTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PartyTypes_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PartyTypes_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PartyTypes_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BackendName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Permissions_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Permissions_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Permissions_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlanStatuses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanStatuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanStatuses_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlanStatuses_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlanStatuses_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlanType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderNo = table.Column<int>(type: "int", nullable: false),
                    BackendName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanType_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlanType_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlanType_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RelatedFieldsGroup",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BackendName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelatedFieldsGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RelatedFieldsGroup_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RelatedFieldsGroup_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RelatedFieldsGroup_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RequestAssign",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsLeader = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestAssign", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestAssign_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Roles_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Roles_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Roles_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SchoolTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchoolTypes_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SchoolTypes_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SchoolTypes_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SectionDepartment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SectionId = table.Column<int>(type: "int", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SectionDepartment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SectionDepartment_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SectionDepartment_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SectionDepartment_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SiteDocument",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TitleAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TitleEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Skey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileName_UiFileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileName_BlobURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileName_FileExt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileName_Size = table.Column<long>(type: "bigint", nullable: false),
                    ViewCount = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteDocument", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SiteDocument_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SiteDocument_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SiteDocument_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SMSLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Sent = table.Column<bool>(type: "bit", nullable: false),
                    RefID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SMSLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SMSLog_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SMSLog_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SMSLog_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SMSProfile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BackendName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BaseUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SMSProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SMSProfile_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SMSProfile_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SMSProfile_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SMSProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SMSProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SMSProfiles_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SMSProfiles_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SMSProfiles_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StatusService",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusService", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StatusService_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StatusService_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StatusService_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SystemModuleType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BackendName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentModuleTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemModuleType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SystemModuleType_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SystemModuleType_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SystemModuleType_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SystemModuleType_SystemModuleType_ParentModuleTypeId",
                        column: x => x.ParentModuleTypeId,
                        principalTable: "SystemModuleType",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SystemSetting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SettingGroup = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SettingKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SettingValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemSetting", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SystemSetting_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SystemSetting_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SystemSetting_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TemplateDocuments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TemplateDocuments_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TemplateDocuments_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TemplateDocuments_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TemplateGenrationTypies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TitleAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TitleEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BackendName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateGenrationTypies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TemplateGenrationTypies_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TemplateGenrationTypies_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TemplateGenrationTypies_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransactionType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BackendName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionType_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransactionType_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TransactionType_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UiControl",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PageName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserUiname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BackendName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ControlName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ValueEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ValueAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UiControl", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UiControl_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UiControl_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UiControl_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserLoginLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IP = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserAgent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLoginLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserLoginLogs_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserLoginLogs_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserLoginLogs_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserLoginLogs_MinistryUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserToken",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Deprecated = table.Column<bool>(type: "bit", nullable: false),
                    TokenExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeprecatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserAgent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IP = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserToken_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserToken_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserToken_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserToken_MinistryUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WebsiteAttachment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RefId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UiFileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BlobUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileExtension = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebsiteAttachment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WebsiteAttachment_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WebsiteAttachment_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WebsiteAttachment_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CssClass",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClassName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Styles = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApplyTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CssClass", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CssClass_CssApplyType_ApplyTypeId",
                        column: x => x.ApplyTypeId,
                        principalTable: "CssApplyType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CssClass_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CssClass_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CssClass_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FieldDropDownValue",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TitleEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TitleAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DropDownBackendName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DropDownTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParentDropDownId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OrderNo = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldDropDownValue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FieldDropDownValue_DropDownType_DropDownTypeId",
                        column: x => x.DropDownTypeId,
                        principalTable: "DropDownType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FieldDropDownValue_FieldDropDownValue_ParentDropDownId",
                        column: x => x.ParentDropDownId,
                        principalTable: "FieldDropDownValue",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FieldDropDownValue_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FieldDropDownValue_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FieldDropDownValue_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FieldType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BackendName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderNo = table.Column<int>(type: "int", nullable: true),
                    ShowTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FieldType_FieldTypeShow_ShowTypeId",
                        column: x => x.ShowTypeId,
                        principalTable: "FieldTypeShow",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FieldType_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FieldType_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FieldType_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrganizationTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Departments_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Departments_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Departments_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Departments_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Departments_OrganizationTypes_OrganizationTypeId",
                        column: x => x.OrganizationTypeId,
                        principalTable: "OrganizationTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserPartyTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PartyTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SignaturePlaceHolder = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPartyTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPartyTypes_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPartyTypes_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserPartyTypes_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserPartyTypes_MinistryUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserPartyTypes_PartyTypes_PartyTypeId",
                        column: x => x.PartyTypeId,
                        principalTable: "PartyTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ControlValidations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PermissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ControlType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JsonField = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UibackendName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false),
                    MinLength = table.Column<int>(type: "int", nullable: true),
                    MaxLength = table.Column<int>(type: "int", nullable: true),
                    Regex = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDbrequired = table.Column<bool>(type: "bit", nullable: true),
                    DbMaxLength = table.Column<int>(type: "int", nullable: true),
                    RowOrder = table.Column<int>(type: "int", nullable: true),
                    ColumnOrder = table.Column<int>(type: "int", nullable: false),
                    ShowInGrid = table.Column<bool>(type: "bit", nullable: false),
                    TabulatorConfigJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaxFileCount = table.Column<int>(type: "int", nullable: true),
                    MaxFileSize = table.Column<int>(type: "int", nullable: true),
                    FileExtention = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ControlJsonConfig = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ControlAttribute = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ControlValidations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ControlValidations_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ControlValidations_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ControlValidations_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ControlValidations_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Navbar",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TitleAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TitleEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsInternal = table.Column<bool>(type: "bit", nullable: false),
                    UrlAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UrlEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderNo = table.Column<int>(type: "int", nullable: true),
                    Target = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAuthorized = table.Column<bool>(type: "bit", nullable: false),
                    PermissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Navbar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Navbar_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Navbar_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Navbar_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Navbar_Navbar_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Navbar",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Navbar_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PagePermissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PermissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PagePermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PagePermissions_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PagePermissions_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PagePermissions_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PagePermissions_Pages_PageId",
                        column: x => x.PageId,
                        principalTable: "Pages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PagePermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SideBar",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PermissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoutingPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderNo = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SideBar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SideBar_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SideBar_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SideBar_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SideBar_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SideBar_SideBar_ParentId",
                        column: x => x.ParentId,
                        principalTable: "SideBar",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PermissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RolePermissions_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RolePermissions_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RolePermissions_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRoles_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserRoles_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserRoles_MinistryUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrgTree",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrgTreeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    HrCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NSISCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrgTreeTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrgTreeClassId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    EmployeeNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Nationality = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JoinDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    JobTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrganizationTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EstablishmentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Region = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SchoolTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrgTree", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrgTree_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrgTree_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrgTree_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrgTree_OrgTreeClass_OrgTreeClassId",
                        column: x => x.OrgTreeClassId,
                        principalTable: "OrgTreeClass",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrgTree_OrgTree_ParentId",
                        column: x => x.ParentId,
                        principalTable: "OrgTree",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrgTree_OrganizationTypes_OrganizationTypeId",
                        column: x => x.OrganizationTypeId,
                        principalTable: "OrganizationTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrgTree_SchoolTypes_SchoolTypeId",
                        column: x => x.SchoolTypeId,
                        principalTable: "SchoolTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SMSTemplates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TitleAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TitleEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Messages = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BackendName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SMSProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SMSTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SMSTemplates_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SMSTemplates_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SMSTemplates_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SMSTemplates_SMSProfiles_SMSProfileId",
                        column: x => x.SMSProfileId,
                        principalTable: "SMSProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmailTemplateDocuments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmailTemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TemplateDocId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TemplateDocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailTemplateDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailTemplateDocuments_EmailTemplates_EmailTemplateId",
                        column: x => x.EmailTemplateId,
                        principalTable: "EmailTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmailTemplateDocuments_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmailTemplateDocuments_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmailTemplateDocuments_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmailTemplateDocuments_TemplateDocuments_TemplateDocumentId",
                        column: x => x.TemplateDocumentId,
                        principalTable: "TemplateDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AcademicYears",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademicYears", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AcademicYears_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AcademicYears_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AcademicYears_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AcademicYears_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CalcMethods",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MinPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaxPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MinWeight = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaxWeight = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalcMethods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CalcMethods_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CalcMethods_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CalcMethods_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CalcMethods_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EvaluationParties",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderNo = table.Column<int>(type: "int", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvaluationParties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EvaluationParties_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EvaluationParties_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EvaluationParties_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EvaluationParties_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlanTypeDepartment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlanTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderNo = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanTypeDepartment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanTypeDepartment_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlanTypeDepartment_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlanTypeDepartment_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PlanTypeDepartment_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PlanTypeDepartment_PlanType_PlanTypeId",
                        column: x => x.PlanTypeId,
                        principalTable: "PlanType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScopeTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderNo = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScopeTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScopeTypes_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ScopeTypes_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ScopeTypes_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ScopeTypes_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ScopeTypes_ScopeTypes_ParentId",
                        column: x => x.ParentId,
                        principalTable: "ScopeTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SystemModules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BackendName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Routing = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SchNoDefinition = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ButtonAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ButtonEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderNo = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemModules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SystemModules_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SystemModules_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SystemModules_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SystemModules_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Team",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Team", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Team_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Team_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Team_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Team_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserDeparment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDeparment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserDeparment_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserDeparment_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserDeparment_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserDeparment_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserDeparment_MinistryUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserDepartment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDepartment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserDepartment_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserDepartment_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserDepartment_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserDepartment_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserDepartment_MinistryUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserPartyTypeSignatures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserPartyTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Signature = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPartyTypeSignatures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPartyTypeSignatures_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserPartyTypeSignatures_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserPartyTypeSignatures_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserPartyTypeSignatures_UserPartyTypes_UserPartyTypeId",
                        column: x => x.UserPartyTypeId,
                        principalTable: "UserPartyTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SiteContent",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TitleAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TitleEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Routing = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NavbarId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FileNameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileNameAr_UiFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileNameAr_BlobURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileNameAr_FileExt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileNameAr_Size = table.Column<long>(type: "bigint", nullable: true),
                    FileNameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileNameEn_UiFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileNameEn_BlobURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileNameEn_FileExt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileNameEn_Size = table.Column<long>(type: "bigint", nullable: true),
                    OrderNo = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteContent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SiteContent_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SiteContent_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SiteContent_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SiteContent_Navbar_NavbarId",
                        column: x => x.NavbarId,
                        principalTable: "Navbar",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SiteContent_SiteContent_ParentId",
                        column: x => x.ParentId,
                        principalTable: "SiteContent",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DepartmentOrgTrees",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationTreeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrgTreeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepartmentOrgTrees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DepartmentOrgTrees_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DepartmentOrgTrees_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DepartmentOrgTrees_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DepartmentOrgTrees_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DepartmentOrgTrees_OrgTree_OrgTreeId",
                        column: x => x.OrgTreeId,
                        principalTable: "OrgTree",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SchoolLevel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SchoolId = table.Column<int>(type: "int", nullable: false),
                    SchoolId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LevelId = table.Column<int>(type: "int", nullable: false),
                    LevelId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolLevel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchoolLevel_Level_LevelId1",
                        column: x => x.LevelId1,
                        principalTable: "Level",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SchoolLevel_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SchoolLevel_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SchoolLevel_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SchoolLevel_OrgTree_SchoolId1",
                        column: x => x.SchoolId1,
                        principalTable: "OrgTree",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChangeRequestType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    DepartmentId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AcademicYearId = table.Column<int>(type: "int", nullable: false),
                    AcademicYearId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangeRequestType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChangeRequestType_AcademicYears_AcademicYearId1",
                        column: x => x.AcademicYearId1,
                        principalTable: "AcademicYears",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChangeRequestType_Departments_DepartmentId1",
                        column: x => x.DepartmentId1,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChangeRequestType_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChangeRequestType_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChangeRequestType_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DepartmentHoliday",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsCronExpression = table.Column<bool>(type: "bit", nullable: false),
                    CronExpression = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AcademicYearId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepartmentHoliday", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DepartmentHoliday_AcademicYears_AcademicYearId",
                        column: x => x.AcademicYearId,
                        principalTable: "AcademicYears",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DepartmentHoliday_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DepartmentHoliday_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DepartmentHoliday_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DepartmentHoliday_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OrgAcademicYears",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParentOrgId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: false),
                    AcademicYearId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrgAcademicYears", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrgAcademicYears_AcademicYears_AcademicYearId",
                        column: x => x.AcademicYearId,
                        principalTable: "AcademicYears",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrgAcademicYears_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrgAcademicYears_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrgAcademicYears_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrgAcademicYears_OrgTree_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "OrgTree",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrgAcademicYears_OrgTree_ParentOrgId",
                        column: x => x.ParentOrgId,
                        principalTable: "OrgTree",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Plans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ExpectedListJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    DepartmentId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AcademicYearId = table.Column<int>(type: "int", nullable: false),
                    AcademicYearId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PlanStatusId = table.Column<int>(type: "int", nullable: false),
                    PlanStatusId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PlanScheduleId = table.Column<int>(type: "int", nullable: false),
                    PlanTypeId = table.Column<int>(type: "int", nullable: false),
                    PlanTypeId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Plans_AcademicYears_AcademicYearId1",
                        column: x => x.AcademicYearId1,
                        principalTable: "AcademicYears",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Plans_Departments_DepartmentId1",
                        column: x => x.DepartmentId1,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Plans_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Plans_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Plans_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Plans_PlanStatuses_PlanStatusId1",
                        column: x => x.PlanStatusId1,
                        principalTable: "PlanStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Plans_PlanType_PlanTypeId1",
                        column: x => x.PlanTypeId1,
                        principalTable: "PlanType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Min = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Max = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsEvaluation = table.Column<bool>(type: "bit", nullable: false),
                    Weight = table.Column<int>(type: "int", nullable: false),
                    FormId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScopeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DropDownType = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CalcMethodId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Items_CalcMethods_CalcMethodId",
                        column: x => x.CalcMethodId,
                        principalTable: "CalcMethods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Items_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Items_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Items_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Forms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRopric = table.Column<bool>(type: "bit", nullable: false),
                    HasOneValue = table.Column<bool>(type: "bit", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EvaluationPartyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HasEvaluation = table.Column<bool>(type: "bit", nullable: false),
                    CalcMethodId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FormStatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Forms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Forms_CalcMethods_CalcMethodId",
                        column: x => x.CalcMethodId,
                        principalTable: "CalcMethods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Forms_EvaluationParties_EvaluationPartyId",
                        column: x => x.EvaluationPartyId,
                        principalTable: "EvaluationParties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Forms_FormStatus_FormStatusId",
                        column: x => x.FormStatusId,
                        principalTable: "FormStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Forms_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Forms_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Forms_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Scopes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScopeTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderNo = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scopes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Scopes_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Scopes_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Scopes_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Scopes_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Scopes_ScopeTypes_ScopeTypeId",
                        column: x => x.ScopeTypeId,
                        principalTable: "ScopeTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NotificationTemplates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubjectAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubjectEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BodyAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BodyEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BackendName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SystemModuleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationTemplates_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NotificationTemplates_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NotificationTemplates_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NotificationTemplates_SystemModules_SystemModuleId",
                        column: x => x.SystemModuleId,
                        principalTable: "SystemModules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SystemModuleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BackendName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrefixCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReqNumberDef = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsAutoAssignEnabled = table.Column<bool>(type: "bit", nullable: true),
                    IsFreez = table.Column<bool>(type: "bit", nullable: false),
                    FreezDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderNo = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ServiceSettings = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Initialservice = table.Column<bool>(type: "bit", nullable: false),
                    UrlAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UrlEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShowInWebSite = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Services_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Services_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Services_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Services_SystemModules_SystemModuleId",
                        column: x => x.SystemModuleId,
                        principalTable: "SystemModules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChangeRequest",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestTypeId = table.Column<int>(type: "int", nullable: false),
                    ChangeRequestTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PlanId = table.Column<int>(type: "int", nullable: false),
                    PlanId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RequestedById = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangeRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChangeRequest_ChangeRequestType_ChangeRequestTypeId",
                        column: x => x.ChangeRequestTypeId,
                        principalTable: "ChangeRequestType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChangeRequest_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChangeRequest_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChangeRequest_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChangeRequest_MinistryUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChangeRequest_Plans_PlanId1",
                        column: x => x.PlanId1,
                        principalTable: "Plans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlanSchedules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SchoolId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlanId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanSchedules_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlanSchedules_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlanSchedules_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlanSchedules_OrgTree_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "OrgTree",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlanSchedules_Plans_PlanId",
                        column: x => x.PlanId,
                        principalTable: "Plans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlanSchedules_Plans_PlanId1",
                        column: x => x.PlanId1,
                        principalTable: "Plans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ItemValues",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemValues_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItemValues_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItemValues_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItemValues_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItemValues_MinistryUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AcademicYearScope",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScopeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademicYearScope", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AcademicYearScope_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AcademicYearScope_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AcademicYearScope_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AcademicYearScope_Scopes_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Scopes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AcademicYearScope_Scopes_ScopeId",
                        column: x => x.ScopeId,
                        principalTable: "Scopes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FormScopes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FormId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScopeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Wegiht = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormScopes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormScopes_Forms_FormId",
                        column: x => x.FormId,
                        principalTable: "Forms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FormScopes_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FormScopes_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FormScopes_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FormScopes_Scopes_ScopeId",
                        column: x => x.ScopeId,
                        principalTable: "Scopes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ScopeAcademicYears",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScopeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AcademicYearId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScopeAcademicYears", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScopeAcademicYears_AcademicYears_AcademicYearId",
                        column: x => x.AcademicYearId,
                        principalTable: "AcademicYears",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ScopeAcademicYears_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ScopeAcademicYears_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ScopeAcademicYears_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ScopeAcademicYears_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ScopeAcademicYears_Scopes_ScopeId",
                        column: x => x.ScopeId,
                        principalTable: "Scopes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ScopeUserTeam",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScopeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScopeUserTeam", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScopeUserTeam_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScopeUserTeam_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ScopeUserTeam_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ScopeUserTeam_MinistryUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScopeUserTeam_Scopes_ScopeId",
                        column: x => x.ScopeId,
                        principalTable: "Scopes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScopeUserTeam_Team_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Team",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTeamScope",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScopeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTeamScope", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTeamScope_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserTeamScope_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserTeamScope_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserTeamScope_MinistryUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserTeamScope_Scopes_ScopeId",
                        column: x => x.ScopeId,
                        principalTable: "Scopes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserTeamScope_Team_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Team",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NotificationTemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TitleAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TitleEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReadCount = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstReadDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastReadDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notifications_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notifications_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notifications_MinistryUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notifications_NotificationTemplates_NotificationTemplateId",
                        column: x => x.NotificationTemplateId,
                        principalTable: "NotificationTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FormGroup",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TitleAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TitleEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BackendName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    FormGroupTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FormGroupCustomListId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormGroup_FormGroupCustomList_FormGroupCustomListId",
                        column: x => x.FormGroupCustomListId,
                        principalTable: "FormGroupCustomList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FormGroup_FormGroupType_FormGroupTypeId",
                        column: x => x.FormGroupTypeId,
                        principalTable: "FormGroupType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FormGroup_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FormGroup_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FormGroup_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FormGroup_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlaceHolder",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlaceHolderName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeDisplay = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FieldId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ChildFieldIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ColumnName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaceHolder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlaceHolder_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlaceHolder_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PlaceHolder_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PlaceHolder_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServiceAction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BackendName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsConfirmationAction = table.Column<bool>(type: "bit", nullable: false),
                    ConfirmationTitleAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConfirmationTitleEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConfirmationBodyAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConfirmationBodyEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActionTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsInitialAction = table.Column<bool>(type: "bit", nullable: false),
                    IsAutoAssign = table.Column<bool>(type: "bit", nullable: false),
                    AllowDraft = table.Column<bool>(type: "bit", nullable: false),
                    NewStatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceAction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceAction_ActionType_ActionTypeId",
                        column: x => x.ActionTypeId,
                        principalTable: "ActionType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServiceAction_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServiceAction_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServiceAction_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServiceAction_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ServiceInitiatorPartyType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    serviceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PartyTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceInitiatorPartyType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceInitiatorPartyType_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceInitiatorPartyType_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ServiceInitiatorPartyType_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ServiceInitiatorPartyType_PartyTypes_PartyTypeId",
                        column: x => x.PartyTypeId,
                        principalTable: "PartyTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceInitiatorPartyType_Services_serviceId",
                        column: x => x.serviceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServiceRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequestNo = table.Column<int>(type: "int", nullable: false),
                    PlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceRequests_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServiceRequests_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServiceRequests_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServiceRequests_OrgTree_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "OrgTree",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServiceRequests_Plans_PlanId",
                        column: x => x.PlanId,
                        principalTable: "Plans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServiceRequests_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServiceRequests_StatusService_StatusId",
                        column: x => x.StatusId,
                        principalTable: "StatusService",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ServiceRequestShowPartyType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    serviceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PartyTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceRequestShowPartyType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceRequestShowPartyType_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServiceRequestShowPartyType_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServiceRequestShowPartyType_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServiceRequestShowPartyType_PartyTypes_PartyTypeId",
                        column: x => x.PartyTypeId,
                        principalTable: "PartyTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServiceRequestShowPartyType_Services_serviceId",
                        column: x => x.serviceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ServiceStatus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BackendName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsInitial = table.Column<bool>(type: "bit", nullable: false),
                    IsOpen = table.Column<bool>(type: "bit", nullable: false),
                    ColorCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderNo = table.Column<int>(type: "int", nullable: true),
                    ServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StatusGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceStatus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceStatus_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServiceStatus_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServiceStatus_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServiceStatus_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChangeRequestDetail",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChangeRequestId = table.Column<int>(type: "int", nullable: false),
                    ChangeRequestId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SchoolId = table.Column<int>(type: "int", nullable: false),
                    SchoolId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EvidenceDocument = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangeRequestDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChangeRequestDetail_ChangeRequest_ChangeRequestId1",
                        column: x => x.ChangeRequestId1,
                        principalTable: "ChangeRequest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChangeRequestDetail_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChangeRequestDetail_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ChangeRequestDetail_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ChangeRequestDetail_OrgTree_SchoolId1",
                        column: x => x.SchoolId1,
                        principalTable: "OrgTree",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Field",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TitleAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TitleEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BackendName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InfoAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InfoEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FieldTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Column = table.Column<int>(type: "int", nullable: false),
                    Row = table.Column<int>(type: "int", nullable: false),
                    FormGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FormGroupListId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ClassName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MappingFieldId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReadFieldId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DropDownParentFieldId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DropDownTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FormGroupCustomListId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Field", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Field_DropDownType_DropDownTypeId",
                        column: x => x.DropDownTypeId,
                        principalTable: "DropDownType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Field_FieldType_FieldTypeId",
                        column: x => x.FieldTypeId,
                        principalTable: "FieldType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Field_Field_DropDownParentFieldId",
                        column: x => x.DropDownParentFieldId,
                        principalTable: "Field",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Field_Field_MappingFieldId",
                        column: x => x.MappingFieldId,
                        principalTable: "Field",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Field_Field_ReadFieldId",
                        column: x => x.ReadFieldId,
                        principalTable: "Field",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Field_FormGroupCustomList_FormGroupCustomListId",
                        column: x => x.FormGroupCustomListId,
                        principalTable: "FormGroupCustomList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Field_FormGroup_FormGroupId",
                        column: x => x.FormGroupId,
                        principalTable: "FormGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Field_FormGroup_FormGroupListId",
                        column: x => x.FormGroupListId,
                        principalTable: "FormGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Field_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Field_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Field_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Field_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ActionAssignPartyType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EvaluationActionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PartyTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaximumAssignedUser = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionAssignPartyType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActionAssignPartyType_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActionAssignPartyType_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ActionAssignPartyType_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ActionAssignPartyType_PartyTypes_PartyTypeId",
                        column: x => x.PartyTypeId,
                        principalTable: "PartyTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActionAssignPartyType_ServiceAction_EvaluationActionId",
                        column: x => x.EvaluationActionId,
                        principalTable: "ServiceAction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActionCondition",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceActionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RefID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    operators = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FieldValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionCondition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActionCondition_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActionCondition_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ActionCondition_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ActionCondition_ServiceAction_ServiceActionId",
                        column: x => x.ServiceActionId,
                        principalTable: "ServiceAction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActionPartyType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceActionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PartyTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    ActionPartyTypeSettings = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionPartyType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActionPartyType_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActionPartyType_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ActionPartyType_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ActionPartyType_PartyTypes_PartyTypeId",
                        column: x => x.PartyTypeId,
                        principalTable: "PartyTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActionPartyType_ServiceAction_ServiceActionId",
                        column: x => x.ServiceActionId,
                        principalTable: "ServiceAction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActionShowLogPartyType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceActionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PartytypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionShowLogPartyType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActionShowLogPartyType_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActionShowLogPartyType_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActionShowLogPartyType_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActionShowLogPartyType_PartyTypes_PartytypeId",
                        column: x => x.PartytypeId,
                        principalTable: "PartyTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActionShowLogPartyType_ServiceAction_ServiceActionId",
                        column: x => x.ServiceActionId,
                        principalTable: "ServiceAction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ActionTemplateDoc",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceActionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TemplateDocId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionTemplateDoc", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActionTemplateDoc_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActionTemplateDoc_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActionTemplateDoc_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActionTemplateDoc_ServiceAction_ServiceActionId",
                        column: x => x.ServiceActionId,
                        principalTable: "ServiceAction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActionTemplateDoc_TemplateDocuments_TemplateDocId",
                        column: x => x.TemplateDocId,
                        principalTable: "TemplateDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ActionTransactionsLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceActionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RefId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SystemModuleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PreviousStatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NextStatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionTransactionsLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActionTransactionsLog_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActionTransactionsLog_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ActionTransactionsLog_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ActionTransactionsLog_ServiceAction_ServiceActionId",
                        column: x => x.ServiceActionId,
                        principalTable: "ServiceAction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActionTransactionsLog_SystemModules_SystemModuleId",
                        column: x => x.SystemModuleId,
                        principalTable: "SystemModules",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FieldValueTransactionsLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RefId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OldValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionsTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ServiceActionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldValueTransactionsLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FieldValueTransactionsLog_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FieldValueTransactionsLog_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FieldValueTransactionsLog_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FieldValueTransactionsLog_ServiceAction_ServiceActionId",
                        column: x => x.ServiceActionId,
                        principalTable: "ServiceAction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FieldValueTransactionsLog_TransactionType_TransactionsTypeId",
                        column: x => x.TransactionsTypeId,
                        principalTable: "TransactionType",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ActionStatusConfiguration",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceActionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrentStatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NextStatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsRemark = table.Column<bool>(type: "bit", nullable: false),
                    IsAuto = table.Column<bool>(type: "bit", nullable: false),
                    RemarkLabelAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AttachmentLabelAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RemarkLabelEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AttachmentLabelEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsRemarkRequired = table.Column<bool>(type: "bit", nullable: false),
                    IsOtherAttachment = table.Column<bool>(type: "bit", nullable: false),
                    IsOtherAttachmentRequired = table.Column<bool>(type: "bit", nullable: false),
                    OrderNo = table.Column<int>(type: "int", nullable: false),
                    ShowIsDefaultAssigner = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionStatusConfiguration", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActionStatusConfiguration_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActionStatusConfiguration_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActionStatusConfiguration_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActionStatusConfiguration_ServiceAction_ServiceActionId",
                        column: x => x.ServiceActionId,
                        principalTable: "ServiceAction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActionStatusConfiguration_ServiceStatus_CurrentStatusId",
                        column: x => x.CurrentStatusId,
                        principalTable: "ServiceStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActionStatusConfiguration_ServiceStatus_NextStatusId",
                        column: x => x.NextStatusId,
                        principalTable: "ServiceStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ServiceStatusPartyTypeDisplayName",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TitleAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TitleEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PartyTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceStatusPartyTypeDisplayName", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceStatusPartyTypeDisplayName_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServiceStatusPartyTypeDisplayName_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServiceStatusPartyTypeDisplayName_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServiceStatusPartyTypeDisplayName_PartyTypes_PartyTypeId",
                        column: x => x.PartyTypeId,
                        principalTable: "PartyTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServiceStatusPartyTypeDisplayName_ServiceStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "ServiceStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ServiceStatusPreventPartyType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PartyTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceStatusPreventPartyType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceStatusPreventPartyType_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServiceStatusPreventPartyType_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServiceStatusPreventPartyType_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServiceStatusPreventPartyType_PartyTypes_PartyTypeId",
                        column: x => x.PartyTypeId,
                        principalTable: "PartyTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServiceStatusPreventPartyType_ServiceStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "ServiceStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ActionField",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FieldId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceActionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsEditable = table.Column<bool>(type: "bit", nullable: false),
                    IsUpdateOnModule = table.Column<bool>(type: "bit", nullable: false),
                    OrderNo = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionField", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActionField_Field_FieldId",
                        column: x => x.FieldId,
                        principalTable: "Field",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActionField_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActionField_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActionField_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActionField_ServiceAction_ServiceActionId",
                        column: x => x.ServiceActionId,
                        principalTable: "ServiceAction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FieldAttributeValue",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FieldId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AttributeKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AttributeValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MessageAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MessageEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldAttributeValue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FieldAttributeValue_Field_FieldId",
                        column: x => x.FieldId,
                        principalTable: "Field",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FieldAttributeValue_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FieldAttributeValue_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FieldAttributeValue_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FieldPartyType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PartyTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FieldId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldPartyType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FieldPartyType_Field_FieldId",
                        column: x => x.FieldId,
                        principalTable: "Field",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FieldPartyType_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FieldPartyType_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FieldPartyType_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FieldPartyType_PartyTypes_PartyTypeId",
                        column: x => x.PartyTypeId,
                        principalTable: "PartyTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FieldViewCondition",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FieldId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    operators = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FieldValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsSufficient = table.Column<bool>(type: "bit", nullable: false),
                    FieldDropDownValueIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentFieldId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldViewCondition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FieldViewCondition_Field_FieldId",
                        column: x => x.FieldId,
                        principalTable: "Field",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FieldViewCondition_Field_ParentFieldId",
                        column: x => x.ParentFieldId,
                        principalTable: "Field",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FieldViewCondition_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FieldViewCondition_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FieldViewCondition_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FieldVisabilityConfig",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PartyTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FieldId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceStatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsShow = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldVisabilityConfig", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FieldVisabilityConfig_Field_FieldId",
                        column: x => x.FieldId,
                        principalTable: "Field",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FieldVisabilityConfig_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FieldVisabilityConfig_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FieldVisabilityConfig_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FieldVisabilityConfig_PartyTypes_PartyTypeId",
                        column: x => x.PartyTypeId,
                        principalTable: "PartyTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FieldVisabilityConfig_ServiceStatus_ServiceStatusId",
                        column: x => x.ServiceStatusId,
                        principalTable: "ServiceStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Attachment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActionTransactionsLogId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FieldId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ServiceRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UiFileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileExtension = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    IsOthers = table.Column<bool>(type: "bit", nullable: false),
                    ChildFieldId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Index = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attachment_ActionTransactionsLog_ActionTransactionsLogId",
                        column: x => x.ActionTransactionsLogId,
                        principalTable: "ActionTransactionsLog",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Attachment_Field_ChildFieldId",
                        column: x => x.ChildFieldId,
                        principalTable: "Field",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Attachment_Field_FieldId",
                        column: x => x.FieldId,
                        principalTable: "Field",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Attachment_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Attachment_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Attachment_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ActionStatusConfigNotification",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActionStatusConfigurationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PartyTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsEmailSend = table.Column<bool>(type: "bit", nullable: false),
                    EmailTemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsMessageSend = table.Column<bool>(type: "bit", nullable: false),
                    SMSTemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsNotificationSend = table.Column<bool>(type: "bit", nullable: false),
                    NotificationTemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionStatusConfigNotification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActionStatusConfigNotification_ActionStatusConfiguration_ActionStatusConfigurationId",
                        column: x => x.ActionStatusConfigurationId,
                        principalTable: "ActionStatusConfiguration",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActionStatusConfigNotification_EmailTemplates_EmailTemplateId",
                        column: x => x.EmailTemplateId,
                        principalTable: "EmailTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActionStatusConfigNotification_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActionStatusConfigNotification_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActionStatusConfigNotification_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActionStatusConfigNotification_NotificationTemplates_NotificationTemplateId",
                        column: x => x.NotificationTemplateId,
                        principalTable: "NotificationTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActionStatusConfigNotification_PartyTypes_PartyTypeId",
                        column: x => x.PartyTypeId,
                        principalTable: "PartyTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActionStatusConfigNotification_SMSTemplates_SMSTemplateId",
                        column: x => x.SMSTemplateId,
                        principalTable: "SMSTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ActionFieldAttribute",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FieldId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubFieldId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AttributeKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AttributeValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MessageAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MessageEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActionStepFieldId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ActionFieldId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdateById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionFieldAttribute", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActionFieldAttribute_ActionField_ActionFieldId",
                        column: x => x.ActionFieldId,
                        principalTable: "ActionField",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActionFieldAttribute_Field_FieldId",
                        column: x => x.FieldId,
                        principalTable: "Field",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActionFieldAttribute_Field_SubFieldId",
                        column: x => x.SubFieldId,
                        principalTable: "Field",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActionFieldAttribute_MinistryUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActionFieldAttribute_MinistryUsers_DeleteById",
                        column: x => x.DeleteById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActionFieldAttribute_MinistryUsers_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "MinistryUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AcademicYears_CreateById",
                table: "AcademicYears",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_AcademicYears_DeleteById",
                table: "AcademicYears",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_AcademicYears_DepartmentId_Year",
                table: "AcademicYears",
                columns: new[] { "DepartmentId", "Year" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AcademicYears_UpdateById",
                table: "AcademicYears",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_AcademicYearScope_CreateById",
                table: "AcademicYearScope",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_AcademicYearScope_DeleteById",
                table: "AcademicYearScope",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_AcademicYearScope_ParentId",
                table: "AcademicYearScope",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_AcademicYearScope_ScopeId",
                table: "AcademicYearScope",
                column: "ScopeId");

            migrationBuilder.CreateIndex(
                name: "IX_AcademicYearScope_UpdateById",
                table: "AcademicYearScope",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_ActionAssignPartyType_CreateById",
                table: "ActionAssignPartyType",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_ActionAssignPartyType_DeleteById",
                table: "ActionAssignPartyType",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_ActionAssignPartyType_EvaluationActionId",
                table: "ActionAssignPartyType",
                column: "EvaluationActionId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionAssignPartyType_PartyTypeId",
                table: "ActionAssignPartyType",
                column: "PartyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionAssignPartyType_UpdateById",
                table: "ActionAssignPartyType",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_ActionCondition_CreateById",
                table: "ActionCondition",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_ActionCondition_DeleteById",
                table: "ActionCondition",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_ActionCondition_ServiceActionId",
                table: "ActionCondition",
                column: "ServiceActionId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionCondition_UpdateById",
                table: "ActionCondition",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_ActionField_CreateById",
                table: "ActionField",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_ActionField_DeleteById",
                table: "ActionField",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_ActionField_FieldId",
                table: "ActionField",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionField_ServiceActionId",
                table: "ActionField",
                column: "ServiceActionId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionField_UpdateById",
                table: "ActionField",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_ActionFieldAttribute_ActionFieldId",
                table: "ActionFieldAttribute",
                column: "ActionFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionFieldAttribute_CreateById",
                table: "ActionFieldAttribute",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_ActionFieldAttribute_DeleteById",
                table: "ActionFieldAttribute",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_ActionFieldAttribute_FieldId",
                table: "ActionFieldAttribute",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionFieldAttribute_SubFieldId",
                table: "ActionFieldAttribute",
                column: "SubFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionFieldAttribute_UpdateById",
                table: "ActionFieldAttribute",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_ActionPartyType_CreateById",
                table: "ActionPartyType",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_ActionPartyType_DeleteById",
                table: "ActionPartyType",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_ActionPartyType_PartyTypeId",
                table: "ActionPartyType",
                column: "PartyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionPartyType_ServiceActionId",
                table: "ActionPartyType",
                column: "ServiceActionId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionPartyType_UpdateById",
                table: "ActionPartyType",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_ActionShowLogPartyType_CreateById",
                table: "ActionShowLogPartyType",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_ActionShowLogPartyType_DeleteById",
                table: "ActionShowLogPartyType",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_ActionShowLogPartyType_PartytypeId",
                table: "ActionShowLogPartyType",
                column: "PartytypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionShowLogPartyType_ServiceActionId",
                table: "ActionShowLogPartyType",
                column: "ServiceActionId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionShowLogPartyType_UpdateById",
                table: "ActionShowLogPartyType",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_ActionStatusConfigNotification_ActionStatusConfigurationId",
                table: "ActionStatusConfigNotification",
                column: "ActionStatusConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionStatusConfigNotification_CreateById",
                table: "ActionStatusConfigNotification",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_ActionStatusConfigNotification_DeleteById",
                table: "ActionStatusConfigNotification",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_ActionStatusConfigNotification_EmailTemplateId",
                table: "ActionStatusConfigNotification",
                column: "EmailTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionStatusConfigNotification_NotificationTemplateId",
                table: "ActionStatusConfigNotification",
                column: "NotificationTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionStatusConfigNotification_PartyTypeId",
                table: "ActionStatusConfigNotification",
                column: "PartyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionStatusConfigNotification_SMSTemplateId",
                table: "ActionStatusConfigNotification",
                column: "SMSTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionStatusConfigNotification_UpdateById",
                table: "ActionStatusConfigNotification",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_ActionStatusConfiguration_CreateById",
                table: "ActionStatusConfiguration",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_ActionStatusConfiguration_CurrentStatusId",
                table: "ActionStatusConfiguration",
                column: "CurrentStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionStatusConfiguration_DeleteById",
                table: "ActionStatusConfiguration",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_ActionStatusConfiguration_NextStatusId",
                table: "ActionStatusConfiguration",
                column: "NextStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionStatusConfiguration_ServiceActionId",
                table: "ActionStatusConfiguration",
                column: "ServiceActionId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionStatusConfiguration_UpdateById",
                table: "ActionStatusConfiguration",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_ActionTemplateDoc_CreateById",
                table: "ActionTemplateDoc",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_ActionTemplateDoc_DeleteById",
                table: "ActionTemplateDoc",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_ActionTemplateDoc_ServiceActionId",
                table: "ActionTemplateDoc",
                column: "ServiceActionId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionTemplateDoc_TemplateDocId",
                table: "ActionTemplateDoc",
                column: "TemplateDocId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionTemplateDoc_UpdateById",
                table: "ActionTemplateDoc",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_ActionTransactionsLog_CreateById",
                table: "ActionTransactionsLog",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_ActionTransactionsLog_DeleteById",
                table: "ActionTransactionsLog",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_ActionTransactionsLog_ServiceActionId",
                table: "ActionTransactionsLog",
                column: "ServiceActionId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionTransactionsLog_SystemModuleId",
                table: "ActionTransactionsLog",
                column: "SystemModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionTransactionsLog_UpdateById",
                table: "ActionTransactionsLog",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_ActionType_BackendName",
                table: "ActionType",
                column: "BackendName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ActionType_CreateById",
                table: "ActionType",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_ActionType_DeleteById",
                table: "ActionType",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_ActionType_UpdateById",
                table: "ActionType",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_Attachment_ActionTransactionsLogId",
                table: "Attachment",
                column: "ActionTransactionsLogId");

            migrationBuilder.CreateIndex(
                name: "IX_Attachment_ChildFieldId",
                table: "Attachment",
                column: "ChildFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_Attachment_CreateById",
                table: "Attachment",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_Attachment_DeleteById",
                table: "Attachment",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_Attachment_FieldId",
                table: "Attachment",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_Attachment_UpdateById",
                table: "Attachment",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_Attribute_CreateById",
                table: "Attribute",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_Attribute_DeleteById",
                table: "Attribute",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_Attribute_UpdateById",
                table: "Attribute",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_CreateById",
                table: "AuditLogs",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_DeleteById",
                table: "AuditLogs",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_UpdateById",
                table: "AuditLogs",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_Banner_CreateById",
                table: "Banner",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_Banner_DeleteById",
                table: "Banner",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_Banner_UpdateById",
                table: "Banner",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_CalcMethods_CreateById",
                table: "CalcMethods",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_CalcMethods_DeleteById",
                table: "CalcMethods",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_CalcMethods_DepartmentId",
                table: "CalcMethods",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_CalcMethods_UpdateById",
                table: "CalcMethods",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_Category_CreateById",
                table: "Category",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_Category_DeleteById",
                table: "Category",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_Category_UpdateById",
                table: "Category",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeRequest_ChangeRequestTypeId",
                table: "ChangeRequest",
                column: "ChangeRequestTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeRequest_CreateById",
                table: "ChangeRequest",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeRequest_DeleteById",
                table: "ChangeRequest",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeRequest_PlanId1",
                table: "ChangeRequest",
                column: "PlanId1");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeRequest_UpdateById",
                table: "ChangeRequest",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeRequest_UserId",
                table: "ChangeRequest",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeRequestDetail_ChangeRequestId1",
                table: "ChangeRequestDetail",
                column: "ChangeRequestId1");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeRequestDetail_CreateById",
                table: "ChangeRequestDetail",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeRequestDetail_DeleteById",
                table: "ChangeRequestDetail",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeRequestDetail_SchoolId1",
                table: "ChangeRequestDetail",
                column: "SchoolId1");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeRequestDetail_UpdateById",
                table: "ChangeRequestDetail",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeRequestStatus_CreateById",
                table: "ChangeRequestStatus",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeRequestStatus_DeleteById",
                table: "ChangeRequestStatus",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeRequestStatus_UpdateById",
                table: "ChangeRequestStatus",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeRequestType_AcademicYearId1",
                table: "ChangeRequestType",
                column: "AcademicYearId1");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeRequestType_CreateById",
                table: "ChangeRequestType",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeRequestType_DeleteById",
                table: "ChangeRequestType",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeRequestType_DepartmentId1",
                table: "ChangeRequestType",
                column: "DepartmentId1");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeRequestType_UpdateById",
                table: "ChangeRequestType",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_ControlValidations_CreateById",
                table: "ControlValidations",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_ControlValidations_DeleteById",
                table: "ControlValidations",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_ControlValidations_PermissionId",
                table: "ControlValidations",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_ControlValidations_UpdateById",
                table: "ControlValidations",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_CssApplyType_BackendName",
                table: "CssApplyType",
                column: "BackendName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CssApplyType_CreateById",
                table: "CssApplyType",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_CssApplyType_DeleteById",
                table: "CssApplyType",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_CssApplyType_UpdateById",
                table: "CssApplyType",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_CssClass_ApplyTypeId",
                table: "CssClass",
                column: "ApplyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CssClass_CreateById",
                table: "CssClass",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_CssClass_DeleteById",
                table: "CssClass",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_CssClass_UpdateById",
                table: "CssClass",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentEvaluationParty_CreateById",
                table: "DepartmentEvaluationParty",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentEvaluationParty_DeleteById",
                table: "DepartmentEvaluationParty",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentEvaluationParty_UpdateById",
                table: "DepartmentEvaluationParty",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentHoliday_AcademicYearId",
                table: "DepartmentHoliday",
                column: "AcademicYearId");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentHoliday_CreateById",
                table: "DepartmentHoliday",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentHoliday_DeleteById",
                table: "DepartmentHoliday",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentHoliday_DepartmentId",
                table: "DepartmentHoliday",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentHoliday_UpdateById",
                table: "DepartmentHoliday",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentOrgTrees_CreateById",
                table: "DepartmentOrgTrees",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentOrgTrees_DeleteById",
                table: "DepartmentOrgTrees",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentOrgTrees_DepartmentId",
                table: "DepartmentOrgTrees",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentOrgTrees_OrgTreeId",
                table: "DepartmentOrgTrees",
                column: "OrgTreeId");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentOrgTrees_UpdateById",
                table: "DepartmentOrgTrees",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentRoleAttributeValue_CreateById",
                table: "DepartmentRoleAttributeValue",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentRoleAttributeValue_DeleteById",
                table: "DepartmentRoleAttributeValue",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentRoleAttributeValue_UpdateById",
                table: "DepartmentRoleAttributeValue",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_CategoryId",
                table: "Departments",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_CreateById",
                table: "Departments",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_DeleteById",
                table: "Departments",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_OrganizationTypeId",
                table: "Departments",
                column: "OrganizationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_UpdateById",
                table: "Departments",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_DropDownType_BackendName",
                table: "DropDownType",
                column: "BackendName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DropDownType_CreateById",
                table: "DropDownType",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_DropDownType_DeleteById",
                table: "DropDownType",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_DropDownType_ParentId",
                table: "DropDownType",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_DropDownType_UpdateById",
                table: "DropDownType",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_EmailLog_CreateById",
                table: "EmailLog",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_EmailLog_DeleteById",
                table: "EmailLog",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_EmailLog_UpdateById",
                table: "EmailLog",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_EmailTemplateDocuments_CreateById",
                table: "EmailTemplateDocuments",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_EmailTemplateDocuments_DeleteById",
                table: "EmailTemplateDocuments",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_EmailTemplateDocuments_EmailTemplateId",
                table: "EmailTemplateDocuments",
                column: "EmailTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailTemplateDocuments_TemplateDocumentId",
                table: "EmailTemplateDocuments",
                column: "TemplateDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailTemplateDocuments_UpdateById",
                table: "EmailTemplateDocuments",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_EmailTemplates_BackendName",
                table: "EmailTemplates",
                column: "BackendName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmailTemplates_CreateById",
                table: "EmailTemplates",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_EmailTemplates_DeleteById",
                table: "EmailTemplates",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_EmailTemplates_UpdateById",
                table: "EmailTemplates",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_EvaluationParties_CreateById",
                table: "EvaluationParties",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_EvaluationParties_DeleteById",
                table: "EvaluationParties",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_EvaluationParties_DepartmentId",
                table: "EvaluationParties",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_EvaluationParties_UpdateById",
                table: "EvaluationParties",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_ExceptionLog_CreateById",
                table: "ExceptionLog",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_ExceptionLog_DeleteById",
                table: "ExceptionLog",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_ExceptionLog_UpdateById",
                table: "ExceptionLog",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_Field_CreateById",
                table: "Field",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_Field_DeleteById",
                table: "Field",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_Field_DropDownParentFieldId",
                table: "Field",
                column: "DropDownParentFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_Field_DropDownTypeId",
                table: "Field",
                column: "DropDownTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Field_FieldTypeId",
                table: "Field",
                column: "FieldTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Field_FormGroupCustomListId",
                table: "Field",
                column: "FormGroupCustomListId");

            migrationBuilder.CreateIndex(
                name: "IX_Field_FormGroupId",
                table: "Field",
                column: "FormGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Field_FormGroupListId",
                table: "Field",
                column: "FormGroupListId");

            migrationBuilder.CreateIndex(
                name: "IX_Field_MappingFieldId",
                table: "Field",
                column: "MappingFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_Field_ReadFieldId",
                table: "Field",
                column: "ReadFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_Field_ServiceId",
                table: "Field",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Field_UpdateById",
                table: "Field",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_FieldAttributeValue_CreateById",
                table: "FieldAttributeValue",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_FieldAttributeValue_DeleteById",
                table: "FieldAttributeValue",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_FieldAttributeValue_FieldId",
                table: "FieldAttributeValue",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldAttributeValue_UpdateById",
                table: "FieldAttributeValue",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_FieldDropDownValue_CreateById",
                table: "FieldDropDownValue",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_FieldDropDownValue_DeleteById",
                table: "FieldDropDownValue",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_FieldDropDownValue_DropDownTypeId",
                table: "FieldDropDownValue",
                column: "DropDownTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldDropDownValue_ParentDropDownId",
                table: "FieldDropDownValue",
                column: "ParentDropDownId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldDropDownValue_UpdateById",
                table: "FieldDropDownValue",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_FieldPartyType_CreateById",
                table: "FieldPartyType",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_FieldPartyType_DeleteById",
                table: "FieldPartyType",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_FieldPartyType_FieldId",
                table: "FieldPartyType",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldPartyType_PartyTypeId",
                table: "FieldPartyType",
                column: "PartyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldPartyType_UpdateById",
                table: "FieldPartyType",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_FieldType_CreateById",
                table: "FieldType",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_FieldType_DeleteById",
                table: "FieldType",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_FieldType_ShowTypeId",
                table: "FieldType",
                column: "ShowTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldType_UpdateById",
                table: "FieldType",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_FieldTypeShow_CreateById",
                table: "FieldTypeShow",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_FieldTypeShow_DeleteById",
                table: "FieldTypeShow",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_FieldTypeShow_UpdateById",
                table: "FieldTypeShow",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_FieldValueTransactionsLog_CreateById",
                table: "FieldValueTransactionsLog",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_FieldValueTransactionsLog_DeleteById",
                table: "FieldValueTransactionsLog",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_FieldValueTransactionsLog_ServiceActionId",
                table: "FieldValueTransactionsLog",
                column: "ServiceActionId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldValueTransactionsLog_TransactionsTypeId",
                table: "FieldValueTransactionsLog",
                column: "TransactionsTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldValueTransactionsLog_UpdateById",
                table: "FieldValueTransactionsLog",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_FieldViewCondition_CreateById",
                table: "FieldViewCondition",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_FieldViewCondition_DeleteById",
                table: "FieldViewCondition",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_FieldViewCondition_FieldId",
                table: "FieldViewCondition",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldViewCondition_ParentFieldId",
                table: "FieldViewCondition",
                column: "ParentFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldViewCondition_UpdateById",
                table: "FieldViewCondition",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_FieldVisabilityConfig_CreateById",
                table: "FieldVisabilityConfig",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_FieldVisabilityConfig_DeleteById",
                table: "FieldVisabilityConfig",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_FieldVisabilityConfig_FieldId",
                table: "FieldVisabilityConfig",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldVisabilityConfig_PartyTypeId",
                table: "FieldVisabilityConfig",
                column: "PartyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldVisabilityConfig_ServiceStatusId",
                table: "FieldVisabilityConfig",
                column: "ServiceStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldVisabilityConfig_UpdateById",
                table: "FieldVisabilityConfig",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_FormGroup_CreateById",
                table: "FormGroup",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_FormGroup_DeleteById",
                table: "FormGroup",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_FormGroup_FormGroupCustomListId",
                table: "FormGroup",
                column: "FormGroupCustomListId");

            migrationBuilder.CreateIndex(
                name: "IX_FormGroup_FormGroupTypeId",
                table: "FormGroup",
                column: "FormGroupTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FormGroup_ServiceId",
                table: "FormGroup",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_FormGroup_UpdateById",
                table: "FormGroup",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_FormGroupCustomList_CreateById",
                table: "FormGroupCustomList",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_FormGroupCustomList_DeleteById",
                table: "FormGroupCustomList",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_FormGroupCustomList_UpdateById",
                table: "FormGroupCustomList",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_FormGroupType_CreateById",
                table: "FormGroupType",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_FormGroupType_DeleteById",
                table: "FormGroupType",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_FormGroupType_UpdateById",
                table: "FormGroupType",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_Forms_CalcMethodId",
                table: "Forms",
                column: "CalcMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_Forms_CreateById",
                table: "Forms",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_Forms_DeleteById",
                table: "Forms",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_Forms_EvaluationPartyId",
                table: "Forms",
                column: "EvaluationPartyId");

            migrationBuilder.CreateIndex(
                name: "IX_Forms_FormStatusId",
                table: "Forms",
                column: "FormStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Forms_UpdateById",
                table: "Forms",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_FormScopes_CreateById",
                table: "FormScopes",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_FormScopes_DeleteById",
                table: "FormScopes",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_FormScopes_FormId",
                table: "FormScopes",
                column: "FormId");

            migrationBuilder.CreateIndex(
                name: "IX_FormScopes_ScopeId",
                table: "FormScopes",
                column: "ScopeId");

            migrationBuilder.CreateIndex(
                name: "IX_FormScopes_UpdateById",
                table: "FormScopes",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_FormStatus_CreateById",
                table: "FormStatus",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_FormStatus_DeleteById",
                table: "FormStatus",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_FormStatus_UpdateById",
                table: "FormStatus",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_Items_CalcMethodId",
                table: "Items",
                column: "CalcMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_CreateById",
                table: "Items",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_Items_DeleteById",
                table: "Items",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_Items_UpdateById",
                table: "Items",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_ItemValues_CreateById",
                table: "ItemValues",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_ItemValues_DeleteById",
                table: "ItemValues",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_ItemValues_ItemId",
                table: "ItemValues",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemValues_UpdateById",
                table: "ItemValues",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_ItemValues_UserId",
                table: "ItemValues",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Level_CreateById",
                table: "Level",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_Level_DeleteById",
                table: "Level",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_Level_UpdateById",
                table: "Level",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_MinistryUsers_CreateById",
                table: "MinistryUsers",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_MinistryUsers_DeleteById",
                table: "MinistryUsers",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_MinistryUsers_QID",
                table: "MinistryUsers",
                column: "QID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MinistryUsers_UpdateById",
                table: "MinistryUsers",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_ModuleType_CreateById",
                table: "ModuleType",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_ModuleType_DeleteById",
                table: "ModuleType",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_ModuleType_ParentId",
                table: "ModuleType",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ModuleType_UpdateById",
                table: "ModuleType",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_Navbar_CreateById",
                table: "Navbar",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_Navbar_DeleteById",
                table: "Navbar",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_Navbar_ParentId",
                table: "Navbar",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Navbar_PermissionId",
                table: "Navbar",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_Navbar_UpdateById",
                table: "Navbar",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_CreateById",
                table: "Notifications",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_DeleteById",
                table: "Notifications",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_NotificationTemplateId",
                table: "Notifications",
                column: "NotificationTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UpdateById",
                table: "Notifications",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationTemplates_BackendName",
                table: "NotificationTemplates",
                column: "BackendName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NotificationTemplates_CreateById",
                table: "NotificationTemplates",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationTemplates_DeleteById",
                table: "NotificationTemplates",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationTemplates_SystemModuleId",
                table: "NotificationTemplates",
                column: "SystemModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationTemplates_UpdateById",
                table: "NotificationTemplates",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_OrgAcademicYears_AcademicYearId",
                table: "OrgAcademicYears",
                column: "AcademicYearId");

            migrationBuilder.CreateIndex(
                name: "IX_OrgAcademicYears_CreateById",
                table: "OrgAcademicYears",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_OrgAcademicYears_DeleteById",
                table: "OrgAcademicYears",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_OrgAcademicYears_OrganizationId",
                table: "OrgAcademicYears",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_OrgAcademicYears_ParentOrgId",
                table: "OrgAcademicYears",
                column: "ParentOrgId");

            migrationBuilder.CreateIndex(
                name: "IX_OrgAcademicYears_UpdateById",
                table: "OrgAcademicYears",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationTypes_CreateById",
                table: "OrganizationTypes",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationTypes_DeleteById",
                table: "OrganizationTypes",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationTypes_UpdateById",
                table: "OrganizationTypes",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_OrgTree_CreateById",
                table: "OrgTree",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_OrgTree_DeleteById",
                table: "OrgTree",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_OrgTree_OrganizationTypeId",
                table: "OrgTree",
                column: "OrganizationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_OrgTree_OrgTreeClassId",
                table: "OrgTree",
                column: "OrgTreeClassId");

            migrationBuilder.CreateIndex(
                name: "IX_OrgTree_ParentId",
                table: "OrgTree",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_OrgTree_SchoolTypeId",
                table: "OrgTree",
                column: "SchoolTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_OrgTree_UpdateById",
                table: "OrgTree",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_OrgTreeClass_BackendName",
                table: "OrgTreeClass",
                column: "BackendName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrgTreeClass_CreateById",
                table: "OrgTreeClass",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_OrgTreeClass_DeleteById",
                table: "OrgTreeClass",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_OrgTreeClass_ParentId",
                table: "OrgTreeClass",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_OrgTreeClass_UpdateById",
                table: "OrgTreeClass",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_PagePermissions_CreateById",
                table: "PagePermissions",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_PagePermissions_DeleteById",
                table: "PagePermissions",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_PagePermissions_PageId",
                table: "PagePermissions",
                column: "PageId");

            migrationBuilder.CreateIndex(
                name: "IX_PagePermissions_PermissionId",
                table: "PagePermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_PagePermissions_UpdateById",
                table: "PagePermissions",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_Pages_BackendName",
                table: "Pages",
                column: "BackendName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pages_CreateById",
                table: "Pages",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_Pages_DeleteById",
                table: "Pages",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_Pages_UpdateById",
                table: "Pages",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_PartyTypes_BackendName",
                table: "PartyTypes",
                column: "BackendName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PartyTypes_CreateById",
                table: "PartyTypes",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_PartyTypes_DeleteById",
                table: "PartyTypes",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_PartyTypes_UpdateById",
                table: "PartyTypes",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_BackendName",
                table: "Permissions",
                column: "BackendName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_CreateById",
                table: "Permissions",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_DeleteById",
                table: "Permissions",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_UpdateById",
                table: "Permissions",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_PlaceHolder_CreateById",
                table: "PlaceHolder",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_PlaceHolder_DeleteById",
                table: "PlaceHolder",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_PlaceHolder_ServiceId",
                table: "PlaceHolder",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaceHolder_UpdateById",
                table: "PlaceHolder",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_Plans_AcademicYearId1",
                table: "Plans",
                column: "AcademicYearId1");

            migrationBuilder.CreateIndex(
                name: "IX_Plans_CreateById",
                table: "Plans",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_Plans_DeleteById",
                table: "Plans",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_Plans_DepartmentId1",
                table: "Plans",
                column: "DepartmentId1");

            migrationBuilder.CreateIndex(
                name: "IX_Plans_PlanStatusId1",
                table: "Plans",
                column: "PlanStatusId1");

            migrationBuilder.CreateIndex(
                name: "IX_Plans_PlanTypeId1",
                table: "Plans",
                column: "PlanTypeId1");

            migrationBuilder.CreateIndex(
                name: "IX_Plans_UpdateById",
                table: "Plans",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_PlanSchedules_CreateById",
                table: "PlanSchedules",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_PlanSchedules_DeleteById",
                table: "PlanSchedules",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_PlanSchedules_PlanId",
                table: "PlanSchedules",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanSchedules_PlanId1",
                table: "PlanSchedules",
                column: "PlanId1",
                unique: true,
                filter: "[PlanId1] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PlanSchedules_SchoolId",
                table: "PlanSchedules",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanSchedules_UpdateById",
                table: "PlanSchedules",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_PlanStatuses_CreateById",
                table: "PlanStatuses",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_PlanStatuses_DeleteById",
                table: "PlanStatuses",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_PlanStatuses_UpdateById",
                table: "PlanStatuses",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_PlanType_BackendName",
                table: "PlanType",
                column: "BackendName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlanType_CreateById",
                table: "PlanType",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_PlanType_DeleteById",
                table: "PlanType",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_PlanType_UpdateById",
                table: "PlanType",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_PlanTypeDepartment_CreateById",
                table: "PlanTypeDepartment",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_PlanTypeDepartment_DeleteById",
                table: "PlanTypeDepartment",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_PlanTypeDepartment_DepartmentId",
                table: "PlanTypeDepartment",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanTypeDepartment_PlanTypeId",
                table: "PlanTypeDepartment",
                column: "PlanTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanTypeDepartment_UpdateById",
                table: "PlanTypeDepartment",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_RelatedFieldsGroup_CreateById",
                table: "RelatedFieldsGroup",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_RelatedFieldsGroup_DeleteById",
                table: "RelatedFieldsGroup",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_RelatedFieldsGroup_UpdateById",
                table: "RelatedFieldsGroup",
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

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_CreateById",
                table: "RolePermissions",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_DeleteById",
                table: "RolePermissions",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_PermissionId",
                table: "RolePermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_RoleId",
                table: "RolePermissions",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_UpdateById",
                table: "RolePermissions",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_CreateById",
                table: "Roles",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_DeleteById",
                table: "Roles",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_UpdateById",
                table: "Roles",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolLevel_CreateById",
                table: "SchoolLevel",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolLevel_DeleteById",
                table: "SchoolLevel",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolLevel_LevelId1",
                table: "SchoolLevel",
                column: "LevelId1");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolLevel_SchoolId1",
                table: "SchoolLevel",
                column: "SchoolId1");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolLevel_UpdateById",
                table: "SchoolLevel",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolTypes_CreateById",
                table: "SchoolTypes",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolTypes_DeleteById",
                table: "SchoolTypes",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolTypes_UpdateById",
                table: "SchoolTypes",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_ScopeAcademicYears_AcademicYearId",
                table: "ScopeAcademicYears",
                column: "AcademicYearId");

            migrationBuilder.CreateIndex(
                name: "IX_ScopeAcademicYears_CreateById",
                table: "ScopeAcademicYears",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_ScopeAcademicYears_DeleteById",
                table: "ScopeAcademicYears",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_ScopeAcademicYears_DepartmentId",
                table: "ScopeAcademicYears",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ScopeAcademicYears_ScopeId",
                table: "ScopeAcademicYears",
                column: "ScopeId");

            migrationBuilder.CreateIndex(
                name: "IX_ScopeAcademicYears_UpdateById",
                table: "ScopeAcademicYears",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_Scopes_CreateById",
                table: "Scopes",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_Scopes_DeleteById",
                table: "Scopes",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_Scopes_DepartmentId",
                table: "Scopes",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Scopes_ScopeTypeId",
                table: "Scopes",
                column: "ScopeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Scopes_UpdateById",
                table: "Scopes",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_ScopeTypes_CreateById",
                table: "ScopeTypes",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_ScopeTypes_DeleteById",
                table: "ScopeTypes",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_ScopeTypes_DepartmentId",
                table: "ScopeTypes",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ScopeTypes_ParentId",
                table: "ScopeTypes",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ScopeTypes_UpdateById",
                table: "ScopeTypes",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_ScopeUserTeam_CreateById",
                table: "ScopeUserTeam",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_ScopeUserTeam_DeleteById",
                table: "ScopeUserTeam",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_ScopeUserTeam_ScopeId",
                table: "ScopeUserTeam",
                column: "ScopeId");

            migrationBuilder.CreateIndex(
                name: "IX_ScopeUserTeam_TeamId",
                table: "ScopeUserTeam",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_ScopeUserTeam_UpdateById",
                table: "ScopeUserTeam",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_ScopeUserTeam_UserId",
                table: "ScopeUserTeam",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SectionDepartment_CreateById",
                table: "SectionDepartment",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_SectionDepartment_DeleteById",
                table: "SectionDepartment",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_SectionDepartment_UpdateById",
                table: "SectionDepartment",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceAction_ActionTypeId",
                table: "ServiceAction",
                column: "ActionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceAction_BackendName",
                table: "ServiceAction",
                column: "BackendName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServiceAction_CreateById",
                table: "ServiceAction",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceAction_DeleteById",
                table: "ServiceAction",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceAction_ServiceId",
                table: "ServiceAction",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceAction_UpdateById",
                table: "ServiceAction",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceInitiatorPartyType_CreateById",
                table: "ServiceInitiatorPartyType",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceInitiatorPartyType_DeleteById",
                table: "ServiceInitiatorPartyType",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceInitiatorPartyType_PartyTypeId",
                table: "ServiceInitiatorPartyType",
                column: "PartyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceInitiatorPartyType_serviceId",
                table: "ServiceInitiatorPartyType",
                column: "serviceId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceInitiatorPartyType_UpdateById",
                table: "ServiceInitiatorPartyType",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequests_CreateById",
                table: "ServiceRequests",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequests_DeleteById",
                table: "ServiceRequests",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequests_OrganizationId",
                table: "ServiceRequests",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequests_PlanId",
                table: "ServiceRequests",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequests_ServiceId",
                table: "ServiceRequests",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequests_StatusId",
                table: "ServiceRequests",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequests_UpdateById",
                table: "ServiceRequests",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequestShowPartyType_CreateById",
                table: "ServiceRequestShowPartyType",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequestShowPartyType_DeleteById",
                table: "ServiceRequestShowPartyType",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequestShowPartyType_PartyTypeId",
                table: "ServiceRequestShowPartyType",
                column: "PartyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequestShowPartyType_serviceId",
                table: "ServiceRequestShowPartyType",
                column: "serviceId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequestShowPartyType_UpdateById",
                table: "ServiceRequestShowPartyType",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_Services_CreateById",
                table: "Services",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_Services_DeleteById",
                table: "Services",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_Services_SystemModuleId",
                table: "Services",
                column: "SystemModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_UpdateById",
                table: "Services",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceStatus_CreateById",
                table: "ServiceStatus",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceStatus_DeleteById",
                table: "ServiceStatus",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceStatus_ServiceId",
                table: "ServiceStatus",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceStatus_UpdateById",
                table: "ServiceStatus",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceStatusPartyTypeDisplayName_CreateById",
                table: "ServiceStatusPartyTypeDisplayName",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceStatusPartyTypeDisplayName_DeleteById",
                table: "ServiceStatusPartyTypeDisplayName",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceStatusPartyTypeDisplayName_PartyTypeId",
                table: "ServiceStatusPartyTypeDisplayName",
                column: "PartyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceStatusPartyTypeDisplayName_StatusId",
                table: "ServiceStatusPartyTypeDisplayName",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceStatusPartyTypeDisplayName_UpdateById",
                table: "ServiceStatusPartyTypeDisplayName",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceStatusPreventPartyType_CreateById",
                table: "ServiceStatusPreventPartyType",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceStatusPreventPartyType_DeleteById",
                table: "ServiceStatusPreventPartyType",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceStatusPreventPartyType_PartyTypeId",
                table: "ServiceStatusPreventPartyType",
                column: "PartyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceStatusPreventPartyType_StatusId",
                table: "ServiceStatusPreventPartyType",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceStatusPreventPartyType_UpdateById",
                table: "ServiceStatusPreventPartyType",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_SideBar_CreateById",
                table: "SideBar",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_SideBar_DeleteById",
                table: "SideBar",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_SideBar_ParentId",
                table: "SideBar",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_SideBar_PermissionId",
                table: "SideBar",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_SideBar_UpdateById",
                table: "SideBar",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_SiteContent_CreateById",
                table: "SiteContent",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_SiteContent_DeleteById",
                table: "SiteContent",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_SiteContent_NavbarId",
                table: "SiteContent",
                column: "NavbarId");

            migrationBuilder.CreateIndex(
                name: "IX_SiteContent_ParentId",
                table: "SiteContent",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_SiteContent_UpdateById",
                table: "SiteContent",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_SiteDocument_CreateById",
                table: "SiteDocument",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_SiteDocument_DeleteById",
                table: "SiteDocument",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_SiteDocument_UpdateById",
                table: "SiteDocument",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_SMSLog_CreateById",
                table: "SMSLog",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_SMSLog_DeleteById",
                table: "SMSLog",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_SMSLog_UpdateById",
                table: "SMSLog",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_SMSProfile_BackendName",
                table: "SMSProfile",
                column: "BackendName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SMSProfile_CreateById",
                table: "SMSProfile",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_SMSProfile_DeleteById",
                table: "SMSProfile",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_SMSProfile_UpdateById",
                table: "SMSProfile",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_SMSProfiles_CreateById",
                table: "SMSProfiles",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_SMSProfiles_DeleteById",
                table: "SMSProfiles",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_SMSProfiles_UpdateById",
                table: "SMSProfiles",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_SMSTemplates_BackendName",
                table: "SMSTemplates",
                column: "BackendName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SMSTemplates_CreateById",
                table: "SMSTemplates",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_SMSTemplates_DeleteById",
                table: "SMSTemplates",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_SMSTemplates_SMSProfileId",
                table: "SMSTemplates",
                column: "SMSProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_SMSTemplates_UpdateById",
                table: "SMSTemplates",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_StatusService_CreateById",
                table: "StatusService",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_StatusService_DeleteById",
                table: "StatusService",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_StatusService_UpdateById",
                table: "StatusService",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_SystemModules_CreateById",
                table: "SystemModules",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_SystemModules_DeleteById",
                table: "SystemModules",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_SystemModules_DepartmentId",
                table: "SystemModules",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_SystemModules_UpdateById",
                table: "SystemModules",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_SystemModuleType_CreateById",
                table: "SystemModuleType",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_SystemModuleType_DeleteById",
                table: "SystemModuleType",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_SystemModuleType_ParentModuleTypeId",
                table: "SystemModuleType",
                column: "ParentModuleTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SystemModuleType_UpdateById",
                table: "SystemModuleType",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_SystemSetting_CreateById",
                table: "SystemSetting",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_SystemSetting_DeleteById",
                table: "SystemSetting",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_SystemSetting_UpdateById",
                table: "SystemSetting",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_Team_CreateById",
                table: "Team",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_Team_DeleteById",
                table: "Team",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_Team_DepartmentId",
                table: "Team",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Team_UpdateById",
                table: "Team",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateDocuments_CreateById",
                table: "TemplateDocuments",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateDocuments_DeleteById",
                table: "TemplateDocuments",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateDocuments_UpdateById",
                table: "TemplateDocuments",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateGenrationTypies_BackendName",
                table: "TemplateGenrationTypies",
                column: "BackendName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TemplateGenrationTypies_CreateById",
                table: "TemplateGenrationTypies",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateGenrationTypies_DeleteById",
                table: "TemplateGenrationTypies",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateGenrationTypies_UpdateById",
                table: "TemplateGenrationTypies",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionType_CreateById",
                table: "TransactionType",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionType_DeleteById",
                table: "TransactionType",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionType_UpdateById",
                table: "TransactionType",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_UiControl_BackendName",
                table: "UiControl",
                column: "BackendName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UiControl_CreateById",
                table: "UiControl",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_UiControl_DeleteById",
                table: "UiControl",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_UiControl_UpdateById",
                table: "UiControl",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_UserDeparment_CreateById",
                table: "UserDeparment",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_UserDeparment_DeleteById",
                table: "UserDeparment",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_UserDeparment_DepartmentId",
                table: "UserDeparment",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDeparment_UpdateById",
                table: "UserDeparment",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_UserDeparment_UserId",
                table: "UserDeparment",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDepartment_CreateById",
                table: "UserDepartment",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_UserDepartment_DeleteById",
                table: "UserDepartment",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_UserDepartment_DepartmentId",
                table: "UserDepartment",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDepartment_UpdateById",
                table: "UserDepartment",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_UserDepartment_UserId",
                table: "UserDepartment",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLoginLogs_CreateById",
                table: "UserLoginLogs",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_UserLoginLogs_DeleteById",
                table: "UserLoginLogs",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_UserLoginLogs_UpdateById",
                table: "UserLoginLogs",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_UserLoginLogs_UserId",
                table: "UserLoginLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPartyTypes_CreateById",
                table: "UserPartyTypes",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_UserPartyTypes_DeleteById",
                table: "UserPartyTypes",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_UserPartyTypes_PartyTypeId",
                table: "UserPartyTypes",
                column: "PartyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPartyTypes_UpdateById",
                table: "UserPartyTypes",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_UserPartyTypes_UserId",
                table: "UserPartyTypes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPartyTypeSignatures_CreateById",
                table: "UserPartyTypeSignatures",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_UserPartyTypeSignatures_DeleteById",
                table: "UserPartyTypeSignatures",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_UserPartyTypeSignatures_UpdateById",
                table: "UserPartyTypeSignatures",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_UserPartyTypeSignatures_UserPartyTypeId",
                table: "UserPartyTypeSignatures",
                column: "UserPartyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_CreateById",
                table: "UserRoles",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_DeleteById",
                table: "UserRoles",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UpdateById",
                table: "UserRoles",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId",
                table: "UserRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTeamScope_CreateById",
                table: "UserTeamScope",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_UserTeamScope_DeleteById",
                table: "UserTeamScope",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_UserTeamScope_ScopeId",
                table: "UserTeamScope",
                column: "ScopeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTeamScope_TeamId",
                table: "UserTeamScope",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTeamScope_UpdateById",
                table: "UserTeamScope",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_UserTeamScope_UserId",
                table: "UserTeamScope",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserToken_CreateById",
                table: "UserToken",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_UserToken_DeleteById",
                table: "UserToken",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_UserToken_UpdateById",
                table: "UserToken",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_UserToken_UserId",
                table: "UserToken",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WebsiteAttachment_CreateById",
                table: "WebsiteAttachment",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_WebsiteAttachment_DeleteById",
                table: "WebsiteAttachment",
                column: "DeleteById");

            migrationBuilder.CreateIndex(
                name: "IX_WebsiteAttachment_UpdateById",
                table: "WebsiteAttachment",
                column: "UpdateById");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AcademicYearScope");

            migrationBuilder.DropTable(
                name: "ActionAssignPartyType");

            migrationBuilder.DropTable(
                name: "ActionCondition");

            migrationBuilder.DropTable(
                name: "ActionFieldAttribute");

            migrationBuilder.DropTable(
                name: "ActionPartyType");

            migrationBuilder.DropTable(
                name: "ActionShowLogPartyType");

            migrationBuilder.DropTable(
                name: "ActionStatusConfigNotification");

            migrationBuilder.DropTable(
                name: "ActionTemplateDoc");

            migrationBuilder.DropTable(
                name: "Attachment");

            migrationBuilder.DropTable(
                name: "Attribute");

            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "Banner");

            migrationBuilder.DropTable(
                name: "ChangeRequestDetail");

            migrationBuilder.DropTable(
                name: "ChangeRequestStatus");

            migrationBuilder.DropTable(
                name: "ControlValidations");

            migrationBuilder.DropTable(
                name: "CssClass");

            migrationBuilder.DropTable(
                name: "DepartmentEvaluationParty");

            migrationBuilder.DropTable(
                name: "DepartmentHoliday");

            migrationBuilder.DropTable(
                name: "DepartmentOrgTrees");

            migrationBuilder.DropTable(
                name: "DepartmentRoleAttributeValue");

            migrationBuilder.DropTable(
                name: "EmailLog");

            migrationBuilder.DropTable(
                name: "EmailTemplateDocuments");

            migrationBuilder.DropTable(
                name: "ExceptionLog");

            migrationBuilder.DropTable(
                name: "FieldAttributeValue");

            migrationBuilder.DropTable(
                name: "FieldDropDownValue");

            migrationBuilder.DropTable(
                name: "FieldPartyType");

            migrationBuilder.DropTable(
                name: "FieldValueTransactionsLog");

            migrationBuilder.DropTable(
                name: "FieldViewCondition");

            migrationBuilder.DropTable(
                name: "FieldVisabilityConfig");

            migrationBuilder.DropTable(
                name: "FormScopes");

            migrationBuilder.DropTable(
                name: "ItemValues");

            migrationBuilder.DropTable(
                name: "ModuleType");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "OrgAcademicYears");

            migrationBuilder.DropTable(
                name: "PagePermissions");

            migrationBuilder.DropTable(
                name: "PlaceHolder");

            migrationBuilder.DropTable(
                name: "PlanSchedules");

            migrationBuilder.DropTable(
                name: "PlanTypeDepartment");

            migrationBuilder.DropTable(
                name: "RelatedFieldsGroup");

            migrationBuilder.DropTable(
                name: "RequestAssign");

            migrationBuilder.DropTable(
                name: "RolePermissions");

            migrationBuilder.DropTable(
                name: "SchoolLevel");

            migrationBuilder.DropTable(
                name: "ScopeAcademicYears");

            migrationBuilder.DropTable(
                name: "ScopeUserTeam");

            migrationBuilder.DropTable(
                name: "SectionDepartment");

            migrationBuilder.DropTable(
                name: "ServiceInitiatorPartyType");

            migrationBuilder.DropTable(
                name: "ServiceRequests");

            migrationBuilder.DropTable(
                name: "ServiceRequestShowPartyType");

            migrationBuilder.DropTable(
                name: "ServiceStatusPartyTypeDisplayName");

            migrationBuilder.DropTable(
                name: "ServiceStatusPreventPartyType");

            migrationBuilder.DropTable(
                name: "SideBar");

            migrationBuilder.DropTable(
                name: "SiteContent");

            migrationBuilder.DropTable(
                name: "SiteDocument");

            migrationBuilder.DropTable(
                name: "SMSLog");

            migrationBuilder.DropTable(
                name: "SMSProfile");

            migrationBuilder.DropTable(
                name: "SystemModuleType");

            migrationBuilder.DropTable(
                name: "SystemSetting");

            migrationBuilder.DropTable(
                name: "TemplateGenrationTypies");

            migrationBuilder.DropTable(
                name: "UiControl");

            migrationBuilder.DropTable(
                name: "UserDeparment");

            migrationBuilder.DropTable(
                name: "UserDepartment");

            migrationBuilder.DropTable(
                name: "UserLoginLogs");

            migrationBuilder.DropTable(
                name: "UserPartyTypeSignatures");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserTeamScope");

            migrationBuilder.DropTable(
                name: "UserToken");

            migrationBuilder.DropTable(
                name: "WebsiteAttachment");

            migrationBuilder.DropTable(
                name: "ActionField");

            migrationBuilder.DropTable(
                name: "ActionStatusConfiguration");

            migrationBuilder.DropTable(
                name: "SMSTemplates");

            migrationBuilder.DropTable(
                name: "ActionTransactionsLog");

            migrationBuilder.DropTable(
                name: "ChangeRequest");

            migrationBuilder.DropTable(
                name: "CssApplyType");

            migrationBuilder.DropTable(
                name: "EmailTemplates");

            migrationBuilder.DropTable(
                name: "TemplateDocuments");

            migrationBuilder.DropTable(
                name: "TransactionType");

            migrationBuilder.DropTable(
                name: "Forms");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "NotificationTemplates");

            migrationBuilder.DropTable(
                name: "Pages");

            migrationBuilder.DropTable(
                name: "Level");

            migrationBuilder.DropTable(
                name: "OrgTree");

            migrationBuilder.DropTable(
                name: "StatusService");

            migrationBuilder.DropTable(
                name: "Navbar");

            migrationBuilder.DropTable(
                name: "UserPartyTypes");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Scopes");

            migrationBuilder.DropTable(
                name: "Team");

            migrationBuilder.DropTable(
                name: "Field");

            migrationBuilder.DropTable(
                name: "ServiceStatus");

            migrationBuilder.DropTable(
                name: "SMSProfiles");

            migrationBuilder.DropTable(
                name: "ServiceAction");

            migrationBuilder.DropTable(
                name: "ChangeRequestType");

            migrationBuilder.DropTable(
                name: "Plans");

            migrationBuilder.DropTable(
                name: "EvaluationParties");

            migrationBuilder.DropTable(
                name: "FormStatus");

            migrationBuilder.DropTable(
                name: "CalcMethods");

            migrationBuilder.DropTable(
                name: "OrgTreeClass");

            migrationBuilder.DropTable(
                name: "SchoolTypes");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "PartyTypes");

            migrationBuilder.DropTable(
                name: "ScopeTypes");

            migrationBuilder.DropTable(
                name: "DropDownType");

            migrationBuilder.DropTable(
                name: "FieldType");

            migrationBuilder.DropTable(
                name: "FormGroup");

            migrationBuilder.DropTable(
                name: "ActionType");

            migrationBuilder.DropTable(
                name: "AcademicYears");

            migrationBuilder.DropTable(
                name: "PlanStatuses");

            migrationBuilder.DropTable(
                name: "PlanType");

            migrationBuilder.DropTable(
                name: "FieldTypeShow");

            migrationBuilder.DropTable(
                name: "FormGroupCustomList");

            migrationBuilder.DropTable(
                name: "FormGroupType");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "SystemModules");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "OrganizationTypes");

            migrationBuilder.DropTable(
                name: "MinistryUsers");
        }
    }
}
