using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Shifty.Persistence.Migrations.Application
{
    /// <inheritdoc />
    public partial class test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey("FK_RefreshTokens_AspNetUsers_UserId" , "RefreshTokens");

            migrationBuilder.DropPrimaryKey("PK_RefreshTokens" , "RefreshTokens");

            migrationBuilder.RenameTable("RefreshTokens" , newName: "BaseEntities");

            migrationBuilder.RenameIndex("IX_RefreshTokens_UserId" , table: "BaseEntities" , newName: "IX_BaseEntities_UserId");

            migrationBuilder.AlterColumn<Guid>("UserId" ,
                "BaseEntities" ,
                "uniqueidentifier" ,
                nullable: true ,
                oldClrType: typeof(Guid) ,
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>("Token" ,
                "BaseEntities" ,
                "nvarchar(max)" ,
                nullable: true ,
                oldClrType: typeof(string) ,
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>("ExpiryTime" ,
                "BaseEntities" ,
                "datetime2" ,
                nullable: true ,
                oldClrType: typeof(DateTime) ,
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>("Discriminator" , "BaseEntities" , "nvarchar(13)" , maxLength: 13 , nullable: false , defaultValue: "");

            migrationBuilder.AddPrimaryKey("PK_BaseEntities" , "BaseEntities" , "Id");

            migrationBuilder.AddForeignKey("FK_BaseEntities_AspNetUsers_UserId" ,
                "BaseEntities" ,
                "UserId" ,
                "AspNetUsers" ,
                principalColumn: "Id" ,
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey("FK_BaseEntities_AspNetUsers_UserId" , "BaseEntities");

            migrationBuilder.DropPrimaryKey("PK_BaseEntities" , "BaseEntities");

            migrationBuilder.DropColumn("Discriminator" , "BaseEntities");

            migrationBuilder.RenameTable("BaseEntities" , newName: "RefreshTokens");

            migrationBuilder.RenameIndex("IX_BaseEntities_UserId" , table: "RefreshTokens" , newName: "IX_RefreshTokens_UserId");

            migrationBuilder.AlterColumn<Guid>("UserId" ,
                "RefreshTokens" ,
                "uniqueidentifier" ,
                nullable: false ,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000") ,
                oldClrType: typeof(Guid) ,
                oldType: "uniqueidentifier" ,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>("Token" ,
                "RefreshTokens" ,
                "nvarchar(max)" ,
                nullable: false ,
                defaultValue: "" ,
                oldClrType: typeof(string) ,
                oldType: "nvarchar(max)" ,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>("ExpiryTime" ,
                "RefreshTokens" ,
                "datetime2" ,
                nullable: false ,
                defaultValue: new DateTime(1 , 1 , 1 , 0 , 0 , 0 , 0 , DateTimeKind.Unspecified) ,
                oldClrType: typeof(DateTime) ,
                oldType: "datetime2" ,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey("PK_RefreshTokens" , "RefreshTokens" , "Id");

            migrationBuilder.AddForeignKey("FK_RefreshTokens_AspNetUsers_UserId" ,
                "RefreshTokens" ,
                "UserId" ,
                "AspNetUsers" ,
                principalColumn: "Id" ,
                onDelete: ReferentialAction.Cascade);
        }
    }
}