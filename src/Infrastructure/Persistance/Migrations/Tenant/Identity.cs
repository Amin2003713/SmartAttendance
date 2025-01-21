using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Shifty.Persistence.Migrations.Tenant
{
    /// <inheritdoc />
    public partial class Identity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn("CreatedAt" , "Users");

            migrationBuilder.DropColumn("CreatedBy" , "Users");

            migrationBuilder.DropColumn("DeletedAt" , "Users");

            migrationBuilder.DropColumn("DeletedBy" , "Users");

            migrationBuilder.DropColumn("IsActive" , "Users");

            migrationBuilder.DropColumn("LastLoginDate" , "Users");

            migrationBuilder.DropColumn("ModifiedAt" , "Users");

            migrationBuilder.DropColumn("ModifiedBy" , "Users");
        }
    }
}