using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Shifty.Persistence.Migrations.Tenant
{
    /// <inheritdoc />
    public partial class REmoveCompany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Companies");

            migrationBuilder.DropIndex("IX_TenantInfo_Identifier" , "TenantInfo");

            migrationBuilder.DropColumn("CompanyId" , "TenantInfo");

            migrationBuilder.AlterColumn<string>("UserName" ,
                "Users" ,
                "nvarchar(100)" ,
                maxLength: 100 ,
                nullable: false ,
                defaultValue: "" ,
                oldClrType: typeof(string) ,
                oldType: "nvarchar(max)" ,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>("ProfilePicture" ,
                "Users" ,
                "nvarchar(max)" ,
                nullable: true ,
                oldClrType: typeof(string) ,
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>("NationalCode" ,
                "Users" ,
                "nvarchar(max)" ,
                nullable: true ,
                oldClrType: typeof(string) ,
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>("MobileNumber" ,
                "Users" ,
                "nvarchar(max)" ,
                nullable: true ,
                oldClrType: typeof(string) ,
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>("LastName" , "Users" , "nvarchar(max)" , nullable: true , oldClrType: typeof(string) , oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>("FirstName" ,
                "Users" ,
                "nvarchar(max)" ,
                nullable: true ,
                oldClrType: typeof(string) ,
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>("FatherName" ,
                "Users" ,
                "nvarchar(max)" ,
                nullable: true ,
                oldClrType: typeof(string) ,
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>("Address" , "Users" , "nvarchar(max)" , nullable: true , oldClrType: typeof(string) , oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>("Name" ,
                "TenantInfo" ,
                "nvarchar(100)" ,
                maxLength: 100 ,
                nullable: false ,
                oldClrType: typeof(string) ,
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>("Identifier" ,
                "TenantInfo" ,
                "nvarchar(450)" ,
                nullable: true ,
                oldClrType: typeof(string) ,
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>("Address" , "TenantInfo" , "nvarchar(max)" , nullable: false , defaultValue: "");

            migrationBuilder.AddColumn<string>("EconomicCode" , "TenantInfo" , "nvarchar(max)" , nullable: false , defaultValue: "");

            migrationBuilder.AddColumn<string>("Email" , "TenantInfo" , "nvarchar(max)" , nullable: false , defaultValue: "");

            migrationBuilder.AddColumn<string>("NationalId" , "TenantInfo" , "nvarchar(max)" , nullable: false , defaultValue: "");

            migrationBuilder.AddColumn<string>("PhoneNumber" , "TenantInfo" , "nvarchar(max)" , nullable: false , defaultValue: "");

            migrationBuilder.AddColumn<string>("PostalCode" , "TenantInfo" , "nvarchar(max)" , nullable: false , defaultValue: "");

            migrationBuilder.AddColumn<string>("RegistrationNumber" , "TenantInfo" , "nvarchar(max)" , nullable: false , defaultValue: "");

            migrationBuilder.AlterColumn<string>("ShiftyTenantInfoId" ,
                "Payments" ,
                "nvarchar(64)" ,
                nullable: true ,
                oldClrType: typeof(string) ,
                oldType: "nvarchar(64)");

            migrationBuilder.CreateIndex("IX_TenantInfo_Identifier" , "TenantInfo" , "Identifier" , unique: true , filter: "[Identifier] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex("IX_TenantInfo_Identifier" , "TenantInfo");

            migrationBuilder.DropColumn("Address" , "TenantInfo");

            migrationBuilder.DropColumn("EconomicCode" , "TenantInfo");

            migrationBuilder.DropColumn("Email" , "TenantInfo");

            migrationBuilder.DropColumn("NationalId" , "TenantInfo");

            migrationBuilder.DropColumn("PhoneNumber" , "TenantInfo");

            migrationBuilder.DropColumn("PostalCode" , "TenantInfo");

            migrationBuilder.DropColumn("RegistrationNumber" , "TenantInfo");

            migrationBuilder.AlterColumn<string>("UserName" ,
                "Users" ,
                "nvarchar(max)" ,
                nullable: true ,
                oldClrType: typeof(string) ,
                oldType: "nvarchar(100)" ,
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>("ProfilePicture" ,
                "Users" ,
                "nvarchar(max)" ,
                nullable: false ,
                defaultValue: "" ,
                oldClrType: typeof(string) ,
                oldType: "nvarchar(max)" ,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>("NationalCode" ,
                "Users" ,
                "nvarchar(max)" ,
                nullable: false ,
                defaultValue: "" ,
                oldClrType: typeof(string) ,
                oldType: "nvarchar(max)" ,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>("MobileNumber" ,
                "Users" ,
                "nvarchar(max)" ,
                nullable: false ,
                defaultValue: "" ,
                oldClrType: typeof(string) ,
                oldType: "nvarchar(max)" ,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>("LastName" ,
                "Users" ,
                "nvarchar(max)" ,
                nullable: false ,
                defaultValue: "" ,
                oldClrType: typeof(string) ,
                oldType: "nvarchar(max)" ,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>("FirstName" ,
                "Users" ,
                "nvarchar(max)" ,
                nullable: false ,
                defaultValue: "" ,
                oldClrType: typeof(string) ,
                oldType: "nvarchar(max)" ,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>("FatherName" ,
                "Users" ,
                "nvarchar(max)" ,
                nullable: false ,
                defaultValue: "" ,
                oldClrType: typeof(string) ,
                oldType: "nvarchar(max)" ,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>("Address" ,
                "Users" ,
                "nvarchar(max)" ,
                nullable: false ,
                defaultValue: "" ,
                oldClrType: typeof(string) ,
                oldType: "nvarchar(max)" ,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>("Name" ,
                "TenantInfo" ,
                "nvarchar(max)" ,
                nullable: false ,
                oldClrType: typeof(string) ,
                oldType: "nvarchar(100)" ,
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>("Identifier" ,
                "TenantInfo" ,
                "nvarchar(450)" ,
                nullable: false ,
                defaultValue: "" ,
                oldClrType: typeof(string) ,
                oldType: "nvarchar(450)" ,
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>("CompanyId" ,
                "TenantInfo" ,
                "uniqueidentifier" ,
                nullable: false ,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>("ShiftyTenantInfoId" ,
                "Payments" ,
                "nvarchar(64)" ,
                nullable: false ,
                defaultValue: "" ,
                oldClrType: typeof(string) ,
                oldType: "nvarchar(64)" ,
                oldNullable: true);

            migrationBuilder.CreateTable("Companies" ,
                table => new
                {
                    Id = table.Column<Guid>("uniqueidentifier" , nullable: false) , TenantInfosId = table.Column<string>("nvarchar(64)" , nullable: false) ,
                    Address = table.Column<string>("nvarchar(max)" , nullable: false) ,
                    ApplicationAccessDomain = table.Column<string>("nvarchar(max)" , nullable: false) ,
                    CreatedAt = table.Column<DateTime>("datetime2" , nullable: false) , CreatedBy = table.Column<Guid>("uniqueidentifier" , nullable: false) ,
                    DeletedAt = table.Column<DateTime>("datetime2" , nullable: true) , DeletedBy = table.Column<Guid>("uniqueidentifier" , nullable: true) ,
                    IsActive = table.Column<bool>("bit" , nullable: false) , ModifiedAt = table.Column<DateTime>("datetime2" , nullable: false) ,
                    ModifiedBy = table.Column<Guid>("uniqueidentifier" , nullable: true) , Name = table.Column<string>("nvarchar(max)" , nullable: false) ,
                    PhoneNumber = table.Column<string>("nvarchar(max)" , nullable: false) ,
                    WebSite = table.Column<string>("nvarchar(max)" , nullable: false) } ,
                constraints: table =>
                             {
                                 table.PrimaryKey("PK_Companies" , x => x.Id);
                                 table.ForeignKey("FK_Companies_TenantInfo_TenantInfosId" ,
                                     x => x.TenantInfosId ,
                                     "TenantInfo" ,
                                     "Id" ,
                                     onDelete: ReferentialAction.Restrict);
                             });

            migrationBuilder.CreateIndex("IX_TenantInfo_Identifier" , "TenantInfo" , "Identifier" , unique: true);

            migrationBuilder.CreateIndex("IX_Companies_TenantInfosId" , "Companies" , "TenantInfosId" , unique: true);
        }
    }
}