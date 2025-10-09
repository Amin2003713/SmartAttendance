using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartAttendance.Persistence.Migrations.feat
{
    /// <inheritdoc />
    public partial class asdagfrertg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendance_AspNetUsers_StudentId",
                table: "Attendance");

            migrationBuilder.DropForeignKey(
                name: "FK_Attendance_Excuses_ExcuseId",
                table: "Attendance");

            migrationBuilder.DropForeignKey(
                name: "FK_Attendance_Plans_PlanId",
                table: "Attendance");

            migrationBuilder.DropForeignKey(
                name: "FK_Excuses_Plans_PlanId",
                table: "Excuses");

            migrationBuilder.DropTable(
                name: "CalendarUsers");

            migrationBuilder.DropTable(
                name: "DailyCalendars");

            migrationBuilder.DropIndex(
                name: "IX_Excuses_PlanId",
                table: "Excuses");

            migrationBuilder.DropIndex(
                name: "IX_Attendance_ExcuseId",
                table: "Attendance");

            migrationBuilder.DropIndex(
                name: "IX_Attendance_PlanId",
                table: "Attendance");

            migrationBuilder.DropIndex(
                name: "IX_Attendance_StudentId",
                table: "Attendance");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Plans");

            migrationBuilder.DropColumn(
                name: "PlanId",
                table: "Attendance");

            migrationBuilder.RenameColumn(
                name: "PlanId",
                table: "Excuses",
                newName: "AttendanceId");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "Attendance",
                newName: "EnrollmentId");

            migrationBuilder.AddColumn<double>(
                name: "Location_Lat",
                table: "Plans",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Location_Lng",
                table: "Plans",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Location_Name",
                table: "Plans",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AttendanceId",
                table: "PlanEnrollments",
                type: "uniqueidentifier",
                nullable: true,
                defaultValue: null);

            migrationBuilder.AddColumn<Guid>(
                name: "HeadMasterId",
                table: "Majors",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Attendance",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Majors_HeadMasterId",
                table: "Majors",
                column: "HeadMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_Attendance_EnrollmentId",
                table: "Attendance",
                column: "EnrollmentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Attendance_ExcuseId",
                table: "Attendance",
                column: "ExcuseId",
                unique: true,
                filter: "[ExcuseId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Attendance_UserId",
                table: "Attendance",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendance_AspNetUsers_UserId",
                table: "Attendance",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendance_Excuses_ExcuseId",
                table: "Attendance",
                column: "ExcuseId",
                principalTable: "Excuses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendance_PlanEnrollments_EnrollmentId",
                table: "Attendance",
                column: "EnrollmentId",
                principalTable: "PlanEnrollments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Majors_AspNetUsers_HeadMasterId",
                table: "Majors",
                column: "HeadMasterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendance_AspNetUsers_UserId",
                table: "Attendance");

            migrationBuilder.DropForeignKey(
                name: "FK_Attendance_Excuses_ExcuseId",
                table: "Attendance");

            migrationBuilder.DropForeignKey(
                name: "FK_Attendance_PlanEnrollments_EnrollmentId",
                table: "Attendance");

            migrationBuilder.DropForeignKey(
                name: "FK_Majors_AspNetUsers_HeadMasterId",
                table: "Majors");

            migrationBuilder.DropIndex(
                name: "IX_Majors_HeadMasterId",
                table: "Majors");

            migrationBuilder.DropIndex(
                name: "IX_Attendance_EnrollmentId",
                table: "Attendance");

            migrationBuilder.DropIndex(
                name: "IX_Attendance_ExcuseId",
                table: "Attendance");

            migrationBuilder.DropIndex(
                name: "IX_Attendance_UserId",
                table: "Attendance");

            migrationBuilder.DropColumn(
                name: "Location_Lat",
                table: "Plans");

            migrationBuilder.DropColumn(
                name: "Location_Lng",
                table: "Plans");

            migrationBuilder.DropColumn(
                name: "Location_Name",
                table: "Plans");

            migrationBuilder.DropColumn(
                name: "AttendanceId",
                table: "PlanEnrollments");

            migrationBuilder.DropColumn(
                name: "HeadMasterId",
                table: "Majors");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Attendance");

            migrationBuilder.RenameColumn(
                name: "AttendanceId",
                table: "Excuses",
                newName: "PlanId");

            migrationBuilder.RenameColumn(
                name: "EnrollmentId",
                table: "Attendance",
                newName: "StudentId");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Plans",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "PlanId",
                table: "Attendance",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "DailyCalendars",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsHoliday = table.Column<bool>(type: "bit", nullable: false),
                    IsMeeting = table.Column<bool>(type: "bit", nullable: false),
                    IsReminder = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyCalendars", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CalendarUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CalendarId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalendarUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CalendarUsers_DailyCalendars_CalendarId",
                        column: x => x.CalendarId,
                        principalTable: "DailyCalendars",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Excuses_PlanId",
                table: "Excuses",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Attendance_ExcuseId",
                table: "Attendance",
                column: "ExcuseId");

            migrationBuilder.CreateIndex(
                name: "IX_Attendance_PlanId",
                table: "Attendance",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Attendance_StudentId",
                table: "Attendance",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_CalendarUsers_CalendarId",
                table: "CalendarUsers",
                column: "CalendarId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendance_AspNetUsers_StudentId",
                table: "Attendance",
                column: "StudentId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Attendance_Excuses_ExcuseId",
                table: "Attendance",
                column: "ExcuseId",
                principalTable: "Excuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Attendance_Plans_PlanId",
                table: "Attendance",
                column: "PlanId",
                principalTable: "Plans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Excuses_Plans_PlanId",
                table: "Excuses",
                column: "PlanId",
                principalTable: "Plans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
