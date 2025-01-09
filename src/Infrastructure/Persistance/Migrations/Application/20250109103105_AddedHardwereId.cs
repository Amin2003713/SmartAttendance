using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shifty.Persistence.Migrations.Application
{
    /// <inheritdoc />
    public partial class AddedHardwereId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SimSerialNumber",
                table: "AspNetUsers",
                newName: "HardwareId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HardwareId",
                table: "AspNetUsers",
                newName: "SimSerialNumber");
        }
    }
}
