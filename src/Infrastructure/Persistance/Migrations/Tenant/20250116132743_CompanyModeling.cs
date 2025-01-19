using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Shifty.Persistence.Migrations.Tenant
{
    /// <inheritdoc />
    public partial class CompanyModeling : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                "AccessFailedCount"
                , "Users");

            migrationBuilder.DropColumn(
                "Address"
                , "Users");

            migrationBuilder.DropColumn(
                "ConcurrencyStamp"
                , "Users");

            migrationBuilder.DropColumn(
                "CreatedAt"
                , "Users");

            migrationBuilder.DropColumn(
                "Email"
                , "Users");

            migrationBuilder.DropColumn(
                "EmailConfirmed"
                , "Users");

            migrationBuilder.DropColumn(
                "FatherName"
                , "Users");

            migrationBuilder.DropColumn(
                "Gender"
                , "Users");

            migrationBuilder.DropColumn(
                "IsActive"
                , "Users");

            migrationBuilder.DropColumn(
                "LastLoginDate"
                , "Users");

            migrationBuilder.DropColumn(
                "LockoutEnabled"
                , "Users");

            migrationBuilder.DropColumn(
                "LockoutEnd"
                , "Users");

            migrationBuilder.DropColumn(
                "NationalCode"
                , "Users");

            migrationBuilder.DropColumn(
                "NormalizedEmail"
                , "Users");

            migrationBuilder.DropColumn(
                "NormalizedUserName"
                , "Users");

            migrationBuilder.DropColumn(
                "PasswordHash"
                , "Users");

            migrationBuilder.DropColumn(
                "PhoneNumberConfirmed"
                , "Users");

            migrationBuilder.DropColumn(
                "ProfilePicture"
                , "Users");

            migrationBuilder.DropColumn(
                "SecurityStamp"
                , "Users");

            migrationBuilder.DropColumn(
                "TwoFactorEnabled"
                , "Users");

            migrationBuilder.DropColumn(
                "UserName"
                , "Users");

            migrationBuilder.DropColumn(
                "EconomicCode"
                , "TenantInfo");

            migrationBuilder.DropColumn(
                "Email"
                , "TenantInfo");

            migrationBuilder.DropColumn(
                "NationalId"
                , "TenantInfo");

            migrationBuilder.DropColumn(
                "PhoneNumber"
                , "TenantInfo");

            migrationBuilder.DropColumn(
                "PostalCode"
                , "TenantInfo");

            migrationBuilder.DropColumn(
                "RegistrationNumber"
                , "TenantInfo");

            migrationBuilder.RenameColumn(
                "ModifiedAt"
                , "Users"
                , "RegisteredAt");

            migrationBuilder.AlterColumn<string>(
                "PhoneNumber"
                , "Users"
                , "nvarchar(max)"
                , nullable: false
                , defaultValue: ""
                , oldClrType: typeof(string)
                , oldType: "nvarchar(max)"
                , oldNullable: true);

            migrationBuilder.AddColumn<string>(
                "LandLine"
                , "TenantInfo"
                , "nvarchar(max)"
                , nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                "LandLine"
                , "TenantInfo");

            migrationBuilder.RenameColumn(
                "RegisteredAt"
                , "Users"
                , "ModifiedAt");

            migrationBuilder.AlterColumn<string>(
                "PhoneNumber"
                , "Users"
                , "nvarchar(max)"
                , nullable: true
                , oldClrType: typeof(string)
                , oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                "AccessFailedCount"
                , "Users"
                , "int"
                , nullable: false
                , defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                "Address"
                , "Users"
                , "nvarchar(max)"
                , nullable: false
                , defaultValue: "");

            migrationBuilder.AddColumn<string>(
                "ConcurrencyStamp"
                , "Users"
                , "nvarchar(max)"
                , nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                "CreatedAt"
                , "Users"
                , "datetime2"
                , nullable: false
                , defaultValue: new DateTime(1 , 1 , 1 , 0 , 0 , 0 , 0 , DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                "Email"
                , "Users"
                , "nvarchar(max)"
                , nullable: true);

            migrationBuilder.AddColumn<bool>(
                "EmailConfirmed"
                , "Users"
                , "bit"
                , nullable: false
                , defaultValue: false);

            migrationBuilder.AddColumn<string>(
                "FatherName"
                , "Users"
                , "nvarchar(max)"
                , nullable: false
                , defaultValue: "");

            migrationBuilder.AddColumn<int>(
                "Gender"
                , "Users"
                , "int"
                , nullable: false
                , defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                "IsActive"
                , "Users"
                , "bit"
                , nullable: false
                , defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                "LastLoginDate"
                , "Users"
                , "datetime2"
                , nullable: false
                , defaultValue: new DateTime(1 , 1 , 1 , 0 , 0 , 0 , 0 , DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                "LockoutEnabled"
                , "Users"
                , "bit"
                , nullable: false
                , defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                "LockoutEnd"
                , "Users"
                , "datetimeoffset"
                , nullable: true);

            migrationBuilder.AddColumn<string>(
                "NationalCode"
                , "Users"
                , "nvarchar(max)"
                , nullable: false
                , defaultValue: "");

            migrationBuilder.AddColumn<string>(
                "NormalizedEmail"
                , "Users"
                , "nvarchar(max)"
                , nullable: true);

            migrationBuilder.AddColumn<string>(
                "NormalizedUserName"
                , "Users"
                , "nvarchar(max)"
                , nullable: true);

            migrationBuilder.AddColumn<string>(
                "PasswordHash"
                , "Users"
                , "nvarchar(max)"
                , nullable: true);

            migrationBuilder.AddColumn<bool>(
                "PhoneNumberConfirmed"
                , "Users"
                , "bit"
                , nullable: false
                , defaultValue: false);

            migrationBuilder.AddColumn<string>(
                "ProfilePicture"
                , "Users"
                , "nvarchar(max)"
                , nullable: false
                , defaultValue: "");

            migrationBuilder.AddColumn<string>(
                "SecurityStamp"
                , "Users"
                , "nvarchar(max)"
                , nullable: true);

            migrationBuilder.AddColumn<bool>(
                "TwoFactorEnabled"
                , "Users"
                , "bit"
                , nullable: false
                , defaultValue: false);

            migrationBuilder.AddColumn<string>(
                "UserName"
                , "Users"
                , "nvarchar(100)"
                , maxLength: 100
                , nullable: false
                , defaultValue: "");

            migrationBuilder.AddColumn<string>(
                "EconomicCode"
                , "TenantInfo"
                , "nvarchar(max)"
                , nullable: false
                , defaultValue: "");

            migrationBuilder.AddColumn<string>(
                "Email"
                , "TenantInfo"
                , "nvarchar(max)"
                , nullable: false
                , defaultValue: "");

            migrationBuilder.AddColumn<string>(
                "NationalId"
                , "TenantInfo"
                , "nvarchar(max)"
                , nullable: false
                , defaultValue: "");

            migrationBuilder.AddColumn<string>(
                "PhoneNumber"
                , "TenantInfo"
                , "nvarchar(max)"
                , nullable: false
                , defaultValue: "");

            migrationBuilder.AddColumn<string>(
                "PostalCode"
                , "TenantInfo"
                , "nvarchar(max)"
                , nullable: false
                , defaultValue: "");

            migrationBuilder.AddColumn<string>(
                "RegistrationNumber"
                , "TenantInfo"
                , "nvarchar(max)"
                , nullable: false
                , defaultValue: "");
        }
    }
}