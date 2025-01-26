using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shifty.Persistence.Migrations.Application
{
    /// <inheritdoc />
    public partial class Addmodels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BaseEntities_AspNetUsers_UserId",
                table: "BaseEntities");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FatherName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsTeamLeader",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "NationalCode",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "NotificationToken",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "ProfilePicture",
                table: "AspNetUsers",
                newName: "Profile");

            migrationBuilder.AlterColumn<string>(
                name: "Discriminator",
                table: "BaseEntities",
                type: "nvarchar(21)",
                maxLength: 21,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(13)",
                oldMaxLength: 13);

            migrationBuilder.AddColumn<Guid>(
                name: "DivisionAssignee_UserId",
                table: "BaseEntities",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DivisionId",
                table: "BaseEntities",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<TimeOnly>(
                name: "End",
                table: "BaseEntities",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "GraceEarlyLeave",
                table: "BaseEntities",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "GraceLateArrival",
                table: "BaseEntities",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsManager",
                table: "BaseEntities",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "BaseEntities",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ParentId",
                table: "BaseEntities",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RefreshToken_UserId",
                table: "BaseEntities",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ShiftId",
                table: "BaseEntities",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Shift_Name",
                table: "BaseEntities",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<TimeOnly>(
                name: "Start",
                table: "BaseEntities",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DivisionId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntities_DivisionAssignee_UserId",
                table: "BaseEntities",
                column: "DivisionAssignee_UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntities_DivisionId",
                table: "BaseEntities",
                column: "DivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntities_ParentId",
                table: "BaseEntities",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntities_RefreshToken_UserId",
                table: "BaseEntities",
                column: "RefreshToken_UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntities_ShiftId",
                table: "BaseEntities",
                column: "ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_DivisionId",
                table: "AspNetUsers",
                column: "DivisionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_BaseEntities_DivisionId",
                table: "AspNetUsers",
                column: "DivisionId",
                principalTable: "BaseEntities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BaseEntities_AspNetUsers_DivisionAssignee_UserId",
                table: "BaseEntities",
                column: "DivisionAssignee_UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BaseEntities_AspNetUsers_RefreshToken_UserId",
                table: "BaseEntities",
                column: "RefreshToken_UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BaseEntities_AspNetUsers_UserId",
                table: "BaseEntities",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BaseEntities_BaseEntities_DivisionId",
                table: "BaseEntities",
                column: "DivisionId",
                principalTable: "BaseEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BaseEntities_BaseEntities_ParentId",
                table: "BaseEntities",
                column: "ParentId",
                principalTable: "BaseEntities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BaseEntities_BaseEntities_ShiftId",
                table: "BaseEntities",
                column: "ShiftId",
                principalTable: "BaseEntities",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_BaseEntities_DivisionId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_BaseEntities_AspNetUsers_DivisionAssignee_UserId",
                table: "BaseEntities");

            migrationBuilder.DropForeignKey(
                name: "FK_BaseEntities_AspNetUsers_RefreshToken_UserId",
                table: "BaseEntities");

            migrationBuilder.DropForeignKey(
                name: "FK_BaseEntities_AspNetUsers_UserId",
                table: "BaseEntities");

            migrationBuilder.DropForeignKey(
                name: "FK_BaseEntities_BaseEntities_DivisionId",
                table: "BaseEntities");

            migrationBuilder.DropForeignKey(
                name: "FK_BaseEntities_BaseEntities_ParentId",
                table: "BaseEntities");

            migrationBuilder.DropForeignKey(
                name: "FK_BaseEntities_BaseEntities_ShiftId",
                table: "BaseEntities");

            migrationBuilder.DropIndex(
                name: "IX_BaseEntities_DivisionAssignee_UserId",
                table: "BaseEntities");

            migrationBuilder.DropIndex(
                name: "IX_BaseEntities_DivisionId",
                table: "BaseEntities");

            migrationBuilder.DropIndex(
                name: "IX_BaseEntities_ParentId",
                table: "BaseEntities");

            migrationBuilder.DropIndex(
                name: "IX_BaseEntities_RefreshToken_UserId",
                table: "BaseEntities");

            migrationBuilder.DropIndex(
                name: "IX_BaseEntities_ShiftId",
                table: "BaseEntities");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_DivisionId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DivisionAssignee_UserId",
                table: "BaseEntities");

            migrationBuilder.DropColumn(
                name: "DivisionId",
                table: "BaseEntities");

            migrationBuilder.DropColumn(
                name: "End",
                table: "BaseEntities");

            migrationBuilder.DropColumn(
                name: "GraceEarlyLeave",
                table: "BaseEntities");

            migrationBuilder.DropColumn(
                name: "GraceLateArrival",
                table: "BaseEntities");

            migrationBuilder.DropColumn(
                name: "IsManager",
                table: "BaseEntities");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "BaseEntities");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "BaseEntities");

            migrationBuilder.DropColumn(
                name: "RefreshToken_UserId",
                table: "BaseEntities");

            migrationBuilder.DropColumn(
                name: "ShiftId",
                table: "BaseEntities");

            migrationBuilder.DropColumn(
                name: "Shift_Name",
                table: "BaseEntities");

            migrationBuilder.DropColumn(
                name: "Start",
                table: "BaseEntities");

            migrationBuilder.DropColumn(
                name: "DivisionId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Profile",
                table: "AspNetUsers",
                newName: "ProfilePicture");

            migrationBuilder.AlterColumn<string>(
                name: "Discriminator",
                table: "BaseEntities",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(21)",
                oldMaxLength: 21);

            migrationBuilder.AddColumn<string>(
                name: "EmployeeId",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FatherName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Gender",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsTeamLeader",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "NationalCode",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NotificationToken",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BaseEntities_AspNetUsers_UserId",
                table: "BaseEntities",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
