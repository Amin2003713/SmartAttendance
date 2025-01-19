using Microsoft.EntityFrameworkCore.Migrations;

namespace Shifty.Persistence.Migrations.Tenant
{
    /// <inheritdoc />
    public partial class FixFiled : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                "MobileNumber"
                , "Users");

            migrationBuilder.AlterColumn<string>(
                "ProfilePicture"
                , "Users"
                , "nvarchar(max)"
                , nullable: false
                , defaultValue: ""
                , oldClrType: typeof(string)
                , oldType: "nvarchar(max)"
                , oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                "NationalCode"
                , "Users"
                , "nvarchar(max)"
                , nullable: false
                , defaultValue: ""
                , oldClrType: typeof(string)
                , oldType: "nvarchar(max)"
                , oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                "LastName"
                , "Users"
                , "nvarchar(max)"
                , nullable: false
                , defaultValue: ""
                , oldClrType: typeof(string)
                , oldType: "nvarchar(max)"
                , oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                "FirstName"
                , "Users"
                , "nvarchar(max)"
                , nullable: false
                , defaultValue: ""
                , oldClrType: typeof(string)
                , oldType: "nvarchar(max)"
                , oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                "FatherName"
                , "Users"
                , "nvarchar(max)"
                , nullable: false
                , defaultValue: ""
                , oldClrType: typeof(string)
                , oldType: "nvarchar(max)"
                , oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                "Address"
                , "Users"
                , "nvarchar(max)"
                , nullable: false
                , defaultValue: ""
                , oldClrType: typeof(string)
                , oldType: "nvarchar(max)"
                , oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                "ShiftyTenantInfoId"
                , "Payments"
                , "nvarchar(64)"
                , nullable: false
                , defaultValue: ""
                , oldClrType: typeof(string)
                , oldType: "nvarchar(64)"
                , oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                "ProfilePicture"
                , "Users"
                , "nvarchar(max)"
                , nullable: true
                , oldClrType: typeof(string)
                , oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                "NationalCode"
                , "Users"
                , "nvarchar(max)"
                , nullable: true
                , oldClrType: typeof(string)
                , oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                "LastName"
                , "Users"
                , "nvarchar(max)"
                , nullable: true
                , oldClrType: typeof(string)
                , oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                "FirstName"
                , "Users"
                , "nvarchar(max)"
                , nullable: true
                , oldClrType: typeof(string)
                , oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                "FatherName"
                , "Users"
                , "nvarchar(max)"
                , nullable: true
                , oldClrType: typeof(string)
                , oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                "Address"
                , "Users"
                , "nvarchar(max)"
                , nullable: true
                , oldClrType: typeof(string)
                , oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                "MobileNumber"
                , "Users"
                , "nvarchar(max)"
                , nullable: true);

            migrationBuilder.AlterColumn<string>(
                "ShiftyTenantInfoId"
                , "Payments"
                , "nvarchar(64)"
                , nullable: true
                , oldClrType: typeof(string)
                , oldType: "nvarchar(64)");
        }
    }
}