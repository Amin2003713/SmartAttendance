using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartAttendance.Persistence.Migrations.@base
{
    /// <inheritdoc />
    public partial class asdasd3e : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Excuses_Attachments_AttachmentId",
                table: "Excuses");

            migrationBuilder.AddForeignKey(
                name: "FK_Excuses_Attachments_AttachmentId",
                table: "Excuses",
                column: "AttachmentId",
                principalTable: "Attachments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Excuses_Attachments_AttachmentId",
                table: "Excuses");

            migrationBuilder.AddForeignKey(
                name: "FK_Excuses_Attachments_AttachmentId",
                table: "Excuses",
                column: "AttachmentId",
                principalTable: "Attachments",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
