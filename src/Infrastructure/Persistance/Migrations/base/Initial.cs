#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartAttendance.Persistence.Migrations.@base;

/// <inheritdoc />
public partial class Initial : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            "TenantCalendars",
            table => new
            {
                Id = table.Column<Guid>("uniqueidentifier", nullable: false),
                IsActive = table.Column<bool>("bit", nullable: false),
                Date = table.Column<DateTime>("datetime2", nullable: false),
                IsHoliday = table.Column<bool>("bit", nullable: false),
                IsWeekend = table.Column<bool>("bit", nullable: false),
                Details = table.Column<string>("nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_TenantCalendars", x => x.Id);
            });

        migrationBuilder.CreateTable(
            "TenantRequests",
            table => new
            {
                Id = table.Column<Guid>("uniqueidentifier", nullable: false),
                RequestTime = table.Column<DateTime>("datetime2", nullable: false),
                Endpoint = table.Column<string>("nvarchar(max)", nullable: false),
                TenantId = table.Column<string>("nvarchar(max)", nullable: false),
                UserId = table.Column<Guid>("uniqueidentifier", nullable: true),
                CorrelationId = table.Column<string>("nvarchar(max)", nullable: false),
                ServiceName = table.Column<string>("nvarchar(max)",   nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_TenantRequests", x => x.Id);
            });

        migrationBuilder.CreateTable(
            "UniversityAdmins",
            table => new
            {
                Id = table.Column<Guid>("uniqueidentifier", nullable: false),
                FirstName = table.Column<string>("nvarchar(max)",   nullable: false),
                LastName = table.Column<string>("nvarchar(max)",    nullable: false),
                PhoneNumber = table.Column<string>("nvarchar(max)", nullable: false),
                IsLeader = table.Column<bool>("bit", nullable: false),
                FatherName = table.Column<string>("nvarchar(max)",   nullable: false),
                NationalCode = table.Column<string>("nvarchar(max)", nullable: false),
                RegisteredAt = table.Column<DateTime>("datetime2", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UniversityAdmins", x => x.Id);
            });

        migrationBuilder.CreateTable(
            "TenantInfo",
            table => new
            {
                Id = table.Column<string>("nvarchar(64)",                 maxLength: 64,  nullable: false),
                BranchName = table.Column<string>("nvarchar(100)",        maxLength: 100, nullable: true),
                LegalName = table.Column<string>("nvarchar(max)",         nullable: true),
                AccreditationCode = table.Column<string>("nvarchar(max)", nullable: true),
                PhoneNumber = table.Column<string>("nvarchar(max)",       nullable: true),
                LandLine = table.Column<string>("nvarchar(max)",          nullable: true),
                Email = table.Column<string>("nvarchar(max)",             nullable: true),
                Address = table.Column<string>("nvarchar(max)",           nullable: true),
                PostalCode = table.Column<string>("nvarchar(max)",        nullable: true),
                City = table.Column<string>("nvarchar(max)",              nullable: true),
                Province = table.Column<string>("nvarchar(max)",          nullable: true),
                BranchAdminId = table.Column<Guid>("uniqueidentifier", nullable: true),
                Logo = table.Column<string>("nvarchar(max)",    nullable: true),
                Website = table.Column<string>("nvarchar(max)", nullable: true),
                IsPublic = table.Column<bool>("bit", nullable: false),
                Identifier = table.Column<string>("nvarchar(450)", nullable: true),
                Name = table.Column<string>("nvarchar(150)",       maxLength: 150, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_TenantInfo", x => x.Id);
                table.ForeignKey(
                    "FK_TenantInfo_UniversityAdmins_BranchAdminId",
                    x => x.BranchAdminId,
                    "UniversityAdmins",
                    "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            "UniversityUsers",
            table => new
            {
                Id = table.Column<Guid>("uniqueidentifier", nullable: false),
                FirstName = table.Column<string>("nvarchar(max)",    nullable: false),
                LastName = table.Column<string>("nvarchar(max)",     nullable: false),
                PhoneNumber = table.Column<string>("nvarchar(max)",  nullable: false),
                NationalCode = table.Column<string>("nvarchar(max)", nullable: false),
                RegisteredAt = table.Column<DateTime>("datetime2", nullable: false),
                UniversityTenantInfoId = table.Column<string>("nvarchar(64)", nullable: false),
                IsActive = table.Column<bool>("bit", nullable: false),
                UserName = table.Column<string>("nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UniversityUsers", x => x.Id);
                table.ForeignKey(
                    "FK_UniversityUsers_TenantInfo_UniversityTenantInfoId",
                    x => x.UniversityTenantInfoId,
                    "TenantInfo",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            "IX_TenantInfo_BranchAdminId",
            "TenantInfo",
            "BranchAdminId");

        migrationBuilder.CreateIndex(
            "IX_TenantInfo_Identifier",
            "TenantInfo",
            "Identifier",
            unique: true,
            filter: "[Identifier] IS NOT NULL");

        migrationBuilder.CreateIndex(
            "IX_UniversityUsers_UniversityTenantInfoId",
            "UniversityUsers",
            "UniversityTenantInfoId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "TenantCalendars");

        migrationBuilder.DropTable(
            name: "TenantRequests");

        migrationBuilder.DropTable(
            name: "UniversityUsers");

        migrationBuilder.DropTable(
            name: "TenantInfo");

        migrationBuilder.DropTable(
            name: "UniversityAdmins");
    }
}