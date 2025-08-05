using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shifty.Persistence.Migrations.tenant
{
    /// <inheritdoc />
    public partial class sadasd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActiveService_Payments_PaymentId",
                table: "ActiveService");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ActiveService",
                table: "ActiveService");

            migrationBuilder.RenameTable(
                name: "ActiveService",
                newName: "ActiveServices");

            migrationBuilder.RenameIndex(
                name: "IX_ActiveService_PaymentId",
                table: "ActiveServices",
                newName: "IX_ActiveServices_PaymentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ActiveServices",
                table: "ActiveServices",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ActiveServices_Payments_PaymentId",
                table: "ActiveServices",
                column: "PaymentId",
                principalTable: "Payments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActiveServices_Payments_PaymentId",
                table: "ActiveServices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ActiveServices",
                table: "ActiveServices");

            migrationBuilder.RenameTable(
                name: "ActiveServices",
                newName: "ActiveService");

            migrationBuilder.RenameIndex(
                name: "IX_ActiveServices_PaymentId",
                table: "ActiveService",
                newName: "IX_ActiveService_PaymentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ActiveService",
                table: "ActiveService",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ActiveService_Payments_PaymentId",
                table: "ActiveService",
                column: "PaymentId",
                principalTable: "Payments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
