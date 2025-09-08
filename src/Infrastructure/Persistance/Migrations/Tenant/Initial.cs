#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartAttendance.Persistence.Migrations.Tenant;

/// <inheritdoc />
public partial class Initial : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            "Discounts",
            table => new
            {
                Id = table.Column<Guid>("uniqueidentifier", nullable: false),
                Code = table.Column<string>("nvarchar(max)", nullable: true),
                StartDate = table.Column<DateTime>("datetime2", nullable: false),
                EndDate = table.Column<DateTime>("datetime2",   nullable: false),
                Duration = table.Column<int>("int", nullable: false),
                DiscountType = table.Column<byte>("tinyint", nullable: false),
                Value = table.Column<decimal>("decimal(18,2)", nullable: false),
                PackageMonth = table.Column<int>("int", nullable: true),
                IsPrivate = table.Column<bool>("bit", nullable: false),
                IsActive = table.Column<bool>("bit",  nullable: false),
                CreatedBy = table.Column<Guid>("uniqueidentifier", nullable: true),
                CreatedAt = table.Column<DateTime>("datetime2", nullable: false),
                ModifiedBy = table.Column<Guid>("uniqueidentifier", nullable: true),
                ModifiedAt = table.Column<DateTime>("datetime2", nullable: true),
                DeletedBy = table.Column<Guid>("uniqueidentifier", nullable: true),
                DeletedAt = table.Column<DateTime>("datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Discounts", x => x.Id);
            });

        migrationBuilder.CreateTable(
            "Prices",
            table => new
            {
                Id = table.Column<Guid>("uniqueidentifier", nullable: false),
                Amount = table.Column<decimal>("decimal(18,2)", nullable: false),
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
                table.PrimaryKey("PK_Prices", x => x.Id);
            });

        migrationBuilder.CreateTable(
            "TenantAdmins",
            table => new
            {
                Id = table.Column<Guid>("uniqueidentifier", nullable: false),
                FirstName = table.Column<string>("nvarchar(max)",   nullable: false),
                LastName = table.Column<string>("nvarchar(max)",    nullable: false),
                PhoneNumber = table.Column<string>("nvarchar(max)", nullable: false),
                roleType = table.Column<int>("int", nullable: false),
                IsLeader = table.Column<bool>("bit", nullable: false),
                FatherName = table.Column<string>("nvarchar(max)",   nullable: false),
                NationalCode = table.Column<string>("nvarchar(max)", nullable: false),
                RegisteredAt = table.Column<DateTime>("datetime2", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_TenantAdmins", x => x.Id);
            });

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
            "TenantInfo",
            table => new
            {
                Id = table.Column<string>("nvarchar(64)",        maxLength: 64, nullable: false),
                LandLine = table.Column<string>("nvarchar(max)", nullable: true),
                Address = table.Column<string>("nvarchar(max)",  nullable: true),
                UserId = table.Column<Guid>("uniqueidentifier", nullable: true),
                LegalName = table.Column<string>("nvarchar(max)",    nullable: true),
                NationalCode = table.Column<string>("nvarchar(max)", nullable: true),
                City = table.Column<string>("nvarchar(max)",         nullable: true),
                Province = table.Column<string>("nvarchar(max)",     nullable: true),
                Town = table.Column<string>("nvarchar(max)",         nullable: true),
                PostalCode = table.Column<string>("nvarchar(max)",   nullable: true),
                PhoneNumber = table.Column<string>("nvarchar(max)",  nullable: true),
                IsLegal = table.Column<bool>("bit", nullable: false),
                Logo = table.Column<string>("nvarchar(max)",         nullable: true),
                ActivityType = table.Column<string>("nvarchar(max)", nullable: true),
                Identifier = table.Column<string>("nvarchar(450)",   nullable: true),
                Name = table.Column<string>("nvarchar(100)",         maxLength: 100, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_TenantInfo", x => x.Id);
                table.ForeignKey(
                    "FK_TenantInfo_TenantAdmins_UserId",
                    x => x.UserId,
                    "TenantAdmins",
                    "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            "Payments",
            table => new
            {
                Id = table.Column<Guid>("uniqueidentifier", nullable: false),
                IsActive = table.Column<bool>("bit", nullable: false),
                PaymentDate = table.Column<DateTime>("datetime2", nullable: false),
                StartDate = table.Column<DateTime>("datetime2",   nullable: false),
                EndDate = table.Column<DateTime>("datetime2",     nullable: false),
                UsersCount = table.Column<int>("int",    nullable: false),
                ActiveUsers = table.Column<int>("int",   nullable: false),
                ProjectsCount = table.Column<int>("int", nullable: true),
                Cost = table.Column<decimal>("decimal(18,2)",             nullable: false),
                BasePrice = table.Column<decimal>("decimal(18,2)",        nullable: false),
                DiscountAmount = table.Column<decimal>("decimal(18,2)",   nullable: false),
                TaxAmount = table.Column<decimal>("decimal(18,2)",        nullable: false),
                GrantedStorageMb = table.Column<decimal>("decimal(18,2)", nullable: false),
                Status = table.Column<int>("int", nullable: false),
                PaymentType = table.Column<byte>("tinyint", nullable: false),
                Duration = table.Column<int>("int", nullable: true),
                PhoneNumber = table.Column<string>("nvarchar(max)", nullable: false),
                Email = table.Column<string>("nvarchar(max)",       nullable: false),
                UserId = table.Column<Guid>("uniqueidentifier", nullable: false),
                Authority = table.Column<string>("nvarchar(max)", nullable: true),
                RefId = table.Column<string>("nvarchar(max)",     nullable: true),
                LastPaymentId = table.Column<Guid>("uniqueidentifier", nullable: true),
                PriceId = table.Column<Guid>("uniqueidentifier",       nullable: true),
                DiscountId = table.Column<Guid>("uniqueidentifier",    nullable: true),
                TenantId = table.Column<string>("nvarchar(64)", nullable: false),
                CreatedBy = table.Column<Guid>("uniqueidentifier", nullable: false),
                CreatedAt = table.Column<DateTime>("datetime2", nullable: false),
                ModifiedBy = table.Column<Guid>("uniqueidentifier", nullable: true),
                ModifiedAt = table.Column<DateTime>("datetime2", nullable: false),
                DeletedBy = table.Column<Guid>("uniqueidentifier", nullable: true),
                DeletedAt = table.Column<DateTime>("datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Payments", x => x.Id);
                table.ForeignKey(
                    "FK_Payments_Discounts_DiscountId",
                    x => x.DiscountId,
                    "Discounts",
                    "Id");

                table.ForeignKey(
                    "FK_Payments_Payments_LastPaymentId",
                    x => x.LastPaymentId,
                    "Payments",
                    "Id",
                    onDelete: ReferentialAction.Restrict);

                table.ForeignKey(
                    "FK_Payments_Prices_PriceId",
                    x => x.PriceId,
                    "Prices",
                    "Id");

                table.ForeignKey(
                    "FK_Payments_TenantInfo_TenantId",
                    x => x.TenantId,
                    "TenantInfo",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "TenantDiscounts",
            table => new
            {
                Id = table.Column<Guid>("uniqueidentifier",         nullable: false),
                DiscountId = table.Column<Guid>("uniqueidentifier", nullable: false),
                TenantId = table.Column<string>("nvarchar(64)", nullable: false),
                IsUsed = table.Column<bool>("bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_TenantDiscounts", x => x.Id);
                table.ForeignKey(
                    "FK_TenantDiscounts_Discounts_DiscountId",
                    x => x.DiscountId,
                    "Discounts",
                    "Id",
                    onDelete: ReferentialAction.Cascade);

                table.ForeignKey(
                    "FK_TenantDiscounts_TenantInfo_TenantId",
                    x => x.TenantId,
                    "TenantInfo",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "TenantUsers",
            table => new
            {
                Id = table.Column<Guid>("uniqueidentifier", nullable: false),
                FirstName = table.Column<string>("nvarchar(max)",    nullable: false),
                LastName = table.Column<string>("nvarchar(max)",     nullable: false),
                PhoneNumber = table.Column<string>("nvarchar(max)",  nullable: false),
                NationalCode = table.Column<string>("nvarchar(max)", nullable: false),
                RegisteredAt = table.Column<DateTime>("datetime2", nullable: false),
                SmartAttendanceTenantInfoId = table.Column<string>("nvarchar(64)", nullable: false),
                IsActive = table.Column<bool>("bit", nullable: false),
                UserName = table.Column<string>("nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_TenantUsers", x => x.Id);
                table.ForeignKey(
                    "FK_TenantUsers_TenantInfo_SmartAttendanceTenantInfoId",
                    x => x.SmartAttendanceTenantInfoId,
                    "TenantInfo",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            "IX_Payments_DiscountId",
            "Payments",
            "DiscountId");

        migrationBuilder.CreateIndex(
            "IX_Payments_LastPaymentId",
            "Payments",
            "LastPaymentId");

        migrationBuilder.CreateIndex(
            "IX_Payments_PriceId",
            "Payments",
            "PriceId");

        migrationBuilder.CreateIndex(
            "IX_Payments_TenantId",
            "Payments",
            "TenantId");

        migrationBuilder.CreateIndex(
            "IX_TenantDiscounts_DiscountId",
            "TenantDiscounts",
            "DiscountId");

        migrationBuilder.CreateIndex(
            "IX_TenantDiscounts_TenantId",
            "TenantDiscounts",
            "TenantId");

        migrationBuilder.CreateIndex(
            "IX_TenantInfo_Identifier",
            "TenantInfo",
            "Identifier",
            unique: true,
            filter: "[Identifier] IS NOT NULL");

        migrationBuilder.CreateIndex(
            "IX_TenantInfo_UserId",
            "TenantInfo",
            "UserId");

        migrationBuilder.CreateIndex(
            "IX_TenantUsers_SmartAttendanceTenantInfoId",
            "TenantUsers",
            "SmartAttendanceTenantInfoId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Payments");

        migrationBuilder.DropTable(
            name: "TenantCalendars");

        migrationBuilder.DropTable(
            name: "TenantDiscounts");

        migrationBuilder.DropTable(
            name: "TenantRequests");

        migrationBuilder.DropTable(
            name: "TenantUsers");

        migrationBuilder.DropTable(
            name: "Prices");

        migrationBuilder.DropTable(
            name: "Discounts");

        migrationBuilder.DropTable(
            name: "TenantInfo");

        migrationBuilder.DropTable(
            name: "TenantAdmins");
    }
}