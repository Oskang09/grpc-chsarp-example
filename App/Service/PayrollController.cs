
using System;
using System.Collections.Generic;
using System.Linq;
using amantiq.constant;
using amantiq.model;
using amantiq.util;

namespace amantiq.service
{
    public class PayrollController : Controller
    {
        [Service("findPayrollById")]
        public object findPayrollById(Context request)
        {
            Validate(request, new { id = "string" });
            var id = request.Value<string>("id");
            using (var db = GetDatabase)
            {
                Payroll payroll = db.Payrolls.Find(id);
                if (payroll == null)
                {
                    throw Error.PayrollNotFound;
                }
                return payroll;
            }
        }

        [Service("findPayrolls")]
        public object findPayrolls(Context request)
        {
            Validate(request, new
            {
                limit = "integer",
                page = "integer",
                year = "integer",
                month = "integer",
                employeeId = "string",
            });

            using (var db = GetDatabase)
            {
                return db.Payrolls.Paginate(request.Value<int>("limit"), request.Value<int>("page"), (query) =>
                {
                    int year = request.ValueWithFallback<int>("year", 0);
                    int month = request.ValueWithFallback<int>("month", 0);
                    if (year != 0 && month == 0)
                    {
                        var start = new DateTime(year, 1, 1);
                        var end = new DateTime(year, 12, 31);
                        query = query.Where(x => x.Period >= start && x.Period <= end);
                    }

                    if (year != 0 && month != 0)
                    {
                        var start = new DateTime(year, month, 1);
                        var end = start.LastDayOfMonth();
                        query = query.Where(x => x.Period >= start && x.Period <= end);
                    }

                    string employeeId = request.ValueWithFallback<string>("employeeId", "");
                    if (employeeId != "")
                    {
                        query = query.Where(x => x.EmployeeId == employeeId);
                    }
                    return query;
                });
            }
        }

        [Service("findPayrollPackages")]
        public object findPayrollPackages(Context request)
        {
            Validate(request, new
            {
                limit = "integer",
                page = "integer",
                employeeId = "string",
                status = ValidatorExtension.EnumValues<PayrollPackageStatus>(),
            });

            using (var db = GetDatabase)
            {
                return db.PayrollPackages.Paginate(request.Value<int>("limit"), request.Value<int>("page"), (query) =>
                {
                    PayrollPackageStatus? status = request.ValueWithFallback<PayrollPackageStatus?>("status", null);
                    if (status != null)
                    {
                        query = query.Where(x => x.Status == status);
                    }

                    string employeeId = request.ValueWithFallback<string>("employeeId", "");
                    if (employeeId != "")
                    {
                        query = query.Where(x => x.EmployeeId == employeeId);
                    }

                    string companyId = request.ValueWithFallback<string>("companyId", "");
                    if (companyId != "")
                    {
                        query = query.Where(x => x.CompanyId == companyId);
                    }
                    return query;
                });
            }
        }

        [Service("createPayrollPackage")]
        public object createPayrollPackage(Context request)
        {
            Validate(request, new
            {
                employeeId = "required|string",
                salary = "required|numeric",
                benefits = Rules.Array("string"),
                leaves = Rules.Array("string"),
                earnings = Rules.ArrayMap(new
                {
                    name = "string",
                    value = "numeric",
                    isTaxable = "boolean"
                }),
                deductions = Rules.ArrayMap(new
                {
                    name = "string",
                    value = "numeric",
                }),
            });

            string employeeId = request.Value<string>("employeeId");
            Employee employee = request.FindEmployeeById(employeeId);
            using (var db = GetDatabase)
            using (var trx = db.Database.BeginTransaction())
            {
                List<PayrollPackage> packages = db.PayrollPackages
                    .AsQueryable()
                    .Where(x => x.EmployeeId == employeeId)
                    .Where(x => x.Status == PayrollPackageStatus.ACTIVE)
                    .ToList();
                packages.ForEach(x =>
                {
                    x.Status = PayrollPackageStatus.EXPIRED;
                    x.UpdatedDateTime = DateTime.Now;
                });
                db.SaveChanges();

                PayrollPackage package = new PayrollPackage();
                package.Id = GeneratorExtension.GenerateUNIQ();
                package.CompanyId = employee.CompanyId;
                package.EmployeeId = employeeId;
                package.Salary = request.Value<int>("salary");
                package.Benefits = request.ValueWithFallback<List<string>>("benefits", new List<string>());
                package.Leaves = request.ValueWithFallback<List<string>>("leaves", new List<string>());
                package.Earnings = request.ValueWithFallback<List<PayrollPackageEarning>>("earnings", new List<PayrollPackageEarning>());
                package.Deductions = request.ValueWithFallback<List<PayrollPackageDeduction>>("deductions", new List<PayrollPackageDeduction>());
                package.Status = PayrollPackageStatus.ACTIVE;
                package.CreatedDateTime = DateTime.Now;
                package.UpdatedDateTime = DateTime.Now;
                db.PayrollPackages.Add(package);
                db.SaveChanges();
                trx.Commit();

                request.Background("ensureEmployeeSpendAccount", (ctx) => PayrollController.processEmployeeSpendAccount(ctx, package, employee));
                return package;
            }
        }

        [Service("updatePayrollPackageById")]
        public object updatePayrollPackageById(Context request)
        {
            Validate(request, new
            {
                id = "required|string",
                salary = "numeric",
                benefits = Rules.Array("string"),
                leaves = Rules.Array("string"),
                earnings = Rules.ArrayMap(new
                {
                    name = "string",
                    value = "numeric",
                    isTaxable = "boolean"
                }),
                deductions = Rules.ArrayMap(new
                {
                    name = "string",
                    value = "numeric",
                }),
            });

            string id = request.Value<string>("id");
            using (var db = GetDatabase)
            {
                PayrollPackage package = db.PayrollPackages.Find(id);
                if (package == null)
                {
                    throw Error.PayrollPackageNotFound;
                }
                package.Salary = request.ValueWithFallback<int>("salary", package.Salary);
                package.Benefits = request.ValueWithFallback<List<string>>("benefits", package.Benefits);
                package.Leaves = request.ValueWithFallback<List<string>>("leaves", package.Leaves);
                package.Earnings = request.ValueWithFallback<List<PayrollPackageEarning>>("earnings", package.Earnings);
                package.Deductions = request.ValueWithFallback<List<PayrollPackageDeduction>>("deductions", package.Deductions);

                package.UpdatedDateTime = DateTime.Now;
                db.SaveChanges();

                request.Background("ensureEmployeeSpendAccount", (ctx) =>
                {
                    Employee employee = ctx.FindEmployeeById(package.EmployeeId);
                    PayrollController.processEmployeeSpendAccount(ctx, package, employee);
                });
                return package;
            }
        }

        public static void processEmployeeSpendAccount(Context context, PayrollPackage payroll, Employee employee)
        {
            using (var db = GetDatabase)
            {
                foreach (string leaveId in payroll.Leaves)
                {
                    SpendAccount account = db.SpendAccounts.FirstOrDefault(x => x.PackageType == SpendAccountPackageType.LEAVE && x.PackageId == leaveId);
                    if (account == null)
                    {
                        account = new SpendAccount();
                        account.Id = GeneratorExtension.GenerateUNIQ();
                        account.PackageId = leaveId;
                        account.CreatedDateTime = DateTime.Now;
                        account.UpdatedDateTime = DateTime.Now;
                        account.CompanyId = employee.CompanyId;
                        account.EmployeeId = employee.Id;
                        account.PackageType = SpendAccountPackageType.LEAVE;
                        account.ValueType = SpendAccountValueType.TIMES;
                        account.CreatedDateTime = DateTime.Now;
                        account.UpdatedDateTime = DateTime.Now;
                        account.Status = SpendAccountStatus.BUSY;
                        db.SpendAccounts.Add(account);
                        db.SaveChanges();

                        context.Background("processLeaveSpendAccount", (ctx) => LeaveController.processLeaveSpendAccount(ctx, account.Id));
                    }
                }

                foreach (string benefitId in payroll.Benefits)
                {
                    SpendAccount account = db.SpendAccounts.FirstOrDefault(x => x.PackageType == SpendAccountPackageType.BENEFIT && x.PackageId == benefitId);
                    if (account == null)
                    {
                        BenefitPackage package = db.BenefitPackages.Find(benefitId);
                        if (package == null)
                        {
                            continue;
                        }

                        account = new SpendAccount();
                        account.Id = GeneratorExtension.GenerateUNIQ();
                        account.PackageId = benefitId;
                        account.CreatedDateTime = DateTime.Now;
                        account.UpdatedDateTime = DateTime.Now;
                        account.CompanyId = employee.CompanyId;
                        account.EmployeeId = employee.Id;
                        account.PackageType = SpendAccountPackageType.BENEFIT;
                        account.ValueType = (SpendAccountValueType)package.ValueType;
                        account.CreatedDateTime = DateTime.Now;
                        account.UpdatedDateTime = DateTime.Now;
                        account.Status = SpendAccountStatus.BUSY;
                        db.SpendAccounts.Add(account);
                        db.SaveChanges();

                        context.Background("processBenefitSpendAccount", (ctx) => BenefitController.processBenefitSpendAccount(ctx, account.Id));
                    }
                }
            }
        }
    }
}