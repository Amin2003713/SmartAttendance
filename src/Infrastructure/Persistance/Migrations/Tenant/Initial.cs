using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Shifty.Persistence.Migrations.Tenant
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "TenantAdmin"
                , table => new
                {
                    Id = table.Column<Guid>("uniqueidentifier" , nullable: false) , UserName = table.Column<string>("nvarchar(max)" , nullable: true)
                    , NormalizedUserName = table.Column<string>("nvarchar(max)" , nullable: true) , Email = table.Column<string>("nvarchar(max)" , nullable: true)
                    , NormalizedEmail = table.Column<string>("nvarchar(max)" , nullable: true) , EmailConfirmed = table.Column<bool>("bit" , nullable: false)
                    , PasswordHash = table.Column<string>("nvarchar(max)" , nullable: true)
                    , SecurityStamp = table.Column<string>("nvarchar(max)" , nullable: true)
                    , ConcurrencyStamp = table.Column<string>("nvarchar(max)" , nullable: true)
                    , PhoneNumber = table.Column<string>("nvarchar(max)" , nullable: true) , PhoneNumberConfirmed = table.Column<bool>("bit" , nullable: false)
                    , TwoFactorEnabled = table.Column<bool>("bit" , nullable: false)
                    , LockoutEnd = table.Column<DateTimeOffset>("datetimeoffset" , nullable: true) , LockoutEnabled = table.Column<bool>("bit" , nullable: false)
                    , AccessFailedCount = table.Column<int>("int" , nullable: false) ,
                }
                , constraints: table =>
                               {
                                   table.PrimaryKey("PK_TenantAdmin" , x => x.Id);
                               });

            migrationBuilder.CreateTable(
                "Users"
                , table => new
                {
                    Id = table.Column<Guid>("uniqueidentifier" , nullable: false) , FirstName = table.Column<string>("nvarchar(max)" , nullable: true)
                    , LastName = table.Column<string>("nvarchar(max)" , nullable: true) , FatherName = table.Column<string>("nvarchar(max)" , nullable: true)
                    , NationalCode = table.Column<string>("nvarchar(max)" , nullable: true) , Gender = table.Column<int>("int" , nullable: false)
                    , IsTeamLeader = table.Column<bool>("bit" , nullable: false) , MobileNumber = table.Column<string>("nvarchar(max)" , nullable: true)
                    , EmployeeId = table.Column<string>("nvarchar(max)" , nullable: true)
                    , ProfilePicture = table.Column<string>("nvarchar(max)" , nullable: true)
                    , NotificationToken = table.Column<string>("nvarchar(max)" , nullable: true)
                    , Address = table.Column<string>("nvarchar(max)" , nullable: true) , SimSerialNumber = table.Column<string>("nvarchar(max)" , nullable: true)
                    , IsActive = table.Column<bool>("bit" , nullable: false) , CreatedBy = table.Column<Guid>("uniqueidentifier" , nullable: false)
                    , CreatedAt = table.Column<DateTime>("datetime2" , nullable: false) , ModifiedBy = table.Column<Guid>("uniqueidentifier" , nullable: true)
                    , ModifiedAt = table.Column<DateTime>("datetime2" , nullable: false) , DeletedBy = table.Column<Guid>("uniqueidentifier" , nullable: true)
                    , DeletedAt = table.Column<DateTime>("datetime2" , nullable: true) , LastLoginDate = table.Column<DateTime>("datetime2" , nullable: false)
                    , UserName = table.Column<string>("nvarchar(max)" , nullable: true)
                    , NormalizedUserName = table.Column<string>("nvarchar(max)" , nullable: true) , Email = table.Column<string>("nvarchar(max)" , nullable: true)
                    , NormalizedEmail = table.Column<string>("nvarchar(max)" , nullable: true) , EmailConfirmed = table.Column<bool>("bit" , nullable: false)
                    , PasswordHash = table.Column<string>("nvarchar(max)" , nullable: true)
                    , SecurityStamp = table.Column<string>("nvarchar(max)" , nullable: true)
                    , ConcurrencyStamp = table.Column<string>("nvarchar(max)" , nullable: true)
                    , PhoneNumber = table.Column<string>("nvarchar(max)" , nullable: true) , PhoneNumberConfirmed = table.Column<bool>("bit" , nullable: false)
                    , TwoFactorEnabled = table.Column<bool>("bit" , nullable: false)
                    , LockoutEnd = table.Column<DateTimeOffset>("datetimeoffset" , nullable: true) , LockoutEnabled = table.Column<bool>("bit" , nullable: false)
                    , AccessFailedCount = table.Column<int>("int" , nullable: false) ,
                }
                , constraints: table =>
                               {
                                   table.PrimaryKey("PK_Users" , x => x.Id);
                               });

            migrationBuilder.CreateTable(
                "TenantInfo"
                , table => new
                {
                    Id = table.Column<string>("nvarchar(64)" , maxLength: 64 , nullable: false)
                    , Identifier = table.Column<string>("nvarchar(450)" , nullable: false) , Name = table.Column<string>("nvarchar(max)" , nullable: false)
                    , ConnectionString = table.Column<string>("nvarchar(max)" , nullable: false)
                    , UserId = table.Column<Guid>("uniqueidentifier" , nullable: false) , CompanyId = table.Column<Guid>("uniqueidentifier" , nullable: false) ,
                }
                , constraints: table =>
                               {
                                   table.PrimaryKey("PK_TenantInfo" , x => x.Id);
                                   table.ForeignKey(
                                       "FK_TenantInfo_TenantAdmin_UserId"
                                       , x => x.UserId
                                       , "TenantAdmin"
                                       , "Id"
                                       , onDelete: ReferentialAction.Restrict);
                               });

            migrationBuilder.CreateTable(
                "Companies"
                , table => new
                {
                    Id              = table.Column<Guid>("uniqueidentifier" , nullable: false) , Name = table.Column<string>("nvarchar(max)" , nullable: false)
                    , Address       = table.Column<string>("nvarchar(max)" , nullable: false) , WebSite = table.Column<string>("nvarchar(max)" , nullable: false)
                    , PhoneNumber   = table.Column<string>("nvarchar(max)" , nullable: false)
                    , TenantInfosId = table.Column<string>("nvarchar(64)" , nullable: false) , IsActive = table.Column<bool>("bit" , nullable: false)
                    , CreatedBy     = table.Column<Guid>("uniqueidentifier" , nullable: false) , CreatedAt = table.Column<DateTime>("datetime2" , nullable: false)
                    , ModifiedBy    = table.Column<Guid>("uniqueidentifier" , nullable: true) , ModifiedAt = table.Column<DateTime>("datetime2" , nullable: false)
                    , DeletedBy     = table.Column<Guid>("uniqueidentifier" , nullable: true) , DeletedAt = table.Column<DateTime>("datetime2" , nullable: true) ,
                }
                , constraints: table =>
                               {
                                   table.PrimaryKey("PK_Companies" , x => x.Id);
                                   table.ForeignKey(
                                       "FK_Companies_TenantInfo_TenantInfosId"
                                       , x => x.TenantInfosId
                                       , "TenantInfo"
                                       , "Id"
                                       , onDelete: ReferentialAction.Restrict);
                               });

            migrationBuilder.CreateTable(
                "Payments"
                , table => new
                {
                    Id = table.Column<Guid>("uniqueidentifier" , nullable: false) , ShiftyTenantInfoId = table.Column<string>("nvarchar(64)" , nullable: false)
                    , IsActive = table.Column<bool>("bit" , nullable: false) , CreatedBy = table.Column<Guid>("uniqueidentifier" , nullable: false)
                    , CreatedAt = table.Column<DateTime>("datetime2" , nullable: false) , ModifiedBy = table.Column<Guid>("uniqueidentifier" , nullable: true)
                    , ModifiedAt = table.Column<DateTime>("datetime2" , nullable: false) , DeletedBy = table.Column<Guid>("uniqueidentifier" , nullable: true)
                    , DeletedAt = table.Column<DateTime>("datetime2" , nullable: true) ,
                }
                , constraints: table =>
                               {
                                   table.PrimaryKey("PK_Payments" , x => x.Id);
                                   table.ForeignKey(
                                       "FK_Payments_TenantInfo_ShiftyTenantInfoId"
                                       , x => x.ShiftyTenantInfoId
                                       , "TenantInfo"
                                       , "Id"
                                       , onDelete: ReferentialAction.Cascade);
                               });

            migrationBuilder.CreateIndex(
                "IX_Companies_TenantInfosId"
                , "Companies"
                , "TenantInfosId"
                , unique: true);

            migrationBuilder.CreateIndex(
                "IX_Payments_ShiftyTenantInfoId"
                , "Payments"
                , "ShiftyTenantInfoId");

            migrationBuilder.CreateIndex(
                "IX_TenantInfo_Identifier"
                , "TenantInfo"
                , "Identifier"
                , unique: true);

            migrationBuilder.CreateIndex(
                "IX_TenantInfo_UserId"
                , "TenantInfo"
                , "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "TenantInfo");

            migrationBuilder.DropTable(
                name: "TenantAdmin");
        }
    }
}