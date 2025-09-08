#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartAttendance.Persistence.Migrations.App;

/// <inheritdoc />
public partial class Initial : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            "AspNetRoles",
            table => new
            {
                Id = table.Column<Guid>("uniqueidentifier", nullable: false),
                Description = table.Column<string>("nvarchar(max)", nullable: false),
                IsActive = table.Column<bool>("bit", nullable: false),
                Name = table.Column<string>("nvarchar(256)",             maxLength: 256, nullable: true),
                NormalizedName = table.Column<string>("nvarchar(256)",   maxLength: 256, nullable: true),
                ConcurrencyStamp = table.Column<string>("nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetRoles", x => x.Id);
            });

        migrationBuilder.CreateTable(
            "DailyCalendars",
            table => new
            {
                Id = table.Column<Guid>("uniqueidentifier", nullable: false),
                Date = table.Column<DateTime>("datetime2", nullable: false),
                IsHoliday = table.Column<bool>("bit",  nullable: false),
                IsMeeting = table.Column<bool>("bit",  nullable: false),
                IsReminder = table.Column<bool>("bit", nullable: false),
                Details = table.Column<string>("nvarchar(max)", nullable: true),
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
                table.PrimaryKey("PK_DailyCalendars", x => x.Id);
            });

        migrationBuilder.CreateTable(
            "HubFiles",
            table => new
            {
                Id = table.Column<Guid>("uniqueidentifier", nullable: false),
                Name = table.Column<string>("nvarchar(max)", nullable: true),
                Path = table.Column<string>("nvarchar(max)", nullable: false),
                Type = table.Column<int>("int", nullable: false),
                ReferenceId = table.Column<Guid>("uniqueidentifier", nullable: false),
                ReferenceIdType = table.Column<byte>("tinyint", nullable: false),
                ReportDate = table.Column<DateTime>("datetime2", nullable: false),
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
                table.PrimaryKey("PK_HubFiles", x => x.Id);
            });

        migrationBuilder.CreateTable(
            "Messages",
            table => new
            {
                Id = table.Column<Guid>("uniqueidentifier", nullable: false),
                Title = table.Column<string>("nvarchar(max)",       nullable: false),
                Description = table.Column<string>("nvarchar(max)", nullable: true),
                ImageUrl = table.Column<string>("nvarchar(max)",    nullable: true),
                Visit = table.Column<int>("int", nullable: false),
                Like = table.Column<int>("int",  nullable: false),
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
                table.PrimaryKey("PK_Messages", x => x.Id);
            });

        migrationBuilder.CreateTable(
            "Settings",
            table => new
            {
                Id = table.Column<Guid>("uniqueidentifier", nullable: false),
                Flags = table.Column<int>("int", nullable: false),
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
                table.PrimaryKey("PK_Settings", x => x.Id);
            });

        migrationBuilder.CreateTable(
            "Storages",
            table => new
            {
                Id = table.Column<Guid>("uniqueidentifier", nullable: false),
                FileType = table.Column<int>("int", nullable: false),
                RowId = table.Column<Guid>("uniqueidentifier", nullable: false),
                StorageUsedByItemMb = table.Column<decimal>("decimal(18,4)", precision: 18, scale: 4, nullable: false),
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
                table.PrimaryKey("PK_Storages", x => x.Id);
            });

        migrationBuilder.CreateTable(
            "AspNetRoleClaims",
            table => new
            {
                Id = table.Column<int>("int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                RoleId = table.Column<Guid>("uniqueidentifier", nullable: false),
                ClaimType = table.Column<string>("nvarchar(max)",  nullable: true),
                ClaimValue = table.Column<string>("nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                table.ForeignKey(
                    "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                    x => x.RoleId,
                    "AspNetRoles",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "CalendarProjects",
            table => new
            {
                Id = table.Column<Guid>("uniqueidentifier",         nullable: false),
                CalendarId = table.Column<Guid>("uniqueidentifier", nullable: true),
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
                table.PrimaryKey("PK_CalendarProjects", x => x.Id);
                table.ForeignKey(
                    "FK_CalendarProjects_DailyCalendars_CalendarId",
                    x => x.CalendarId,
                    "DailyCalendars",
                    "Id");
            });

        migrationBuilder.CreateTable(
            "CalendarUsers",
            table => new
            {
                Id = table.Column<Guid>("uniqueidentifier",         nullable: false),
                UserId = table.Column<Guid>("uniqueidentifier",     nullable: false),
                CalendarId = table.Column<Guid>("uniqueidentifier", nullable: true),
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
                table.PrimaryKey("PK_CalendarUsers", x => x.Id);
                table.ForeignKey(
                    "FK_CalendarUsers_DailyCalendars_CalendarId",
                    x => x.CalendarId,
                    "DailyCalendars",
                    "Id");
            });

        migrationBuilder.CreateTable(
            "Comments",
            table => new
            {
                Id = table.Column<Guid>("uniqueidentifier", nullable: false),
                Text = table.Column<string>("nvarchar(max)", nullable: true),
                MessageId = table.Column<Guid>("uniqueidentifier",        nullable: false),
                RelatedCommentId = table.Column<Guid>("uniqueidentifier", nullable: true),
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
                table.PrimaryKey("PK_Comments", x => x.Id);
                table.ForeignKey(
                    "FK_Comments_Comments_RelatedCommentId",
                    x => x.RelatedCommentId,
                    "Comments",
                    "Id");

                table.ForeignKey(
                    "FK_Comments_Messages_MessageId",
                    x => x.MessageId,
                    "Messages",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "MessageTargetUsers",
            table => new
            {
                Id = table.Column<Guid>("uniqueidentifier",        nullable: false),
                MessageId = table.Column<Guid>("uniqueidentifier", nullable: false),
                UserId = table.Column<Guid>("uniqueidentifier",    nullable: false),
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
                table.PrimaryKey("PK_MessageTargetUsers", x => x.Id);
                table.ForeignKey(
                    "FK_MessageTargetUsers_Messages_MessageId",
                    x => x.MessageId,
                    "Messages",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "UserLikedMessages",
            table => new
            {
                Id = table.Column<Guid>("uniqueidentifier",        nullable: false),
                MessageId = table.Column<Guid>("uniqueidentifier", nullable: false),
                UserId = table.Column<Guid>("uniqueidentifier",    nullable: false),
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
                table.PrimaryKey("PK_UserLikedMessages", x => x.Id);
                table.ForeignKey(
                    "FK_UserLikedMessages_Messages_MessageId",
                    x => x.MessageId,
                    "Messages",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "UserVisitedMessages",
            table => new
            {
                Id = table.Column<Guid>("uniqueidentifier",        nullable: false),
                MessageId = table.Column<Guid>("uniqueidentifier", nullable: false),
                UserId = table.Column<Guid>("uniqueidentifier",    nullable: false),
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
                table.PrimaryKey("PK_UserVisitedMessages", x => x.Id);
                table.ForeignKey(
                    "FK_UserVisitedMessages_Messages_MessageId",
                    x => x.MessageId,
                    "Messages",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "AspNetUserClaims",
            table => new
            {
                Id = table.Column<int>("int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                UserId = table.Column<Guid>("uniqueidentifier", nullable: false),
                ClaimType = table.Column<string>("nvarchar(max)",  nullable: true),
                ClaimValue = table.Column<string>("nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
            });

        migrationBuilder.CreateTable(
            "AspNetUserLogins",
            table => new
            {
                LoginProvider = table.Column<string>("nvarchar(450)",       nullable: false),
                ProviderKey = table.Column<string>("nvarchar(450)",         nullable: false),
                ProviderDisplayName = table.Column<string>("nvarchar(max)", nullable: true),
                UserId = table.Column<Guid>("uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetUserLogins",
                    x => new
                    {
                        x.LoginProvider,
                        x.ProviderKey
                    });
            });

        migrationBuilder.CreateTable(
            "AspNetUserRoles",
            table => new
            {
                UserId = table.Column<Guid>("uniqueidentifier", nullable: false),
                RoleId = table.Column<Guid>("uniqueidentifier", nullable: false),
                Id = table.Column<Guid>("uniqueidentifier",     nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetUserRoles",
                    x => new
                    {
                        x.UserId,
                        x.RoleId
                    });

                table.ForeignKey(
                    "FK_AspNetUserRoles_AspNetRoles_RoleId",
                    x => x.RoleId,
                    "AspNetRoles",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "AspNetUsers",
            table => new
            {
                Id = table.Column<Guid>("uniqueidentifier", nullable: false),
                FirstName = table.Column<string>("nvarchar(max)",    nullable: false),
                LastName = table.Column<string>("nvarchar(max)",     nullable: false),
                FatherName = table.Column<string>("nvarchar(max)",   nullable: false),
                NationalCode = table.Column<string>("nvarchar(max)", nullable: false),
                Gender = table.Column<int>("int", nullable: false),
                IsLeader = table.Column<bool>("bit", nullable: false),
                DepartmentId = table.Column<Guid>("uniqueidentifier", nullable: true),
                roleType = table.Column<int>("int", nullable: false),
                PersonnelNumber = table.Column<string>("nvarchar(max)", nullable: true),
                Profile = table.Column<string>("nvarchar(max)",         nullable: true),
                Address = table.Column<string>("nvarchar(max)",         nullable: true),
                LastActionOnServer = table.Column<DateTime>("datetime2", nullable: true),
                ImageUrl = table.Column<string>("nvarchar(max)", nullable: true),
                BirthDate = table.Column<DateTime>("datetime2", nullable: true),
                IsActive = table.Column<bool>("bit", nullable: false),
                CreatedBy = table.Column<Guid>("uniqueidentifier", nullable: true),
                CreatedAt = table.Column<DateTime>("datetime2", nullable: false),
                ModifiedBy = table.Column<Guid>("uniqueidentifier", nullable: true),
                ModifiedAt = table.Column<DateTime>("datetime2", nullable: true),
                DeletedBy = table.Column<Guid>("uniqueidentifier", nullable: true),
                DeletedAt = table.Column<DateTime>("datetime2", nullable: true),
                UserName = table.Column<string>("nvarchar(256)",           maxLength: 256, nullable: true),
                NormalizedUserName = table.Column<string>("nvarchar(256)", maxLength: 256, nullable: true),
                Email = table.Column<string>("nvarchar(256)",              maxLength: 256, nullable: true),
                NormalizedEmail = table.Column<string>("nvarchar(256)",    maxLength: 256, nullable: true),
                EmailConfirmed = table.Column<bool>("bit", nullable: false),
                PasswordHash = table.Column<string>("nvarchar(max)",     nullable: true),
                SecurityStamp = table.Column<string>("nvarchar(max)",    nullable: true),
                ConcurrencyStamp = table.Column<string>("nvarchar(max)", nullable: true),
                PhoneNumber = table.Column<string>("nvarchar(max)",      nullable: true),
                PhoneNumberConfirmed = table.Column<bool>("bit", nullable: false),
                TwoFactorEnabled = table.Column<bool>("bit",     nullable: false),
                LockoutEnd = table.Column<DateTimeOffset>("datetimeoffset", nullable: true),
                LockoutEnabled = table.Column<bool>("bit", nullable: false),
                AccessFailedCount = table.Column<int>("int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetUsers", x => x.Id);
            });

        migrationBuilder.CreateTable(
            "AspNetUserTokens",
            table => new
            {
                UserId = table.Column<Guid>("uniqueidentifier", nullable: false),
                LoginProvider = table.Column<string>("nvarchar(450)", nullable: false),
                Name = table.Column<string>("nvarchar(450)",          nullable: false),
                Value = table.Column<string>("nvarchar(max)",         nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetUserTokens",
                    x => new
                    {
                        x.UserId,
                        x.LoginProvider,
                        x.Name
                    });

                table.ForeignKey(
                    "FK_AspNetUserTokens_AspNetUsers_UserId",
                    x => x.UserId,
                    "AspNetUsers",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "Departments",
            table => new
            {
                Id = table.Column<Guid>("uniqueidentifier", nullable: false),
                Title = table.Column<string>("nvarchar(200)", maxLength: 200, nullable: false),
                ParentDepartmentId = table.Column<Guid>("uniqueidentifier", nullable: true),
                ManagerId = table.Column<Guid>("uniqueidentifier",          nullable: true),
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
                table.PrimaryKey("PK_Departments", x => x.Id);
                table.ForeignKey(
                    "FK_Departments_AspNetUsers_ManagerId",
                    x => x.ManagerId,
                    "AspNetUsers",
                    "Id",
                    onDelete: ReferentialAction.SetNull);

                table.ForeignKey(
                    "FK_Departments_Departments_ParentDepartmentId",
                    x => x.ParentDepartmentId,
                    "Departments",
                    "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            "UserPasswords",
            table => new
            {
                Id = table.Column<Guid>("uniqueidentifier", nullable: false),
                PasswordHash = table.Column<string>("nvarchar(max)", nullable: false),
                UserId = table.Column<Guid>("uniqueidentifier", nullable: false),
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
                table.PrimaryKey("PK_UserPasswords", x => x.Id);
                table.ForeignKey(
                    "FK_UserPasswords_AspNetUsers_UserId",
                    x => x.UserId,
                    "AspNetUsers",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "UserTokens",
            table => new
            {
                Id = table.Column<Guid>("uniqueidentifier",       nullable: false),
                UserId = table.Column<Guid>("uniqueidentifier",   nullable: false),
                UniqueId = table.Column<Guid>("uniqueidentifier", nullable: false),
                AccessToken = table.Column<string>("nvarchar(max)",  nullable: false),
                RefreshToken = table.Column<string>("nvarchar(max)", nullable: false),
                ExpiryTime = table.Column<DateTime>("datetime2", nullable: false),
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
                table.PrimaryKey("PK_UserTokens", x => x.Id);
                table.ForeignKey(
                    "FK_UserTokens_AspNetUsers_UserId",
                    x => x.UserId,
                    "AspNetUsers",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            "IX_AspNetRoleClaims_RoleId",
            "AspNetRoleClaims",
            "RoleId");

        migrationBuilder.CreateIndex(
            "RoleNameIndex",
            "AspNetRoles",
            "NormalizedName",
            unique: true,
            filter: "[NormalizedName] IS NOT NULL");

        migrationBuilder.CreateIndex(
            "IX_AspNetUserClaims_UserId",
            "AspNetUserClaims",
            "UserId");

        migrationBuilder.CreateIndex(
            "IX_AspNetUserLogins_UserId",
            "AspNetUserLogins",
            "UserId");

        migrationBuilder.CreateIndex(
            "IX_AspNetUserRoles_RoleId",
            "AspNetUserRoles",
            "RoleId");

        migrationBuilder.CreateIndex(
            "EmailIndex",
            "AspNetUsers",
            "NormalizedEmail");

        migrationBuilder.CreateIndex(
            "IX_AspNetUsers_DepartmentId",
            "AspNetUsers",
            "DepartmentId");

        migrationBuilder.CreateIndex(
            "UserNameIndex",
            "AspNetUsers",
            "NormalizedUserName",
            unique: true,
            filter: "[NormalizedUserName] IS NOT NULL");

        migrationBuilder.CreateIndex(
            "IX_CalendarProjects_CalendarId",
            "CalendarProjects",
            "CalendarId");

        migrationBuilder.CreateIndex(
            "IX_CalendarUsers_CalendarId",
            "CalendarUsers",
            "CalendarId");

        migrationBuilder.CreateIndex(
            "IX_Comments_MessageId",
            "Comments",
            "MessageId");

        migrationBuilder.CreateIndex(
            "IX_Comments_RelatedCommentId",
            "Comments",
            "RelatedCommentId");

        migrationBuilder.CreateIndex(
            "IX_Departments_ManagerId",
            "Departments",
            "ManagerId");

        migrationBuilder.CreateIndex(
            "IX_Departments_ParentDepartmentId",
            "Departments",
            "ParentDepartmentId");

        migrationBuilder.CreateIndex(
            "IX_MessageTargetUsers_MessageId",
            "MessageTargetUsers",
            "MessageId");

        migrationBuilder.CreateIndex(
            "IX_UserLikedMessages_MessageId",
            "UserLikedMessages",
            "MessageId");

        migrationBuilder.CreateIndex(
            "IX_UserPasswords_UserId",
            "UserPasswords",
            "UserId");

        migrationBuilder.CreateIndex(
            "IX_UserTokens_UserId",
            "UserTokens",
            "UserId");

        migrationBuilder.CreateIndex(
            "IX_UserVisitedMessages_MessageId",
            "UserVisitedMessages",
            "MessageId");

        migrationBuilder.AddForeignKey(
            "FK_AspNetUserClaims_AspNetUsers_UserId",
            "AspNetUserClaims",
            "UserId",
            "AspNetUsers",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            "FK_AspNetUserLogins_AspNetUsers_UserId",
            "AspNetUserLogins",
            "UserId",
            "AspNetUsers",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            "FK_AspNetUserRoles_AspNetUsers_UserId",
            "AspNetUserRoles",
            "UserId",
            "AspNetUsers",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            "FK_AspNetUsers_Departments_DepartmentId",
            "AspNetUsers",
            "DepartmentId",
            "Departments",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            "FK_Departments_AspNetUsers_ManagerId",
            "Departments");

        migrationBuilder.DropTable(
            name: "AspNetRoleClaims");

        migrationBuilder.DropTable(
            name: "AspNetUserClaims");

        migrationBuilder.DropTable(
            name: "AspNetUserLogins");

        migrationBuilder.DropTable(
            name: "AspNetUserRoles");

        migrationBuilder.DropTable(
            name: "AspNetUserTokens");

        migrationBuilder.DropTable(
            name: "CalendarProjects");

        migrationBuilder.DropTable(
            name: "CalendarUsers");

        migrationBuilder.DropTable(
            name: "Comments");

        migrationBuilder.DropTable(
            name: "HubFiles");

        migrationBuilder.DropTable(
            name: "MessageTargetUsers");

        migrationBuilder.DropTable(
            name: "Settings");

        migrationBuilder.DropTable(
            name: "Storages");

        migrationBuilder.DropTable(
            name: "UserLikedMessages");

        migrationBuilder.DropTable(
            name: "UserPasswords");

        migrationBuilder.DropTable(
            name: "UserTokens");

        migrationBuilder.DropTable(
            name: "UserVisitedMessages");

        migrationBuilder.DropTable(
            name: "AspNetRoles");

        migrationBuilder.DropTable(
            name: "DailyCalendars");

        migrationBuilder.DropTable(
            name: "Messages");

        migrationBuilder.DropTable(
            name: "AspNetUsers");

        migrationBuilder.DropTable(
            name: "Departments");
    }
}