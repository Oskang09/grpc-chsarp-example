﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using amantiq;
using amantiq.model;

namespace company_service.Migrations
{
    [DbContext(typeof(Database))]
    [Migration("20201017192528_new3")]
    partial class new3
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("amantiq.model.Benefit", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(20)");

                    b.Property<string>("CompanyId")
                        .HasColumnType("varchar(20)");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("EmployeeId")
                        .HasColumnType("varchar(20)");

                    b.Property<bool>("IsTaxable")
                        .HasColumnType("boolean");

                    b.Property<string>("PackageId")
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Reason")
                        .HasColumnType("text");

                    b.Property<string>("SpendAccountRecordId")
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdateDateTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("Value")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("benefits");
                });

            modelBuilder.Entity("amantiq.model.BenefitPackage", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(20)");

                    b.Property<string>("CarryForwardPeriod")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("CarryForwardPeriodValue")
                        .HasColumnType("integer");

                    b.Property<int>("CarryForwardValue")
                        .HasColumnType("integer");

                    b.Property<string>("CompanyId")
                        .HasColumnType("varchar(20)");

                    b.Property<bool>("IsCarryForward")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsProRated")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("PeriodType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("PeriodValue")
                        .HasColumnType("integer");

                    b.Property<int>("Value")
                        .HasColumnType("integer");

                    b.Property<string>("ValueType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("benefit_packages");
                });

            modelBuilder.Entity("amantiq.model.Company", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(20)");

                    b.Property<string>("BranchOf")
                        .HasColumnType("varchar(20)");

                    b.Property<List<string>>("Holidays")
                        .HasColumnType("text[]");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("RegistrationNumber")
                        .HasColumnType("text");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("companies");
                });

            modelBuilder.Entity("amantiq.model.Holiday", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(20)");

                    b.Property<List<string>>("Applicable")
                        .HasColumnType("text[]");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("End")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<DateTime>("Start")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("UpdatedDateTime")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.ToTable("holdays");
                });

            modelBuilder.Entity("amantiq.model.Leave", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(20)");

                    b.Property<string>("CompanyId")
                        .HasColumnType("varchar(20)");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("EmployeeId")
                        .HasColumnType("varchar(20)");

                    b.Property<DateTime>("End")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("PackageId")
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Reason")
                        .HasColumnType("text");

                    b.Property<string>("SpendAccountRecordId")
                        .HasColumnType("varchar(20)");

                    b.Property<DateTime>("Start")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdateDateTime")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.ToTable("leaves");
                });

            modelBuilder.Entity("amantiq.model.LeavePackage", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(20)");

                    b.Property<string>("CarryForwardPeriod")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("CarryForwardPeriodValue")
                        .HasColumnType("integer");

                    b.Property<int>("CarryForwardValue")
                        .HasColumnType("integer");

                    b.Property<string>("CompanyId")
                        .HasColumnType("varchar(20)");

                    b.Property<bool>("IsCarryForward")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsProRated")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("PeriodType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("PeriodValue")
                        .HasColumnType("integer");

                    b.Property<string>("Type")
                        .HasColumnType("text");

                    b.Property<int>("Value")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("leave_packages");
                });

            modelBuilder.Entity("amantiq.model.Payroll", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("Company")
                        .HasColumnType("text");

                    b.Property<string>("CompanyId")
                        .HasColumnType("varchar(20)");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Date")
                        .HasColumnType("text");

                    b.Property<List<PayrollDeduction>>("Deductions")
                        .HasColumnType("jsonb");

                    b.Property<List<PayrollEarning>>("Earnings")
                        .HasColumnType("jsonb");

                    b.Property<int>("EmployeeEis")
                        .HasColumnType("integer");

                    b.Property<string>("EmployeeEisNo")
                        .HasColumnType("text");

                    b.Property<int>("EmployeeEpf")
                        .HasColumnType("integer");

                    b.Property<string>("EmployeeEpfNo")
                        .HasColumnType("text");

                    b.Property<string>("EmployeeId")
                        .HasColumnType("varchar(20)");

                    b.Property<string>("EmployeeIncomeTaxNo")
                        .HasColumnType("text");

                    b.Property<string>("EmployeeNRIC")
                        .HasColumnType("text");

                    b.Property<string>("EmployeeName")
                        .HasColumnType("text");

                    b.Property<int>("EmployeeSalary")
                        .HasColumnType("integer");

                    b.Property<int>("EmployeeSocso")
                        .HasColumnType("integer");

                    b.Property<string>("EmployeeSocsoNo")
                        .HasColumnType("text");

                    b.Property<int>("EmployeeTax")
                        .HasColumnType("integer");

                    b.Property<int>("EmployeeZakat")
                        .HasColumnType("integer");

                    b.Property<int>("EmployerEis")
                        .HasColumnType("integer");

                    b.Property<int>("EmployerEpf")
                        .HasColumnType("integer");

                    b.Property<int>("EmployerSocso")
                        .HasColumnType("integer");

                    b.Property<int>("NetPay")
                        .HasColumnType("integer");

                    b.Property<string>("PackageId")
                        .HasColumnType("varchar(20)");

                    b.Property<string>("PayPeriod")
                        .HasColumnType("text");

                    b.Property<DateTime>("Period")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("TotalBenefits")
                        .HasColumnType("integer");

                    b.Property<int>("TotalDeductions")
                        .HasColumnType("integer");

                    b.Property<int>("TotalEarnings")
                        .HasColumnType("integer");

                    b.Property<int>("TotalTaxableBenefits")
                        .HasColumnType("integer");

                    b.Property<int>("TotalTaxableEarnings")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedDateTime")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.ToTable("payrolls");
                });

            modelBuilder.Entity("amantiq.model.PayrollPackage", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(20)");

                    b.Property<List<string>>("Benefits")
                        .HasColumnType("jsonb");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<List<PayrollPackageDeduction>>("Deductions")
                        .HasColumnType("jsonb");

                    b.Property<List<PayrollPackageEarning>>("Earnings")
                        .HasColumnType("jsonb");

                    b.Property<string>("EmployeeId")
                        .HasColumnType("varchar(20)");

                    b.Property<List<string>>("Leaves")
                        .HasColumnType("jsonb");

                    b.Property<int>("Salary")
                        .HasColumnType("integer");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedDateTime")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.ToTable("payroll_packages");
                });

            modelBuilder.Entity("amantiq.model.SpendAccount", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(20)");

                    b.Property<int>("Balance")
                        .HasColumnType("integer");

                    b.Property<string>("CompanyId")
                        .HasColumnType("varchar(20)");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("EmployeeId")
                        .HasColumnType("varchar(20)");

                    b.Property<int>("ExpiryBalance")
                        .HasColumnType("integer");

                    b.Property<string>("PackageId")
                        .HasColumnType("varchar(20)");

                    b.Property<string>("PackageType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("SpendBalance")
                        .HasColumnType("integer");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedDateTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("ValueType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("spend_account");
                });

            modelBuilder.Entity("amantiq.model.SpendAccountRecord", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(20)");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("EmployeeId")
                        .HasColumnType("varchar(20)");

                    b.Property<string>("PackageId")
                        .HasColumnType("varchar(20)");

                    b.Property<string>("PackageType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("RootSpendAccountRecordId")
                        .HasColumnType("varchar(20)");

                    b.Property<int>("Sequence")
                        .HasColumnType("integer");

                    b.Property<string>("SpendAccountId")
                        .HasColumnType("varchar(20)");

                    b.Property<int>("SpendBalance")
                        .HasColumnType("integer");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdateDateTime")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.ToTable("spend_account_records");
                });
#pragma warning restore 612, 618
        }
    }
}