using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartAttendance.Persistence.Migrations.feat
{
    /// <inheritdoc />
    public partial class asdrhbyjt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MajorSubjects_Majors_MajorId",
                table: "MajorSubjects");

            migrationBuilder.AddColumn<Guid>(
                name: "MajorId",
                table: "Subjects",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_MajorId",
                table: "Subjects",
                column: "MajorId");

            migrationBuilder.AddForeignKey(
                name: "FK_MajorSubjects_Majors_MajorId",
                table: "MajorSubjects",
                column: "MajorId",
                principalTable: "Majors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_Majors_MajorId",
                table: "Subjects",
                column: "MajorId",
                principalTable: "Majors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MajorSubjects_Majors_MajorId",
                table: "MajorSubjects");

            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_Majors_MajorId",
                table: "Subjects");

            migrationBuilder.DropIndex(
                name: "IX_Subjects_MajorId",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "MajorId",
                table: "Subjects");

            migrationBuilder.AddForeignKey(
                name: "FK_MajorSubjects_Majors_MajorId",
                table: "MajorSubjects",
                column: "MajorId",
                principalTable: "Majors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
