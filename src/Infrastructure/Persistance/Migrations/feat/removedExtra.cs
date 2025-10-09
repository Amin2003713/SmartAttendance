using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartAttendance.Persistence.Migrations.feat
{
    /// <inheritdoc />
    public partial class removedExtra : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FatherName",
                table: "UniversityAdmins");

            migrationBuilder.DropColumn(
                name: "IsLeader",
                table: "UniversityAdmins");

            migrationBuilder.DropColumn(
                name: "NationalCode",
                table: "UniversityAdmins");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FatherName",
                table: "UniversityAdmins",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsLeader",
                table: "UniversityAdmins",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "NationalCode",
                table: "UniversityAdmins",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
