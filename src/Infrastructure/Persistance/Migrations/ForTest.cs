#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartAttendance.Persistence.Migrations;

/// <inheritdoc />
public partial class ForTest : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "CalendarProjects");

        migrationBuilder.CreateTable(
            "Vehicles",
            table => new
            {
                Id = table.Column<Guid>("uniqueidentifier", nullable: false),
                Title = table.Column<string>("nvarchar(max)",                   nullable: false),
                PlateNumber_LeftNumber = table.Column<string>("nvarchar(max)",  nullable: true),
                PlateNumber_MiddleMark = table.Column<string>("nvarchar(max)",  nullable: true),
                PlateNumber_RightNumber = table.Column<string>("nvarchar(max)", nullable: true),
                PlateNumber_RegionCode = table.Column<string>("nvarchar(max)",  nullable: true),
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
                table.PrimaryKey("PK_Vehicles", x => x.Id);
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Vehicles");

        migrationBuilder.CreateTable(
            "CalendarProjects",
            table => new
            {
                Id = table.Column<Guid>("uniqueidentifier",         nullable: false),
                CalendarId = table.Column<Guid>("uniqueidentifier", nullable: true),
                CreatedAt = table.Column<DateTime>("datetime2", nullable: false),
                CreatedBy = table.Column<Guid>("uniqueidentifier", nullable: true),
                DeletedAt = table.Column<DateTime>("datetime2", nullable: true),
                DeletedBy = table.Column<Guid>("uniqueidentifier", nullable: true),
                IsActive = table.Column<bool>("bit", nullable: false),
                ModifiedAt = table.Column<DateTime>("datetime2", nullable: true),
                ModifiedBy = table.Column<Guid>("uniqueidentifier", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_CalendarProjects", x => x.Id);
                table.ForeignKey(
                    "FK_CalendarProjects_DailyCalendars_CalendarId",
                    x => x.CalendarId,
                    "DailyCalendars",
                    "Id");
            });

        migrationBuilder.CreateIndex(
            "IX_CalendarProjects_CalendarId",
            "CalendarProjects",
            "CalendarId");
    }
}