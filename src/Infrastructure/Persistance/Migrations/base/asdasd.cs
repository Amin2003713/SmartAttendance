#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartAttendance.Persistence.Migrations.@base;

/// <inheritdoc />
public partial class asdasd : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            "Attachments",
            table => new
            {
                Id = table.Column<Guid>("uniqueidentifier", nullable: false),
                FileName = table.Column<string>("nvarchar(255)",    maxLength: 255, nullable: false),
                FilePath = table.Column<string>("nvarchar(500)",    maxLength: 500, nullable: false),
                ContentType = table.Column<string>("nvarchar(100)", maxLength: 100, nullable: false),
                UploadedBy = table.Column<Guid>("uniqueidentifier", nullable: false),
                IsActive = table.Column<bool>("bit", nullable: false),
                CreatedBy = table.Column<Guid>("uniqueidentifier", nullable: true),
                CreatedAt = table.Column<DateTime>("datetime2", nullable: false),
                ModifiedBy = table.Column<Guid>("uniqueidentifier", nullable: true),
                ModifiedAt = table.Column<DateTime>("datetime2", nullable: true),
                DeletedBy = table.Column<Guid>("uniqueidentifier", nullable: true),
                DeletedAt = table.Column<DateTime>("datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Attachments", x => x.Id);
                table.ForeignKey(
                    "FK_Attachments_AspNetUsers_UploadedBy",
                    x => x.UploadedBy,
                    "AspNetUsers",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "Majors",
            table => new
            {
                Id = table.Column<Guid>("uniqueidentifier", nullable: false),
                Name = table.Column<string>("nvarchar(max)", nullable: false),
                IsActive = table.Column<bool>("bit", nullable: false),
                CreatedBy = table.Column<Guid>("uniqueidentifier", nullable: true),
                CreatedAt = table.Column<DateTime>("datetime2", nullable: false),
                ModifiedBy = table.Column<Guid>("uniqueidentifier", nullable: true),
                ModifiedAt = table.Column<DateTime>("datetime2", nullable: true),
                DeletedBy = table.Column<Guid>("uniqueidentifier", nullable: true),
                DeletedAt = table.Column<DateTime>("datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Majors", x => x.Id);
            });

        migrationBuilder.CreateTable(
            "Notifications",
            table => new
            {
                Id = table.Column<Guid>("uniqueidentifier",     nullable: false),
                UserId = table.Column<Guid>("uniqueidentifier", nullable: false),
                Title = table.Column<string>("nvarchar(200)",    maxLength: 200,  nullable: false),
                Message = table.Column<string>("nvarchar(1000)", maxLength: 1000, nullable: false),
                IsRead = table.Column<bool>("bit", nullable: false),
                CreatedOn = table.Column<DateTime>("datetime2", nullable: false),
                IsActive = table.Column<bool>("bit", nullable: false),
                CreatedBy = table.Column<Guid>("uniqueidentifier", nullable: true),
                CreatedAt = table.Column<DateTime>("datetime2", nullable: false),
                ModifiedBy = table.Column<Guid>("uniqueidentifier", nullable: true),
                ModifiedAt = table.Column<DateTime>("datetime2", nullable: true),
                DeletedBy = table.Column<Guid>("uniqueidentifier", nullable: true),
                DeletedAt = table.Column<DateTime>("datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Notifications", x => x.Id);
                table.ForeignKey(
                    "FK_Notifications_AspNetUsers_UserId",
                    x => x.UserId,
                    "AspNetUsers",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "Subjects",
            table => new
            {
                Id = table.Column<Guid>("uniqueidentifier", nullable: false),
                Name = table.Column<string>("nvarchar(max)", nullable: false),
                TeacherId = table.Column<Guid>("uniqueidentifier", nullable: false),
                IsActive = table.Column<bool>("bit", nullable: false),
                CreatedBy = table.Column<Guid>("uniqueidentifier", nullable: true),
                CreatedAt = table.Column<DateTime>("datetime2", nullable: false),
                ModifiedBy = table.Column<Guid>("uniqueidentifier", nullable: true),
                ModifiedAt = table.Column<DateTime>("datetime2", nullable: true),
                DeletedBy = table.Column<Guid>("uniqueidentifier", nullable: true),
                DeletedAt = table.Column<DateTime>("datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Subjects", x => x.Id);
                table.ForeignKey(
                    "FK_Subjects_AspNetUsers_TeacherId",
                    x => x.TeacherId,
                    "AspNetUsers",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "SubjectTeachers",
            table => new
            {
                Id = table.Column<Guid>("uniqueidentifier",        nullable: false),
                TeacherId = table.Column<Guid>("uniqueidentifier", nullable: false),
                SubjectId = table.Column<Guid>("uniqueidentifier", nullable: false),
                IsActive = table.Column<bool>("bit", nullable: false),
                CreatedBy = table.Column<Guid>("uniqueidentifier", nullable: true),
                CreatedAt = table.Column<DateTime>("datetime2", nullable: false),
                ModifiedBy = table.Column<Guid>("uniqueidentifier", nullable: true),
                ModifiedAt = table.Column<DateTime>("datetime2", nullable: true),
                DeletedBy = table.Column<Guid>("uniqueidentifier", nullable: true),
                DeletedAt = table.Column<DateTime>("datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_SubjectTeachers", x => x.Id);
                table.ForeignKey(
                    "FK_SubjectTeachers_AspNetUsers_SubjectId",
                    x => x.SubjectId,
                    "AspNetUsers",
                    "Id",
                    onDelete: ReferentialAction.Cascade);

                table.ForeignKey(
                    "FK_SubjectTeachers_AspNetUsers_TeacherId",
                    x => x.TeacherId,
                    "AspNetUsers",
                    "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            "Plans",
            table => new
            {
                Id = table.Column<Guid>("uniqueidentifier", nullable: false),
                CourseName = table.Column<string>("nvarchar(200)", maxLength: 200, nullable: false),
                SubjectId = table.Column<Guid>("uniqueidentifier", nullable: false),
                Description = table.Column<string>("nvarchar(1000)", maxLength: 1000, nullable: false),
                Location = table.Column<string>("nvarchar(200)",     maxLength: 200,  nullable: false),
                StartTime = table.Column<DateTime>("datetime2", nullable: false),
                EndTime = table.Column<DateTime>("datetime2",   nullable: false),
                Capacity = table.Column<int>("int", nullable: false),
                Address = table.Column<string>("nvarchar(max)", nullable: false),
                MajorId = table.Column<Guid>("uniqueidentifier", nullable: true),
                IsActive = table.Column<bool>("bit", nullable: false),
                CreatedBy = table.Column<Guid>("uniqueidentifier", nullable: true),
                CreatedAt = table.Column<DateTime>("datetime2", nullable: false),
                ModifiedBy = table.Column<Guid>("uniqueidentifier", nullable: true),
                ModifiedAt = table.Column<DateTime>("datetime2", nullable: true),
                DeletedBy = table.Column<Guid>("uniqueidentifier", nullable: true),
                DeletedAt = table.Column<DateTime>("datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Plans", x => x.Id);
                table.ForeignKey(
                    "FK_Plans_Majors_MajorId",
                    x => x.MajorId,
                    "Majors",
                    "Id");
            });

        migrationBuilder.CreateTable(
            "MajorSubjects",
            table => new
            {
                Id = table.Column<Guid>("uniqueidentifier",        nullable: false),
                MajorId = table.Column<Guid>("uniqueidentifier",   nullable: false),
                SubjectId = table.Column<Guid>("uniqueidentifier", nullable: false),
                IsActive = table.Column<bool>("bit", nullable: false),
                CreatedBy = table.Column<Guid>("uniqueidentifier", nullable: true),
                CreatedAt = table.Column<DateTime>("datetime2", nullable: false),
                ModifiedBy = table.Column<Guid>("uniqueidentifier", nullable: true),
                ModifiedAt = table.Column<DateTime>("datetime2", nullable: true),
                DeletedBy = table.Column<Guid>("uniqueidentifier", nullable: true),
                DeletedAt = table.Column<DateTime>("datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MajorSubjects", x => x.Id);
                table.ForeignKey(
                    "FK_MajorSubjects_Majors_MajorId",
                    x => x.MajorId,
                    "Majors",
                    "Id",
                    onDelete: ReferentialAction.Cascade);

                table.ForeignKey(
                    "FK_MajorSubjects_Subjects_SubjectId",
                    x => x.SubjectId,
                    "Subjects",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "Excuses",
            table => new
            {
                Id = table.Column<Guid>("uniqueidentifier",        nullable: false),
                StudentId = table.Column<Guid>("uniqueidentifier", nullable: false),
                PlanId = table.Column<Guid>("uniqueidentifier",    nullable: false),
                Reason = table.Column<string>("nvarchar(500)", maxLength: 500, nullable: false),
                Status = table.Column<int>("int", maxLength: 50, nullable: false),
                SubmittedAt = table.Column<DateTime>("datetime2", nullable: false),
                AttachmentId = table.Column<Guid>("uniqueidentifier", nullable: true),
                IsActive = table.Column<bool>("bit", nullable: false),
                CreatedBy = table.Column<Guid>("uniqueidentifier", nullable: true),
                CreatedAt = table.Column<DateTime>("datetime2", nullable: false),
                ModifiedBy = table.Column<Guid>("uniqueidentifier", nullable: true),
                ModifiedAt = table.Column<DateTime>("datetime2", nullable: true),
                DeletedBy = table.Column<Guid>("uniqueidentifier", nullable: true),
                DeletedAt = table.Column<DateTime>("datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Excuses", x => x.Id);
                table.ForeignKey(
                    "FK_Excuses_AspNetUsers_StudentId",
                    x => x.StudentId,
                    "AspNetUsers",
                    "Id",
                    onDelete: ReferentialAction.Cascade);

                table.ForeignKey(
                    "FK_Excuses_Attachments_AttachmentId",
                    x => x.AttachmentId,
                    "Attachments",
                    "Id",
                    onDelete: ReferentialAction.NoAction);

                table.ForeignKey(
                    "FK_Excuses_Plans_PlanId",
                    x => x.PlanId,
                    "Plans",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "PlanEnrollments",
            table => new
            {
                Id = table.Column<Guid>("uniqueidentifier",        nullable: false),
                PlanId = table.Column<Guid>("uniqueidentifier",    nullable: false),
                StudentId = table.Column<Guid>("uniqueidentifier", nullable: false),
                Status = table.Column<int>("int", maxLength: 50, nullable: false),
                EnrolledAt = table.Column<DateTime>("datetime2", nullable: false),
                IsActive = table.Column<bool>("bit", nullable: false),
                CreatedBy = table.Column<Guid>("uniqueidentifier", nullable: true),
                CreatedAt = table.Column<DateTime>("datetime2", nullable: false),
                ModifiedBy = table.Column<Guid>("uniqueidentifier", nullable: true),
                ModifiedAt = table.Column<DateTime>("datetime2", nullable: true),
                DeletedBy = table.Column<Guid>("uniqueidentifier", nullable: true),
                DeletedAt = table.Column<DateTime>("datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PlanEnrollments", x => x.Id);
                table.ForeignKey(
                    "FK_PlanEnrollments_AspNetUsers_StudentId",
                    x => x.StudentId,
                    "AspNetUsers",
                    "Id",
                    onDelete: ReferentialAction.Cascade);

                table.ForeignKey(
                    "FK_PlanEnrollments_Plans_PlanId",
                    x => x.PlanId,
                    "Plans",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "SubjectPlans",
            table => new
            {
                Id = table.Column<Guid>("uniqueidentifier",        nullable: false),
                PlanId = table.Column<Guid>("uniqueidentifier",    nullable: false),
                SubjectId = table.Column<Guid>("uniqueidentifier", nullable: false),
                IsActive = table.Column<bool>("bit", nullable: false),
                CreatedBy = table.Column<Guid>("uniqueidentifier", nullable: true),
                CreatedAt = table.Column<DateTime>("datetime2", nullable: false),
                ModifiedBy = table.Column<Guid>("uniqueidentifier", nullable: true),
                ModifiedAt = table.Column<DateTime>("datetime2", nullable: true),
                DeletedBy = table.Column<Guid>("uniqueidentifier", nullable: true),
                DeletedAt = table.Column<DateTime>("datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_SubjectPlans", x => x.Id);
                table.ForeignKey(
                    "FK_SubjectPlans_Plans_PlanId",
                    x => x.PlanId,
                    "Plans",
                    "Id",
                    onDelete: ReferentialAction.Restrict);

                table.ForeignKey(
                    "FK_SubjectPlans_Subjects_SubjectId",
                    x => x.SubjectId,
                    "Subjects",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "TeacherPlans",
            table => new
            {
                Id = table.Column<Guid>("uniqueidentifier",        nullable: false),
                PlanId = table.Column<Guid>("uniqueidentifier",    nullable: false),
                TeacherId = table.Column<Guid>("uniqueidentifier", nullable: false),
                IsActive = table.Column<bool>("bit", nullable: false),
                CreatedBy = table.Column<Guid>("uniqueidentifier", nullable: true),
                CreatedAt = table.Column<DateTime>("datetime2", nullable: false),
                ModifiedBy = table.Column<Guid>("uniqueidentifier", nullable: true),
                ModifiedAt = table.Column<DateTime>("datetime2", nullable: true),
                DeletedBy = table.Column<Guid>("uniqueidentifier", nullable: true),
                DeletedAt = table.Column<DateTime>("datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_TeacherPlans", x => x.Id);
                table.ForeignKey(
                    "FK_TeacherPlans_AspNetUsers_TeacherId",
                    x => x.TeacherId,
                    "AspNetUsers",
                    "Id",
                    onDelete: ReferentialAction.Cascade);

                table.ForeignKey(
                    "FK_TeacherPlans_Plans_PlanId",
                    x => x.PlanId,
                    "Plans",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "Attendance",
            table => new
            {
                Id = table.Column<Guid>("uniqueidentifier",        nullable: false),
                PlanId = table.Column<Guid>("uniqueidentifier",    nullable: false),
                StudentId = table.Column<Guid>("uniqueidentifier", nullable: false),
                Status = table.Column<int>("int", maxLength: 50, nullable: false),
                RecordedAt = table.Column<DateTime>("datetime2", nullable: false),
                ExcuseId = table.Column<Guid>("uniqueidentifier", nullable: true),
                IsActive = table.Column<bool>("bit", nullable: false),
                CreatedBy = table.Column<Guid>("uniqueidentifier", nullable: true),
                CreatedAt = table.Column<DateTime>("datetime2", nullable: false),
                ModifiedBy = table.Column<Guid>("uniqueidentifier", nullable: true),
                ModifiedAt = table.Column<DateTime>("datetime2", nullable: true),
                DeletedBy = table.Column<Guid>("uniqueidentifier", nullable: true),
                DeletedAt = table.Column<DateTime>("datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Attendance", x => x.Id);
                table.ForeignKey(
                    "FK_Attendance_AspNetUsers_StudentId",
                    x => x.StudentId,
                    "AspNetUsers",
                    "Id",
                    onDelete: ReferentialAction.Cascade);

                table.ForeignKey(
                    "FK_Attendance_Excuses_ExcuseId",
                    x => x.ExcuseId,
                    "Excuses",
                    "Id",
                    onDelete: ReferentialAction.SetNull);

                table.ForeignKey(
                    "FK_Attendance_Plans_PlanId",
                    x => x.PlanId,
                    "Plans",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            "IX_Attachments_UploadedBy",
            "Attachments",
            "UploadedBy");

        migrationBuilder.CreateIndex(
            "IX_Attendance_ExcuseId",
            "Attendance",
            "ExcuseId");

        migrationBuilder.CreateIndex(
            "IX_Attendance_PlanId",
            "Attendance",
            "PlanId");

        migrationBuilder.CreateIndex(
            "IX_Attendance_StudentId",
            "Attendance",
            "StudentId");

        migrationBuilder.CreateIndex(
            "IX_Excuses_AttachmentId",
            "Excuses",
            "AttachmentId");

        migrationBuilder.CreateIndex(
            "IX_Excuses_PlanId",
            "Excuses",
            "PlanId");

        migrationBuilder.CreateIndex(
            "IX_Excuses_StudentId",
            "Excuses",
            "StudentId");

        migrationBuilder.CreateIndex(
            "IX_MajorSubjects_MajorId",
            "MajorSubjects",
            "MajorId");

        migrationBuilder.CreateIndex(
            "IX_MajorSubjects_SubjectId",
            "MajorSubjects",
            "SubjectId");

        migrationBuilder.CreateIndex(
            "IX_Notifications_UserId",
            "Notifications",
            "UserId");

        migrationBuilder.CreateIndex(
            "IX_PlanEnrollments_PlanId",
            "PlanEnrollments",
            "PlanId");

        migrationBuilder.CreateIndex(
            "IX_PlanEnrollments_StudentId",
            "PlanEnrollments",
            "StudentId");

        migrationBuilder.CreateIndex(
            "IX_Plans_MajorId",
            "Plans",
            "MajorId");

        migrationBuilder.CreateIndex(
            "IX_SubjectPlans_PlanId",
            "SubjectPlans",
            "PlanId");

        migrationBuilder.CreateIndex(
            "IX_SubjectPlans_SubjectId",
            "SubjectPlans",
            "SubjectId");

        migrationBuilder.CreateIndex(
            "IX_Subjects_TeacherId",
            "Subjects",
            "TeacherId");

        migrationBuilder.CreateIndex(
            "IX_SubjectTeachers_SubjectId",
            "SubjectTeachers",
            "SubjectId");

        migrationBuilder.CreateIndex(
            "IX_SubjectTeachers_TeacherId",
            "SubjectTeachers",
            "TeacherId");

        migrationBuilder.CreateIndex(
            "IX_TeacherPlans_PlanId",
            "TeacherPlans",
            "PlanId");

        migrationBuilder.CreateIndex(
            "IX_TeacherPlans_TeacherId",
            "TeacherPlans",
            "TeacherId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Attendance");

        migrationBuilder.DropTable(
            name: "MajorSubjects");

        migrationBuilder.DropTable(
            name: "Notifications");

        migrationBuilder.DropTable(
            name: "PlanEnrollments");

        migrationBuilder.DropTable(
            name: "SubjectPlans");

        migrationBuilder.DropTable(
            name: "SubjectTeachers");

        migrationBuilder.DropTable(
            name: "TeacherPlans");

        migrationBuilder.DropTable(
            name: "Excuses");

        migrationBuilder.DropTable(
            name: "Subjects");

        migrationBuilder.DropTable(
            name: "Attachments");

        migrationBuilder.DropTable(
            name: "Plans");

        migrationBuilder.DropTable(
            name: "Majors");
    }
}