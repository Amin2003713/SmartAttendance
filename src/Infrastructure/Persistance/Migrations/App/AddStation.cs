#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartAttendance.Persistence.Migrations.App;

/// <inheritdoc />
public partial class AddStation : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            "Stations",
            table => new
            {
                Id = table.Column<Guid>("uniqueidentifier", nullable: false),
                Name = table.Column<string>("nvarchar(max)", nullable: false),
                Code = table.Column<string>("nvarchar(max)", nullable: false),
                AllowedDistance = table.Column<decimal>("decimal(18,4)", precision: 18, scale: 4, nullable: false),
                OnWay = table.Column<TimeSpan>("time", nullable: false),
                Location_Lat = table.Column<double>("float", nullable: false),
                Location_Lng = table.Column<double>("float", nullable: false),
                Location_Name = table.Column<string>("nvarchar(max)", nullable: true),
                StationStatus = table.Column<int>("int", nullable: false),
                StationType = table.Column<int>("int",   nullable: false),
                IsActive = table.Column<bool>("bit", nullable: false),
                CreatedBy = table.Column<Guid>("uniqueidentifier", nullable: true),
                CreatedAt = table.Column<DateTime>("datetime2", nullable: false),
                ModifiedBy = table.Column<Guid>("uniqueidentifier", nullable: true),
                ModifiedAt = table.Column<DateTime>("datetime2", nullable: true),
                DeletedBy = table.Column<Guid>("uniqueidentifier", nullable: true),
                DeletedAt = table.Column<DateTime>("datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Stations", x => x.Id);
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Stations");
    }
}