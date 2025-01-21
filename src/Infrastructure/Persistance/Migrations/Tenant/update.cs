using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Shifty.Persistence.Migrations.Tenant
{
    /// <inheritdoc />
    public partial class update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey("FK_TenantInfo_TenantAdmin_UserId" , "TenantInfo");

            migrationBuilder.DropTable(name: "TenantAdmin");

            migrationBuilder.DropColumn("CreatedAt" , "Users");

            migrationBuilder.DropColumn("CreatedBy" , "Users");

            migrationBuilder.DropColumn("DeletedAt" , "Users");

            migrationBuilder.DropColumn("DeletedBy" , "Users");

            migrationBuilder.DropColumn("IsActive" , "Users");

            migrationBuilder.DropColumn("LastLoginDate" , "Users");

            migrationBuilder.DropColumn("ModifiedAt" , "Users");

            migrationBuilder.DropColumn("ModifiedBy" , "Users");

            migrationBuilder.DropColumn("ConnectionString" , "TenantInfo");

            migrationBuilder.AlterColumn<string>("SimSerialNumber" ,
                "Users" ,
                "nvarchar(max)" ,
                nullable: false ,
                defaultValue: "" ,
                oldClrType: typeof(string) ,
                oldType: "nvarchar(max)" ,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>("ProfilePicture" ,
                "Users" ,
                "nvarchar(max)" ,
                nullable: false ,
                defaultValue: "" ,
                oldClrType: typeof(string) ,
                oldType: "nvarchar(max)" ,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>("NotificationToken" ,
                "Users" ,
                "nvarchar(max)" ,
                nullable: false ,
                defaultValue: "" ,
                oldClrType: typeof(string) ,
                oldType: "nvarchar(max)" ,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>("NationalCode" ,
                "Users" ,
                "nvarchar(max)" ,
                nullable: false ,
                defaultValue: "" ,
                oldClrType: typeof(string) ,
                oldType: "nvarchar(max)" ,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>("MobileNumber" ,
                "Users" ,
                "nvarchar(max)" ,
                nullable: false ,
                defaultValue: "" ,
                oldClrType: typeof(string) ,
                oldType: "nvarchar(max)" ,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>("LastName" ,
                "Users" ,
                "nvarchar(max)" ,
                nullable: false ,
                defaultValue: "" ,
                oldClrType: typeof(string) ,
                oldType: "nvarchar(max)" ,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>("FirstName" ,
                "Users" ,
                "nvarchar(max)" ,
                nullable: false ,
                defaultValue: "" ,
                oldClrType: typeof(string) ,
                oldType: "nvarchar(max)" ,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>("FatherName" ,
                "Users" ,
                "nvarchar(max)" ,
                nullable: false ,
                defaultValue: "" ,
                oldClrType: typeof(string) ,
                oldType: "nvarchar(max)" ,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>("EmployeeId" ,
                "Users" ,
                "nvarchar(max)" ,
                nullable: false ,
                defaultValue: "" ,
                oldClrType: typeof(string) ,
                oldType: "nvarchar(max)" ,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>("Address" ,
                "Users" ,
                "nvarchar(max)" ,
                nullable: false ,
                defaultValue: "" ,
                oldClrType: typeof(string) ,
                oldType: "nvarchar(max)" ,
                oldNullable: true);

            migrationBuilder.AddColumn<string>("ApplicationAccessDomain" , "Companies" , "nvarchar(max)" , nullable: false , defaultValue: "");

            migrationBuilder.AddForeignKey("FK_TenantInfo_Users_UserId" ,
                "TenantInfo" ,
                "UserId" ,
                "Users" ,
                principalColumn: "Id" ,
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey("FK_TenantInfo_Users_UserId" , "TenantInfo");

            migrationBuilder.DropColumn("ApplicationAccessDomain" , "Companies");

            migrationBuilder.AlterColumn<string>("SimSerialNumber" ,
                "Users" ,
                "nvarchar(max)" ,
                nullable: true ,
                oldClrType: typeof(string) ,
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>("ProfilePicture" ,
                "Users" ,
                "nvarchar(max)" ,
                nullable: true ,
                oldClrType: typeof(string) ,
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>("NotificationToken" ,
                "Users" ,
                "nvarchar(max)" ,
                nullable: true ,
                oldClrType: typeof(string) ,
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>("NationalCode" ,
                "Users" ,
                "nvarchar(max)" ,
                nullable: true ,
                oldClrType: typeof(string) ,
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>("MobileNumber" ,
                "Users" ,
                "nvarchar(max)" ,
                nullable: true ,
                oldClrType: typeof(string) ,
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>("LastName" , "Users" , "nvarchar(max)" , nullable: true , oldClrType: typeof(string) , oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>("FirstName" ,
                "Users" ,
                "nvarchar(max)" ,
                nullable: true ,
                oldClrType: typeof(string) ,
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>("FatherName" ,
                "Users" ,
                "nvarchar(max)" ,
                nullable: true ,
                oldClrType: typeof(string) ,
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>("EmployeeId" ,
                "Users" ,
                "nvarchar(max)" ,
                nullable: true ,
                oldClrType: typeof(string) ,
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>("Address" , "Users" , "nvarchar(max)" , nullable: true , oldClrType: typeof(string) , oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>("CreatedAt" ,
                "Users" ,
                "datetime2" ,
                nullable: false ,
                defaultValue: new DateTime(1 , 1 , 1 , 0 , 0 , 0 , 0 , DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>("CreatedBy" ,
                "Users" ,
                "uniqueidentifier" ,
                nullable: false ,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>("DeletedAt" , "Users" , "datetime2" , nullable: true);

            migrationBuilder.AddColumn<Guid>("DeletedBy" , "Users" , "uniqueidentifier" , nullable: true);

            migrationBuilder.AddColumn<bool>("IsActive" , "Users" , "bit" , nullable: false , defaultValue: false);

            migrationBuilder.AddColumn<DateTime>("LastLoginDate" ,
                "Users" ,
                "datetime2" ,
                nullable: false ,
                defaultValue: new DateTime(1 , 1 , 1 , 0 , 0 , 0 , 0 , DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>("ModifiedAt" ,
                "Users" ,
                "datetime2" ,
                nullable: false ,
                defaultValue: new DateTime(1 , 1 , 1 , 0 , 0 , 0 , 0 , DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>("ModifiedBy" , "Users" , "uniqueidentifier" , nullable: true);

            migrationBuilder.AddColumn<string>("ConnectionString" , "TenantInfo" , "nvarchar(max)" , nullable: false , defaultValue: "");

            migrationBuilder.CreateTable("TenantAdmin" ,
                table => new
                {
                    Id = table.Column<Guid>("uniqueidentifier" , nullable: false) , AccessFailedCount = table.Column<int>("int" , nullable: false) ,
                    ConcurrencyStamp = table.Column<string>("nvarchar(max)" , nullable: true) , Email = table.Column<string>("nvarchar(max)" , nullable: true) ,
                    EmailConfirmed = table.Column<bool>("bit" , nullable: false) , LockoutEnabled = table.Column<bool>("bit" , nullable: false) ,
                    LockoutEnd = table.Column<DateTimeOffset>("datetimeoffset" , nullable: true) ,
                    NormalizedEmail = table.Column<string>("nvarchar(max)" , nullable: true) ,
                    NormalizedUserName = table.Column<string>("nvarchar(max)" , nullable: true) ,
                    PasswordHash = table.Column<string>("nvarchar(max)" , nullable: true) , PhoneNumber = table.Column<string>("nvarchar(max)" , nullable: true) ,
                    PhoneNumberConfirmed = table.Column<bool>("bit" , nullable: false) , SecurityStamp = table.Column<string>("nvarchar(max)" , nullable: true) ,
                    TwoFactorEnabled = table.Column<bool>("bit" , nullable: false) , UserName = table.Column<string>("nvarchar(max)" , nullable: true) } ,
                constraints: table =>
                             {
                                 table.PrimaryKey("PK_TenantAdmin" , x => x.Id);
                             });

            migrationBuilder.AddForeignKey("FK_TenantInfo_TenantAdmin_UserId" ,
                "TenantInfo" ,
                "UserId" ,
                "TenantAdmin" ,
                principalColumn: "Id" ,
                onDelete: ReferentialAction.Restrict);
        }
    }
}