using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Com.Enes.B2B.Crm.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Fix_Leads_ComputedHash : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ComputedHash",
                table: "Leads",
                type: "NVARCHAR(200)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(1)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ComputedHash",
                table: "Leads",
                type: "NVARCHAR(1)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(200)",
                oldNullable: true);
        }
    }
}
