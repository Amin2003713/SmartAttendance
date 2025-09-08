#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartAttendance.Persistence.Migrations;

/// <inheritdoc />
public partial class test : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            "Brand",
            "Vehicles",
            "nvarchar(max)",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<string>(
            "Descriprion",
            "Vehicles",
            "nvarchar(max)",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<string>(
            "Model",
            "Vehicles",
            "nvarchar(max)",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<Guid>(
            "ResponsibleId",
            "Vehicles",
            "uniqueidentifier",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

        migrationBuilder.AddColumn<byte>(
            "Status",
            "Vehicles",
            "tinyint",
            nullable: false,
            defaultValue: (byte)0);

        migrationBuilder.AddColumn<byte>(
            "VehicleType",
            "Vehicles",
            "tinyint",
            nullable: false,
            defaultValue: (byte)0);

        migrationBuilder.AddColumn<DateTime>(
            "YearOfProduction",
            "Vehicles",
            "datetime2",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            "Brand",
            "Vehicles");

        migrationBuilder.DropColumn(
            "Descriprion",
            "Vehicles");

        migrationBuilder.DropColumn(
            "Model",
            "Vehicles");

        migrationBuilder.DropColumn(
            "ResponsibleId",
            "Vehicles");

        migrationBuilder.DropColumn(
            "Status",
            "Vehicles");

        migrationBuilder.DropColumn(
            "VehicleType",
            "Vehicles");

        migrationBuilder.DropColumn(
            "YearOfProduction",
            "Vehicles");
    }
}