using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Shifty.Persistence.Migrations.Application
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "AspNetRoles"
                , table => new
                {
                    Id = table.Column<Guid>("uniqueidentifier" , nullable: false) , Description = table.Column<string>("nvarchar(max)" , nullable: false)
                    , IsActive = table.Column<bool>("bit" , nullable: false) , Name = table.Column<string>("nvarchar(256)" , maxLength: 256 , nullable: true)
                    , NormalizedName = table.Column<string>("nvarchar(256)" , maxLength: 256 , nullable: true)
                    , ConcurrencyStamp = table.Column<string>("nvarchar(max)" , nullable: true) ,
                }
                , constraints: table =>
                               {
                                   table.PrimaryKey("PK_AspNetRoles" , x => x.Id);
                               });

            migrationBuilder.CreateTable(
                "AspNetUsers"
                , table => new
                {
                    Id = table.Column<Guid>("uniqueidentifier" , nullable: false) , FirstName = table.Column<string>("nvarchar(max)" , nullable: false)
                    , LastName = table.Column<string>("nvarchar(max)" , nullable: false) , FatherName = table.Column<string>("nvarchar(max)" , nullable: true)
                    , NationalCode = table.Column<string>("nvarchar(max)" , nullable: true) , Gender = table.Column<int>("int" , nullable: false)
                    , IsTeamLeader = table.Column<bool>("bit" , nullable: false) , EmployeeId = table.Column<string>("nvarchar(max)" , nullable: true)
                    , ProfilePicture = table.Column<string>("nvarchar(max)" , nullable: true)
                    , NotificationToken = table.Column<string>("nvarchar(max)" , nullable: true)
                    , Address = table.Column<string>("nvarchar(max)" , nullable: true) , HardwareId = table.Column<string>("nvarchar(max)" , nullable: true)
                    , LastLoginDate = table.Column<DateTime>("datetime2" , nullable: false) , IsActive = table.Column<bool>("bit" , nullable: false)
                    , CreatedBy = table.Column<Guid>("uniqueidentifier" , nullable: true) , CreatedAt = table.Column<DateTime>("datetime2" , nullable: false)
                    , ModifiedBy = table.Column<Guid>("uniqueidentifier" , nullable: true) , ModifiedAt = table.Column<DateTime>("datetime2" , nullable: true)
                    , DeletedBy = table.Column<Guid>("uniqueidentifier" , nullable: true) , DeletedAt = table.Column<DateTime>("datetime2" , nullable: true)
                    , UserName = table.Column<string>("nvarchar(256)" , maxLength: 256 , nullable: true)
                    , NormalizedUserName = table.Column<string>("nvarchar(256)" , maxLength: 256 , nullable: true)
                    , Email = table.Column<string>("nvarchar(256)" , maxLength: 256 , nullable: true)
                    , NormalizedEmail = table.Column<string>("nvarchar(256)" , maxLength: 256 , nullable: true)
                    , EmailConfirmed = table.Column<bool>("bit" , nullable: false) , PasswordHash = table.Column<string>("nvarchar(max)" , nullable: true)
                    , SecurityStamp = table.Column<string>("nvarchar(max)" , nullable: true)
                    , ConcurrencyStamp = table.Column<string>("nvarchar(max)" , nullable: true)
                    , PhoneNumber = table.Column<string>("nvarchar(max)" , nullable: true) , PhoneNumberConfirmed = table.Column<bool>("bit" , nullable: false)
                    , TwoFactorEnabled = table.Column<bool>("bit" , nullable: false)
                    , LockoutEnd = table.Column<DateTimeOffset>("datetimeoffset" , nullable: true) , LockoutEnabled = table.Column<bool>("bit" , nullable: false)
                    , AccessFailedCount = table.Column<int>("int" , nullable: false) ,
                }
                , constraints: table =>
                               {
                                   table.PrimaryKey("PK_AspNetUsers" , x => x.Id);
                               });

            migrationBuilder.CreateTable(
                "AspNetRoleClaims"
                , table => new
                {
                    Id           = table.Column<int>("int" , nullable: false).Annotation("SqlServer:Identity" , "1, 1")
                    , RoleId     = table.Column<Guid>("uniqueidentifier" , nullable: false) , ClaimType = table.Column<string>("nvarchar(max)" , nullable: true)
                    , ClaimValue = table.Column<string>("nvarchar(max)" ,                                                                        nullable: true) ,
                }
                , constraints: table =>
                               {
                                   table.PrimaryKey("PK_AspNetRoleClaims" , x => x.Id);
                                   table.ForeignKey(
                                       "FK_AspNetRoleClaims_AspNetRoles_RoleId"
                                       , x => x.RoleId
                                       , "AspNetRoles"
                                       , "Id"
                                       , onDelete: ReferentialAction.Cascade);
                               });

            migrationBuilder.CreateTable(
                "AspNetUserClaims"
                , table => new
                {
                    Id           = table.Column<int>("int" , nullable: false).Annotation("SqlServer:Identity" , "1, 1")
                    , UserId     = table.Column<Guid>("uniqueidentifier" , nullable: false) , ClaimType = table.Column<string>("nvarchar(max)" , nullable: true)
                    , ClaimValue = table.Column<string>("nvarchar(max)" ,                                                                        nullable: true) ,
                }
                , constraints: table =>
                               {
                                   table.PrimaryKey("PK_AspNetUserClaims" , x => x.Id);
                                   table.ForeignKey(
                                       "FK_AspNetUserClaims_AspNetUsers_UserId"
                                       , x => x.UserId
                                       , "AspNetUsers"
                                       , "Id"
                                       , onDelete: ReferentialAction.Cascade);
                               });

            migrationBuilder.CreateTable(
                "AspNetUserLogins"
                , table => new
                {
                    LoginProvider         = table.Column<string>("nvarchar(450)" , nullable: false)
                    , ProviderKey         = table.Column<string>("nvarchar(450)" , nullable: false)
                    , ProviderDisplayName = table.Column<string>("nvarchar(max)" , nullable: true)
                    , UserId              = table.Column<Guid>("uniqueidentifier" , nullable: false) ,
                }
                , constraints: table =>
                               {
                                   table.PrimaryKey("PK_AspNetUserLogins"
                                       , x => new
                                       {
                                           x.LoginProvider , x.ProviderKey ,
                                       });
                                   table.ForeignKey(
                                       "FK_AspNetUserLogins_AspNetUsers_UserId"
                                       , x => x.UserId
                                       , "AspNetUsers"
                                       , "Id"
                                       , onDelete: ReferentialAction.Cascade);
                               });

            migrationBuilder.CreateTable(
                "AspNetUserRoles"
                , table => new
                {
                    UserId = table.Column<Guid>("uniqueidentifier" , nullable: false) , RoleId = table.Column<Guid>("uniqueidentifier" , nullable: false) ,
                }
                , constraints: table =>
                               {
                                   table.PrimaryKey("PK_AspNetUserRoles"
                                       , x => new
                                       {
                                           x.UserId , x.RoleId ,
                                       });
                                   table.ForeignKey(
                                       "FK_AspNetUserRoles_AspNetRoles_RoleId"
                                       , x => x.RoleId
                                       , "AspNetRoles"
                                       , "Id"
                                       , onDelete: ReferentialAction.Cascade);
                                   table.ForeignKey(
                                       "FK_AspNetUserRoles_AspNetUsers_UserId"
                                       , x => x.UserId
                                       , "AspNetUsers"
                                       , "Id"
                                       , onDelete: ReferentialAction.Cascade);
                               });

            migrationBuilder.CreateTable(
                "AspNetUserTokens"
                , table => new
                {
                    UserId = table.Column<Guid>("uniqueidentifier" , nullable: false) , LoginProvider = table.Column<string>("nvarchar(450)" , nullable: false)
                    , Name = table.Column<string>("nvarchar(450)" , nullable: false) , Value = table.Column<string>("nvarchar(max)" , nullable: true) ,
                }
                , constraints: table =>
                               {
                                   table.PrimaryKey("PK_AspNetUserTokens"
                                       , x => new
                                       {
                                           x.UserId , x.LoginProvider , x.Name ,
                                       });
                                   table.ForeignKey(
                                       "FK_AspNetUserTokens_AspNetUsers_UserId"
                                       , x => x.UserId
                                       , "AspNetUsers"
                                       , "Id"
                                       , onDelete: ReferentialAction.Cascade);
                               });

            migrationBuilder.CreateTable(
                "RefreshTokens"
                , table => new
                {
                    Id           = table.Column<Guid>("uniqueidentifier" , nullable: false) , UserId    = table.Column<Guid>("uniqueidentifier" , nullable: false)
                    , Token      = table.Column<string>("nvarchar(max)" , nullable: false) , ExpiryTime = table.Column<DateTime>("datetime2" , nullable: false)
                    , IsActive   = table.Column<bool>("bit" , nullable: false) , CreatedBy              = table.Column<Guid>("uniqueidentifier" , nullable: true)
                    , CreatedAt  = table.Column<DateTime>("datetime2" , nullable: false) , ModifiedBy   = table.Column<Guid>("uniqueidentifier" , nullable: true)
                    , ModifiedAt = table.Column<DateTime>("datetime2" , nullable: true) , DeletedBy     = table.Column<Guid>("uniqueidentifier" , nullable: true)
                    , DeletedAt  = table.Column<DateTime>("datetime2" , nullable: true) ,
                }
                , constraints: table =>
                               {
                                   table.PrimaryKey("PK_RefreshTokens" , x => x.Id);
                                   table.ForeignKey(
                                       "FK_RefreshTokens_AspNetUsers_UserId"
                                       , x => x.UserId
                                       , "AspNetUsers"
                                       , "Id"
                                       , onDelete: ReferentialAction.Cascade);
                               });

            migrationBuilder.CreateIndex(
                "IX_AspNetRoleClaims_RoleId"
                , "AspNetRoleClaims"
                , "RoleId");

            migrationBuilder.CreateIndex(
                "RoleNameIndex"
                , "AspNetRoles"
                , "NormalizedName"
                , unique: true
                , filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                "IX_AspNetUserClaims_UserId"
                , "AspNetUserClaims"
                , "UserId");

            migrationBuilder.CreateIndex(
                "IX_AspNetUserLogins_UserId"
                , "AspNetUserLogins"
                , "UserId");

            migrationBuilder.CreateIndex(
                "IX_AspNetUserRoles_RoleId"
                , "AspNetUserRoles"
                , "RoleId");

            migrationBuilder.CreateIndex(
                "EmailIndex"
                , "AspNetUsers"
                , "NormalizedEmail");

            migrationBuilder.CreateIndex(
                "UserNameIndex"
                , "AspNetUsers"
                , "NormalizedUserName"
                , unique: true
                , filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                "IX_RefreshTokens_UserId"
                , "RefreshTokens"
                , "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}