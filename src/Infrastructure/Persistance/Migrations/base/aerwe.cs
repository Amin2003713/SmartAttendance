#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartAttendance.Persistence.Migrations.@base;

/// <inheritdoc />
public partial class aerwe : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<string>(
            "PersonalNumber",
            "AspNetUsers",
            "nvarchar(max)",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<string>(
            "PersonalNumber",
            "AspNetUsers",
            "nvarchar(max)",
            nullable: false,
            defaultValue: "",
            oldClrType: typeof(string),
            oldType: "nvarchar(max)",
            oldNullable: true);
    }
}