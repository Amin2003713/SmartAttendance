using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Shifty.Persistence.Migrations.Tenant
{
    /// <inheritdoc />
    public partial class REmoveExtras : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                "CreatedBy"
                , "Users");

            migrationBuilder.DropColumn(
                "DeletedAt"
                , "Users");

            migrationBuilder.DropColumn(
                "DeletedBy"
                , "Users");

            migrationBuilder.DropColumn(
                "ModifiedBy"
                , "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                "CreatedBy"
                , "Users"
                , "uniqueidentifier"
                , nullable: false
                , defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                "DeletedAt"
                , "Users"
                , "datetime2"
                , nullable: true);

            migrationBuilder.AddColumn<Guid>(
                "DeletedBy"
                , "Users"
                , "uniqueidentifier"
                , nullable: true);

            migrationBuilder.AddColumn<Guid>(
                "ModifiedBy"
                , "Users"
                , "uniqueidentifier"
                , nullable: true);
        }
    }
}