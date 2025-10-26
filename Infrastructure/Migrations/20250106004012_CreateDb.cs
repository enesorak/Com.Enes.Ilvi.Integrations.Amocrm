using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Com.Enes.B2B.Crm.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Raw = table.Column<string>(type: "NVARCHAR(MAX)", nullable: false),
                    Lead = table.Column<string>(type: "NVARCHAR(MAX)", nullable: false),
                    Company = table.Column<string>(type: "NVARCHAR(MAX)", nullable: false),
                    Tag = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ComputedHash = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true),
                    SourceUpdatedAtUtc = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime", nullable: false),
                    CheckedAtUtc = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: false),
                    EventType = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: false),
                    EntityId = table.Column<long>(type: "bigint", nullable: false),
                    EntityType = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: false),
                    Raw = table.Column<string>(type: "NVARCHAR(MAX)", nullable: false),
                    EventAtUtc = table.Column<DateTime>(type: "datetime", nullable: false),
                    ComputedHash = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true),
                    SourceUpdatedAtUtc = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime", nullable: false),
                    CheckedAtUtc = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Leads",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Raw = table.Column<string>(type: "NVARCHAR(MAX)", nullable: false),
                    Company = table.Column<string>(type: "NVARCHAR(MAX)", nullable: false),
                    Tag = table.Column<string>(type: "NVARCHAR(MAX)", nullable: false),
                    ComputedHash = table.Column<string>(type: "NVARCHAR(1)", nullable: true),
                    SourceUpdatedAtUtc = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime", nullable: false),
                    CheckedAtUtc = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leads", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: false),
                    EventType = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: false),
                    EntityId = table.Column<long>(type: "bigint", nullable: false),
                    EntityType = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: false),
                    Raw = table.Column<string>(type: "NVARCHAR(MAX)", nullable: false),
                    EventAtUtc = table.Column<DateTime>(type: "datetime", nullable: false),
                    ComputedHash = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true),
                    SourceUpdatedAtUtc = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime", nullable: false),
                    CheckedAtUtc = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pipelines",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Raw = table.Column<string>(type: "NVARCHAR(MAX)", nullable: false),
                    Status = table.Column<string>(type: "NVARCHAR(MAX)", nullable: false),
                    ComputedHash = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true),
                    SourceUpdatedAtUtc = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime", nullable: false),
                    CheckedAtUtc = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pipelines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Raw = table.Column<string>(type: "NVARCHAR(MAX)", nullable: false),
                    ComputedHash = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true),
                    SourceUpdatedAtUtc = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime", nullable: false),
                    CheckedAtUtc = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaskTypes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: false),
                    Color = table.Column<string>(type: "NVARCHAR(20)", maxLength: 20, nullable: true),
                    IconId = table.Column<long>(type: "bigint", nullable: true),
                    Code = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: false),
                    ComputedHash = table.Column<string>(type: "NVARCHAR(5)", maxLength: 5, nullable: true),
                    SourceUpdatedAtUtc = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime", nullable: false),
                    CheckedAtUtc = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Raw = table.Column<string>(type: "NVARCHAR(MAX)", nullable: false),
                    ComputedHash = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true),
                    SourceUpdatedAtUtc = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime", nullable: false),
                    CheckedAtUtc = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_Id",
                table: "Contacts",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Leads_Id",
                table: "Leads",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Pipelines_Id",
                table: "Pipelines",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_Id",
                table: "Tasks",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_TaskTypes_Id",
                table: "TaskTypes",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Id",
                table: "Users",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Leads");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Pipelines");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "TaskTypes");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
