#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartAttendance.Persistence.Migrations.App;

/// <inheritdoc />
public partial class latandlong : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<double>(
            "Location_Lng",
            "Stations",
            "float",
            nullable: true,
            oldClrType: typeof(double),
            oldType: "float");

        migrationBuilder.AlterColumn<double>(
            "Location_Lat",
            "Stations",
            "float",
            nullable: true,
            oldClrType: typeof(double),
            oldType: "float");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<double>(
            "Location_Lng",
            "Stations",
            "float",
            nullable: false,
            defaultValue: 0.0,
            oldClrType: typeof(double),
            oldType: "float",
            oldNullable: true);

        migrationBuilder.AlterColumn<double>(
            "Location_Lat",
            "Stations",
            "float",
            nullable: false,
            defaultValue: 0.0,
            oldClrType: typeof(double),
            oldType: "float",
            oldNullable: true);
    }
}