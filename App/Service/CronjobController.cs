using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using amantiq.constant;
using amantiq.model;
using amantiq.util;
using Microsoft.EntityFrameworkCore;

namespace amantiq.service
{
    public class CronjobController : Controller
    {
        private static string SALARY_DISPLAY = "Salary";
        private static string ZAKAT_DISPLAY = "Zakat";

        [Service("cronjob.employeePayroll")]
        public async void calculatePayroll(Context request)
        {
            Validate(request, new
            {
                employeeId = "required|string",
                when = "datetime"
            });

            var id = request.Value<string>("employeeId");
            DateTime current = request.ValueWithFallback<DateTime>("when", DateTime.Now);
            DateTime lastMonth = current.AddMonths(-1);
            using (var db = GetDatabase)
            {
                Employee employee = request.FindEmployeeById(id);
                Company company = db.Companies.Find(employee.CompanyId);
                PayrollPackage package = db.PayrollPackages.FirstOrDefault(x => x.EmployeeId == id && x.Status == PayrollPackageStatus.ACTIVE);
                if (package == null)
                {
                    throw Error.EmployeeNoActivePayroll;
                }

                Payroll payroll = db.Payrolls.AsQueryable()
                    .Where(x => x.PackageId == package.Id)
                    .Where(x => x.CompanyId == company.Id)
                    .Where(x => x.EmployeeId == employee.Id)
                    .Where(x => x.PayPeriod == lastMonth.ToString("yyyy MMMM"))
                    .FirstOrDefault();
                if (payroll == null)
                {
                    payroll = new Payroll();
                    payroll.Id = GeneratorExtension.GenerateUNIQ();
                    payroll.CreatedDateTime = DateTime.Now;
                    db.Entry(payroll).State = EntityState.Added;
                }

                DateTime firstDayOfMonth = DateTimeExtension.FirstDayOfMonth(lastMonth);
                DateTime lastDayOfMonth = DateTimeExtension.LastDayOfMonth(lastMonth);
                int numOfDays = DateTimeExtension.DaysInMonth(lastMonth);
                int deductionLeavePerDay = package.Salary / numOfDays;

                payroll.PackageId = package.Id;
                payroll.CompanyId = company.Id;
                payroll.EmployeeId = employee.Id;
                payroll.Period = lastMonth;
                payroll.Company = company.Name;
                payroll.Date = current.ToString("dd/MM/yyyy");
                payroll.PayPeriod = lastMonth.ToString("yyyy MMMM");
                payroll.EmployeeSalary = package.Salary;
                payroll.EmployeeName = employee.Name;
                payroll.EmployeeNRIC = employee.Nationality.IdentityNo;
                payroll.EmployeeEpfNo = employee.Statutory.Epf.AccountNo;
                payroll.EmployeeSocsoNo = employee.Statutory.Socso.AccountNo;
                payroll.EmployeeEisNo = employee.Statutory.Eis.AccountNo;
                payroll.EmployeeIncomeTaxNo = employee.Statutory.Tax.AccountNo;
                payroll.Deductions = new List<PayrollDeduction>();
                payroll.Earnings = new List<PayrollEarning>();
                payroll.Earnings.Add(new PayrollEarning
                {
                    Name = SALARY_DISPLAY,
                    Value = package.Salary,
                    IsTaxable = true
                });

                package.Earnings.ForEach(earning =>
                {
                    payroll.Earnings.Add(new PayrollEarning
                    {
                        Name = earning.Name,
                        Value = earning.Value,
                        IsTaxable = earning.IsTaxable,
                    });
                });

                package.Deductions.ForEach(deduction =>
                {
                    payroll.Deductions.Add(new PayrollDeduction
                    {
                        Name = deduction.Name,
                        Value = deduction.Value
                    });
                });

                Task benefitTask = Task.Run(() =>
                {
                    List<Benefit> benefits = null;
                    using (var innerDB = GetDatabase)
                    {
                        benefits = innerDB.Benefits.AsQueryable()
                        .Where(x => x.EmployeeId == employee.Id)
                        .Where(x => x.CompanyId == employee.CompanyId)
                        .Where(x => x.CreatedDateTime >= firstDayOfMonth && x.CreatedDateTime <= lastDayOfMonth)
                        .Where(x => x.Status == BenefitStatus.APPROVED)
                        .Where(x => x.SpendAccountRecordId != null)
                        .ToList();
                    }

                    benefits.ForEach(benefit =>
                    {
                        db.Entry(benefit).State = EntityState.Detached;

                        int spendBalance = 0;
                        SpendAccountRecord record = db.SpendAccountRecords.Find(benefit.SpendAccountRecordId);
                        spendBalance = record.SpendBalance;

                        List<SpendAccountRecord> childRecords = db.SpendAccountRecords
                            .AsQueryable()
                            .Where(x => x.RootSpendAccountRecordId == record.Id)
                            .ToList();
                        foreach (SpendAccountRecord child in childRecords)
                        {
                            spendBalance += child.SpendBalance;
                        }

                        int earns = benefit.Value - spendBalance;
                        payroll.Earnings.Add(new PayrollEarning
                        {
                            Name = benefit.Title,
                            Value = earns,
                            IsTaxable = benefit.IsTaxable,
                        });
                    });
                });

                Task leaveTask = Task.Run(() =>
                {
                    List<Leave> leaves = null;
                    using (var innerDB = GetDatabase)
                    {
                        leaves = innerDB.Leaves.AsQueryable()
                            .Where(x => x.EmployeeId == employee.Id)
                            .Where(x => x.CompanyId == employee.CompanyId)
                            .Where(x => (x.Start >= firstDayOfMonth && x.Start <= lastDayOfMonth) || (x.Start <= lastDayOfMonth && (x.End >= firstDayOfMonth && x.End <= lastDayOfMonth)))
                            .Where(x => x.Status == LeaveStatus.APPROVED)
                            .ToList();
                    }

                    leaves.ForEach(leave =>
                    {
                        if (leave.SpendAccountRecordId == null)
                        {
                            payroll.Deductions.Add(new PayrollDeduction
                            {
                                Name = leave.Title,
                                Value = leave.getBalanceValue(lastMonth)
                            });
                        }
                        else
                        {
                            int spendBalance = 0;
                            SpendAccountRecord record = db.SpendAccountRecords.Find(leave.SpendAccountRecordId);
                            spendBalance = record.SpendBalance;

                            List<SpendAccountRecord> childRecords = db.SpendAccountRecords
                                .AsQueryable()
                                .Where(x => x.RootSpendAccountRecordId == record.Id)
                                .ToList();
                            foreach (SpendAccountRecord child in childRecords)
                            {
                                spendBalance += child.SpendBalance;
                            }

                            if (leave.IsOverMonth())
                            {
                                if (leave.IsForwardMonth(lastMonth))
                                {
                                    int balance = spendBalance - leave.getBalanceValue(lastMonth);
                                    if (balance < 0)
                                    {
                                        payroll.Deductions.Add(new PayrollDeduction
                                        {
                                            Name = leave.Title,
                                            Value = Math.Abs(balance) * deductionLeavePerDay
                                        });
                                    }
                                }

                                if (leave.IsBackwardMonth(lastMonth))
                                {
                                    int lastMonthBalance = spendBalance - leave.getBalanceValue(lastMonth.AddMonths(-1));
                                    if (lastMonthBalance < 0)
                                    {
                                        payroll.Deductions.Add(new PayrollDeduction
                                        {
                                            Name = leave.Title,
                                            Value = leave.getBalanceValue(lastMonth) * deductionLeavePerDay
                                        });
                                    }
                                    else
                                    {
                                        payroll.Deductions.Add(new PayrollDeduction
                                        {
                                            Name = leave.Title,
                                            Value = Math.Abs(lastMonthBalance - leave.getBalanceValue(lastMonth)) * deductionLeavePerDay
                                        });
                                    }
                                }
                            }
                            else
                            {
                                int unpaidBalance = spendBalance - leave.getBalanceValue();
                                if (unpaidBalance < 0)
                                {
                                    payroll.Deductions.Add(new PayrollDeduction
                                    {
                                        Name = leave.Title,
                                        Value = Math.Abs(unpaidBalance) * deductionLeavePerDay
                                    });
                                }
                            }
                        }
                    });
                });

                await benefitTask;
                await leaveTask;

                payroll.TotalEarnings = payroll.Earnings
                    .AsQueryable()
                    .Sum(x => x.Value);
                payroll.TotalTaxableEarnings = payroll.Earnings
                    .AsQueryable()
                    .Where(x => x.IsTaxable)
                    .Sum(x => x.Value);
                payroll.TotalBenefits = payroll.Earnings
                    .AsQueryable()
                    .Where(x => x.Name != SALARY_DISPLAY)
                    .Sum(x => x.Value);
                payroll.TotalTaxableBenefits = payroll.Earnings
                    .AsQueryable()
                    .Where(x => x.Name != SALARY_DISPLAY)
                    .Where(x => x.IsTaxable)
                    .Sum(x => x.Value);
                payroll.TotalDeductions = payroll.Deductions
                    .AsQueryable()
                    .Sum(x => x.Value);

                payroll.EmployeeEpf = payroll.TotalTaxableEarnings.getEPF(employee.Statutory.Epf.Rate);
                payroll.EmployeeSocso = payroll.TotalTaxableEarnings.getEmployeeSocso();
                payroll.EmployeeEis = payroll.TotalTaxableEarnings.getEIS();
                if (employee.Statutory.Zakat != null)
                {
                    switch (employee.Statutory.Zakat.Type)
                    {
                        case "FIXED":
                            payroll.EmployeeZakat = employee.Statutory.Zakat.Value;
                            break;
                        case "PERCENTAGE":
                            payroll.EmployeeZakat = payroll.TotalTaxableEarnings.getZakat(employee.Statutory.Zakat.Value);
                            break;
                    }

                    payroll.Deductions.Add(new PayrollDeduction
                    {
                        Name = ZAKAT_DISPLAY,
                        Value = payroll.EmployeeZakat
                    });
                }
                // payroll.EmployeeTax = payroll.TotalTaxableEarnings
                // TODO: calcualtion for income tax

                payroll.EmployerEpf = payroll.TotalTaxableEarnings.getEPF(13);
                payroll.EmployerSocso = payroll.TotalTaxableEarnings.getEmployerSocso();
                payroll.EmployerEis = payroll.TotalTaxableEarnings.getEIS();
                payroll.TotalDeductions += (payroll.EmployeeEpf + payroll.EmployeeSocso + payroll.EmployeeEis + payroll.EmployeeTax + payroll.EmployeeZakat);

                int nonTaxableEarnigns = payroll.TotalEarnings - payroll.TotalTaxableEarnings;
                payroll.NetPay = payroll.TotalTaxableEarnings + nonTaxableEarnigns - payroll.TotalDeductions;
                payroll.UpdatedDateTime = DateTime.Now;
                db.SaveChanges();
            }
        }
    }
}