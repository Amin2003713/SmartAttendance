using Microsoft.EntityFrameworkCore.Migrations;

namespace Shifty.Persistence.Migrations.Tenant
{
    /// <inheritdoc />
    public partial class updateUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn("EmployeeId" , "Users");

            migrationBuilder.DropColumn("IsTeamLeader" , "Users");

            migrationBuilder.DropColumn("NotificationToken" , "Users");

            migrationBuilder.DropColumn("SimSerialNumber" , "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>("EmployeeId" , "Users" , "nvarchar(max)" , nullable: false , defaultValue: "");

            migrationBuilder.AddColumn<bool>("IsTeamLeader" , "Users" , "bit" , nullable: false , defaultValue: false);

            migrationBuilder.AddColumn<string>("NotificationToken" , "Users" , "nvarchar(max)" , nullable: false , defaultValue: "");

            migrationBuilder.AddColumn<string>("SimSerialNumber" , "Users" , "nvarchar(max)" , nullable: false , defaultValue: "");
        }
    }
}