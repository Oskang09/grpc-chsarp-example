using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace company_service.Migrations
{
    public partial class new2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "leaves");

            migrationBuilder.DropColumn(
                name: "Admins",
                table: "companies");

            migrationBuilder.DropColumn(
                name: "AmantiqAdmins",
                table: "companies");

            migrationBuilder.AddColumn<string>(
                name: "RootSpendAccountRecordId",
                table: "spend_account_records",
                type: "varchar(20)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "spend_account",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "leaves",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PackageId",
                table: "leaves",
                type: "varchar(20)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Reason",
                table: "leaves",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "leaves",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "leave_packages",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<List<string>>(
                name: "Holidays",
                table: "companies",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "benefits",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Reason",
                table: "benefits",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "benefits",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RootSpendAccountRecordId",
                table: "spend_account_records");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "spend_account");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "leaves");

            migrationBuilder.DropColumn(
                name: "PackageId",
                table: "leaves");

            migrationBuilder.DropColumn(
                name: "Reason",
                table: "leaves");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "leaves");

            migrationBuilder.DropColumn(
                name: "Holidays",
                table: "companies");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "benefits");

            migrationBuilder.DropColumn(
                name: "Reason",
                table: "benefits");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "benefits");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "leaves",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "leave_packages",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<List<string>>(
                name: "Admins",
                table: "companies",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AddColumn<List<string>>(
                name: "AmantiqAdmins",
                table: "companies",
                type: "jsonb",
                nullable: true);
        }
    }
}
