using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace company_service.Migrations
{
    public partial class @new : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "benefit_packages",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(20)", nullable: false),
                    CompanyId = table.Column<string>(type: "varchar(20)", nullable: true),
                    Name = table.Column<string>(nullable: true),
                    ValueType = table.Column<string>(nullable: false),
                    Value = table.Column<int>(nullable: false),
                    PeriodType = table.Column<string>(nullable: false),
                    PeriodValue = table.Column<int>(nullable: false),
                    IsProRated = table.Column<bool>(nullable: false),
                    IsCarryForward = table.Column<bool>(nullable: false),
                    CarryForwardPeriod = table.Column<string>(nullable: false),
                    CarryForwardPeriodValue = table.Column<int>(nullable: false),
                    CarryForwardValue = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_benefit_packages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "benefits",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(20)", nullable: false),
                    SpendAccountRecordId = table.Column<string>(type: "varchar(20)", nullable: true),
                    CompanyId = table.Column<string>(type: "varchar(20)", nullable: true),
                    EmployeeId = table.Column<string>(type: "varchar(20)", nullable: true),
                    PackageId = table.Column<string>(type: "varchar(20)", nullable: true),
                    Status = table.Column<string>(nullable: false),
                    Value = table.Column<int>(nullable: false),
                    IsTaxable = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_benefits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "companies",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(20)", nullable: false),
                    Admins = table.Column<List<string>>(type: "jsonb", nullable: true),
                    AmantiqAdmins = table.Column<List<string>>(type: "jsonb", nullable: true),
                    BranchOf = table.Column<string>(type: "varchar(20)", nullable: true),
                    Name = table.Column<string>(nullable: true),
                    RegistrationNumber = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "company_holidays",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Holidays = table.Column<List<string>>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_company_holidays", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "holdays",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(20)", nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Start = table.Column<DateTime>(nullable: false),
                    End = table.Column<DateTime>(nullable: false),
                    Applicable = table.Column<List<string>>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    UpdatedDateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_holdays", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "leave_packages",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(20)", nullable: false),
                    CompanyId = table.Column<string>(type: "varchar(20)", nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: false),
                    Value = table.Column<int>(nullable: false),
                    PeriodType = table.Column<string>(nullable: false),
                    PeriodValue = table.Column<int>(nullable: false),
                    IsProRated = table.Column<bool>(nullable: false),
                    IsCarryForward = table.Column<bool>(nullable: false),
                    CarryForwardPeriod = table.Column<string>(nullable: false),
                    CarryForwardPeriodValue = table.Column<int>(nullable: false),
                    CarryForwardValue = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_leave_packages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "leaves",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(20)", nullable: false),
                    SpendAccountRecordId = table.Column<string>(type: "varchar(20)", nullable: true),
                    CompanyId = table.Column<string>(type: "varchar(20)", nullable: true),
                    EmployeeId = table.Column<string>(type: "varchar(20)", nullable: true),
                    Start = table.Column<DateTime>(nullable: false),
                    End = table.Column<DateTime>(nullable: false),
                    Type = table.Column<string>(nullable: false),
                    Status = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_leaves", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "spend_account",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(20)", nullable: false),
                    PackageId = table.Column<string>(type: "varchar(20)", nullable: true),
                    EmployeeId = table.Column<string>(type: "varchar(20)", nullable: true),
                    CompanyId = table.Column<string>(type: "varchar(20)", nullable: true),
                    PackageType = table.Column<string>(nullable: false),
                    ValueType = table.Column<string>(nullable: false),
                    Balance = table.Column<int>(nullable: false),
                    SpendBalance = table.Column<int>(nullable: false),
                    ExpiryBalance = table.Column<int>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    UpdatedDateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_spend_account", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "spend_account_records",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(20)", nullable: false),
                    SpendAccountId = table.Column<string>(type: "varchar(20)", nullable: true),
                    PackageId = table.Column<string>(type: "varchar(20)", nullable: true),
                    EmployeeId = table.Column<string>(type: "varchar(20)", nullable: true),
                    Sequence = table.Column<int>(nullable: false),
                    PackageType = table.Column<string>(nullable: false),
                    SpendBalance = table.Column<int>(nullable: false),
                    Status = table.Column<string>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    UpdateDateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_spend_account_records", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "benefit_packages");

            migrationBuilder.DropTable(
                name: "benefits");

            migrationBuilder.DropTable(
                name: "companies");

            migrationBuilder.DropTable(
                name: "company_holidays");

            migrationBuilder.DropTable(
                name: "holdays");

            migrationBuilder.DropTable(
                name: "leave_packages");

            migrationBuilder.DropTable(
                name: "leaves");

            migrationBuilder.DropTable(
                name: "spend_account");

            migrationBuilder.DropTable(
                name: "spend_account_records");
        }
    }
}
