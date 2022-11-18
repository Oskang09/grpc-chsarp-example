using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using amantiq.model;

namespace company_service.Migrations
{
    public partial class new3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "company_holidays");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "leaves",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDateTime",
                table: "leaves",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "benefits",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDateTime",
                table: "benefits",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "payroll_packages",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(20)", nullable: false),
                    EmployeeId = table.Column<string>(type: "varchar(20)", nullable: true),
                    Salary = table.Column<int>(nullable: false),
                    Benefits = table.Column<List<string>>(type: "jsonb", nullable: true),
                    Leaves = table.Column<List<string>>(type: "jsonb", nullable: true),
                    Earnings = table.Column<List<PayrollPackageEarning>>(type: "jsonb", nullable: true),
                    Deductions = table.Column<List<PayrollPackageDeduction>>(type: "jsonb", nullable: true),
                    Status = table.Column<string>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    UpdatedDateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payroll_packages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "payrolls",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Period = table.Column<DateTime>(nullable: false),
                    PackageId = table.Column<string>(type: "varchar(20)", nullable: true),
                    EmployeeId = table.Column<string>(type: "varchar(20)", nullable: true),
                    CompanyId = table.Column<string>(type: "varchar(20)", nullable: true),
                    Company = table.Column<string>(nullable: true),
                    Date = table.Column<string>(nullable: true),
                    PayPeriod = table.Column<string>(nullable: true),
                    EmployeeSalary = table.Column<int>(nullable: false),
                    EmployeeName = table.Column<string>(nullable: true),
                    EmployeeNRIC = table.Column<string>(nullable: true),
                    EmployeeIncomeTaxNo = table.Column<string>(nullable: true),
                    EmployeeEpfNo = table.Column<string>(nullable: true),
                    EmployeeSocsoNo = table.Column<string>(nullable: true),
                    EmployeeEisNo = table.Column<string>(nullable: true),
                    EmployeeEpf = table.Column<int>(nullable: false),
                    EmployeeSocso = table.Column<int>(nullable: false),
                    EmployeeZakat = table.Column<int>(nullable: false),
                    EmployeeEis = table.Column<int>(nullable: false),
                    EmployeeTax = table.Column<int>(nullable: false),
                    EmployerEpf = table.Column<int>(nullable: false),
                    EmployerSocso = table.Column<int>(nullable: false),
                    EmployerEis = table.Column<int>(nullable: false),
                    Deductions = table.Column<List<PayrollDeduction>>(type: "jsonb", nullable: true),
                    Earnings = table.Column<List<PayrollEarning>>(type: "jsonb", nullable: true),
                    TotalBenefits = table.Column<int>(nullable: false),
                    TotalTaxableBenefits = table.Column<int>(nullable: false),
                    TotalEarnings = table.Column<int>(nullable: false),
                    TotalTaxableEarnings = table.Column<int>(nullable: false),
                    TotalDeductions = table.Column<int>(nullable: false),
                    NetPay = table.Column<int>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    UpdatedDateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payrolls", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "payroll_packages");

            migrationBuilder.DropTable(
                name: "payrolls");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "leaves");

            migrationBuilder.DropColumn(
                name: "UpdateDateTime",
                table: "leaves");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "benefits");

            migrationBuilder.DropColumn(
                name: "UpdateDateTime",
                table: "benefits");

            migrationBuilder.CreateTable(
                name: "company_holidays",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Holidays = table.Column<List<string>>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_company_holidays", x => x.Id);
                });
        }
    }
}
