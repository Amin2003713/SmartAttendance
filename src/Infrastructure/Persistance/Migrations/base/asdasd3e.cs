#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartAttendance.Persistence.Migrations.@base;

/// <inheritdoc />
public partial class asdasd3e : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            "FK_Excuses_Attachments_AttachmentId",
            "Excuses");

        migrationBuilder.AddForeignKey(
            "FK_Excuses_Attachments_AttachmentId",
            "Excuses",
            "AttachmentId",
            "Attachments",
            principalColumn: "Id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            "FK_Excuses_Attachments_AttachmentId",
            "Excuses");

        migrationBuilder.AddForeignKey(
            "FK_Excuses_Attachments_AttachmentId",
            "Excuses",
            "AttachmentId",
            "Attachments",
            principalColumn: "Id",
            onDelete: ReferentialAction.SetNull);
    }
}