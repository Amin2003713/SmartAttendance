using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Shifty.Persistence.Migrations.Tenant
{
    /// <inheritdoc />
    public partial class AdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>("UserId" ,
                "TenantInfo" ,
                "uniqueidentifier" ,
                nullable: true ,
                oldClrType: typeof(Guid) ,
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>("Address" ,
                "TenantInfo" ,
                "nvarchar(max)" ,
                nullable: true ,
                oldClrType: typeof(string) ,
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>("UserId" ,
                "TenantInfo" ,
                "uniqueidentifier" ,
                nullable: false ,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000") ,
                oldClrType: typeof(Guid) ,
                oldType: "uniqueidentifier" ,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>("Address" ,
                "TenantInfo" ,
                "nvarchar(max)" ,
                nullable: false ,
                defaultValue: "" ,
                oldClrType: typeof(string) ,
                oldType: "nvarchar(max)" ,
                oldNullable: true);
        }
    }
}