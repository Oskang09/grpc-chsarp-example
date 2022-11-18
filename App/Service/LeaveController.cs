using System;
using System.Collections.Generic;
using System.Linq;
using amantiq.constant;
using amantiq.model;
using amantiq.util;

namespace amantiq.service
{

    public class LeaveController : Controller
    {
        [Service("createLeavePackage")]
        public object createLeavePackage(Context request)
        {
            Validate(request, new
            {
                name = "required|string",
                type = "required|string",
                value = "required|integer",
                periodType = "required|" + ValidatorExtension.EnumValues<LeavePackagePeriod>(),
                periodValue = "required|integer",
                isProRated = "required|boolean",
                isCarryForward = "required|boolean",
                carryForwardPeriod = ValidatorExtension.EnumValues<BenefitPackageCarryForwardPeriod>(),
                carryForwardPeriodValue = "integer",
                carryForwardValue = "integer",
                companyId = "required|string",
            });

            if (request.Value<bool>("isCarryForward"))
            {
                Validate(request, new
                {
                    carryForwardPeriod = "required|" + ValidatorExtension.EnumValues<LeavePackageCarryForwardPeriod>(),
                    carryForwardPeriodValue = "required|integer",
                    carryForwardValue = "required|integer",
                });
            }

            LeavePackage package = new LeavePackage();
            package.Id = GeneratorExtension.GenerateUNIQ();
            package.Name = request.Value<string>("name");
            package.Type = request.Value<string>("type");
            package.Value = request.Value<int>("value");
            package.PeriodType = request.ObjectValue<LeavePackagePeriod>("periodType");
            package.PeriodValue = request.Value<int>("periodValue");
            package.IsProRated = request.Value<bool>("isProRated");
            package.IsCarryForward = request.Value<bool>("isCarryForward");
            package.CarryForwardPeriod = request.ObjectValue<LeavePackageCarryForwardPeriod>("carryForwardPeriod");
            package.CarryForwardPeriodValue = request.Value<int>("carryForwardPeriodValue");
            package.CarryForwardValue = request.Value<int>("carryForwardValue");
            package.CompanyId = request.Value<string>("companyId");
            using (var db = GetDatabase)
            {
                db.LeavePackages.Add(package);
                db.SaveChanges();
            }
            return package;
        }

        [Service("findLeavePackages")]
        public object findLeavePackages(Context request)
        {
            Validate(request, new
            {
                type = "string",
                companyId = "string",
                limit = "integer",
                page = "integer",
            });

            using (var db = GetDatabase)
            {
                return db.LeavePackages.Paginate(request.Value<int>("limit"), request.Value<int>("page"), (query) =>
                {
                    string companyId = request.ValueWithFallback("companyId", "");
                    if (companyId != "")
                    {
                        query = query.Where(x => x.CompanyId == companyId);
                    }

                    string type = request.ValueWithFallback<string>("type", "");
                    if (type != "")
                    {
                        query = query.Where(x => x.Type == type);
                    }

                    return query;
                });
            }
        }

        [Service("findLeavePackageById")]
        public object findLeavePackageById(Context request)
        {
            Validate(request, new
            {
                id = "required|string",
            });

            var id = request.Value<string>("id");
            using (var db = GetDatabase)
            {
                LeavePackage package = db.LeavePackages.Find(id);
                if (package == null)
                {
                    request.status = 404;
                    throw Error.LeaveNotFound;
                }
                return package;
            }
        }

        [Service("updateLeavePackageById")]
        public object updateLeavePackageById(Context request)
        {
            Validate(request, new
            {
                id = "required|string",
                name = "string",
                type = "string",
                value = "integer",
                periodType = ValidatorExtension.EnumValues<LeavePackagePeriod>(),
                periodValue = "integer",
                isProRated = "boolean",
                isCarryForward = "boolean",
                carryForwardPeriod = ValidatorExtension.EnumValues<LeavePackageCarryForwardPeriod>(),
                carryForwardPeriodValue = "integer",
                carryForwardValue = "integer",
                companyId = "string",
            });

            var id = request.Value<string>("id");
            using (var db = GetDatabase)
            {
                LeavePackage package = db.LeavePackages.Find(id);
                package.Name = request.ValueWithFallback<string>("name", package.Name);
                package.Type = request.ValueWithFallback<string>("type", package.Type);
                package.Value = request.ValueWithFallback<int>("value", package.Value);
                package.PeriodType = request.ValueWithFallback<LeavePackagePeriod>("periodType", package.PeriodType);
                package.PeriodValue = request.ValueWithFallback<int>("periodValue", package.PeriodValue);
                package.IsProRated = request.ValueWithFallback<bool>("isProRated", package.IsProRated);
                package.IsCarryForward = request.ValueWithFallback<bool>("isCarryForward", package.IsCarryForward);
                package.CarryForwardPeriod = request.ValueWithFallback<LeavePackageCarryForwardPeriod>("carryForwardPeriod", package.CarryForwardPeriod);
                package.CarryForwardPeriodValue = request.ValueWithFallback<int>("carryForwardPeriodValue", package.CarryForwardPeriodValue);
                package.CarryForwardValue = request.ValueWithFallback<int>("carryForwardValue", package.CarryForwardValue);
                package.CompanyId = request.ValueWithFallback<string>("companyId", package.CompanyId);
                db.SaveChanges();
                return package;
            }
        }

        [Service("removeLeavePackageById")]
        public object removeLeavePackageById(Context request)
        {
            Validate(request, new
            {
                id = "required|string",
            });

            var id = request.Value<string>("id");
            using (var db = GetDatabase)
            {
                LeavePackage package = db.LeavePackages.Find(id);
                db.LeavePackages.Remove(package);
                db.SaveChanges();
                return package;
            }
        }

        [Service("createLeave")]
        public object createLeave(Context request)
        {
            Validate(request, new
            {
                start = "required|datetime",
                end = "datetime",
                title = "string",
                description = "string",
                status = ValidatorExtension.EnumValues<LeaveStatus>(),
                packageId = "required|string",
                employeeId = "required|string",
                companyId = "required|string",
            });

            Leave leave = new Leave();
            leave.Id = GeneratorExtension.GenerateUNIQ();
            leave.Title = request.Value<string>("title");
            leave.Description = request.Value<string>("description");
            leave.Start = request.Value<DateTime>("start");
            leave.End = request.ValueWithFallback<DateTime>("end", default);
            leave.Status = request.ValueWithFallback<LeaveStatus>("status", LeaveStatus.PENDING);
            leave.PackageId = request.Value<string>("packageId");
            leave.EmployeeId = request.Value<string>("employeeId");
            leave.CompanyId = request.Value<string>("companyId");
            using (var db = GetDatabase)
            using (var trx = db.Database.BeginTransaction())
            {
                db.Leaves.Add(leave);
                db.SaveChanges();
                processLeave(request, db, leave);
                trx.Commit();
            }
            return leave;
        }

        [Service("findLeaves")]
        public object findLeaves(Context request)
        {
            Validate(request, new
            {
                limit = "integer",
                page = "integer",
                status = ValidatorExtension.EnumValues<LeaveStatus>(),
                packageId = "string",
                companyId = "string",
                employeeId = "string",
            });

            using (var db = GetDatabase)
            {
                return db.Leaves.Paginate(request.Value<int>("limit"), request.Value<int>("page"), (query) =>
                {
                    LeaveStatus? status = request.ValueWithFallback<LeaveStatus?>("status", null);
                    if (status != null)
                    {
                        query = query.Where(x => x.Status == status);
                    }

                    string companyId = request.ValueWithFallback("companyId", "");
                    if (companyId != "")
                    {
                        query = query.Where(x => x.CompanyId == companyId);
                    }

                    string employeeId = request.ValueWithFallback("employeeId", "");
                    if (employeeId != "")
                    {
                        query = query.Where(x => x.EmployeeId == employeeId);
                    }

                    string packageId = request.ValueWithFallback("packageId", "");
                    if (packageId != "")
                    {
                        query = query.Where(x => x.PackageId == packageId);
                    }
                    return query;
                });
            }
        }

        [Service("updateLeaveById")]
        public object updateLeaveById(Context request)
        {
            Validate(request, new
            {
                id = "required|string",
                start = "datetime",
                end = "datetime",
                title = "string",
                description = "string",
                status = ValidatorExtension.EnumValues<LeaveStatus>(),
                reason = "string",
            });

            var id = request.Value<string>("id");
            using (var db = GetDatabase)
            using (var trx = db.Database.BeginTransaction())
            {
                Leave leave = db.Leaves.Find(id);
                if (leave == null)
                {
                    throw Error.LeaveNotFound;
                }

                if (leave.Status == LeaveStatus.APPROVED)
                {
                    if (!request.IsNullOrEmpty("start") || !request.IsNullOrEmpty("end"))
                    {
                        throw Error.LeaveReadonlyAfterApprovedOrRejected;
                    }

                    if (!request.IsNullOrEmpty("status") && request.ObjectValue<LeaveStatus>("status") != LeaveStatus.REJECTED)
                    {
                        throw Error.LeaveReadonlyAfterApprovedOrRejected;
                    }
                }

                if (leave.Status == LeaveStatus.REJECTED)
                {
                    if (!request.IsNullOrEmpty("start") || !request.IsNullOrEmpty("end") || !request.IsNullOrEmpty("status"))
                    {
                        throw Error.LeaveReadonlyAfterApprovedOrRejected;
                    }
                }

                leave.Start = request.ValueWithFallback<DateTime>("start", leave.Start);
                leave.End = request.ValueWithFallback<DateTime>("end", leave.End);
                leave.Status = request.ValueWithFallback<LeaveStatus>("status", leave.Status);
                leave.Title = request.ValueWithFallback<string>("title", leave.Title);
                leave.Description = request.ValueWithFallback<string>("description", leave.Description);
                leave.Reason = request.ValueWithFallback<string>("reason", leave.Reason);
                db.SaveChanges();
                processLeave(request, db, leave);
                trx.Commit();
                return leave;
            }
        }

        private void processLeave(Context request, Database db, Leave leave)
        {
            if (leave.SpendAccountRecordId != null && leave.Status == LeaveStatus.REJECTED)
            {
                SpendAccountRecord record = db.SpendAccountRecords.Find(leave.SpendAccountRecordId);
                SpendAccount account = db.SpendAccounts.Find(record.SpendAccountId);
                if (account.Status == SpendAccountStatus.BUSY)
                {
                    throw new Exception("Resource is busy, please try again later.");
                }

                List<SpendAccountRecord> records = db.SpendAccountRecords.Where(x => x.RootSpendAccountRecordId == record.Id).ToList();
                foreach (SpendAccountRecord childRecord in records)
                {
                    childRecord.Status = SpendAccountRecordStatus.REFUNDED;
                }
                record.Status = SpendAccountRecordStatus.REFUNDED;
                account.Status = SpendAccountStatus.BUSY;
                db.SaveChanges();

                request.Background("processLeaveSpendAccount", (ctx) => processLeaveSpendAccount(ctx, account.Id));
            }

            if (leave.SpendAccountRecordId == null && leave.Status == LeaveStatus.APPROVED)
            {
                LeavePackage package = db.LeavePackages.Find(leave.PackageId);
                if (package == null)
                {
                    return;
                }

                SpendAccount account = db.SpendAccounts.Single((x) => x.EmployeeId == leave.EmployeeId && x.PackageId == package.Id);
                if (account.Status == SpendAccountStatus.BUSY)
                {
                    throw new Exception("Resource is busy, please try again later.");
                }

                if (account.ExpiryBalance > 0)
                {
                    int lastSequence = db.SpendAccountRecords.
                        OrderByDescending(x => x.Sequence).
                        Where(x => x.EmployeeId == leave.EmployeeId && x.PackageId == package.Id).
                        Select(x => x.Sequence).
                        FirstOrDefault();

                    SpendAccountRecord record = new SpendAccountRecord();
                    record.Id = GeneratorExtension.GenerateUNIQ();
                    record.Sequence = lastSequence + 1;
                    record.PackageType = SpendAccountRecordPackageType.LEAVE;
                    record.SpendAccountId = account.Id;
                    record.PackageId = package.Id;
                    record.EmployeeId = leave.EmployeeId;
                    record.SpendBalance = leave.getBalanceValue();
                    record.Status = SpendAccountRecordStatus.DEDUCT_EXPIRY;
                    record.CreatedDateTime = DateTime.Now;
                    record.UpdateDateTime = DateTime.Now;

                    if (leave.getBalanceValue() > account.ExpiryBalance)
                    {
                        record.SpendBalance = account.ExpiryBalance;

                        SpendAccountRecord recordExtra = new SpendAccountRecord();
                        recordExtra.Id = GeneratorExtension.GenerateUNIQ();
                        recordExtra.Sequence = lastSequence + 2;
                        recordExtra.PackageType = SpendAccountRecordPackageType.LEAVE;
                        recordExtra.SpendAccountId = account.Id;
                        recordExtra.PackageId = package.Id;
                        recordExtra.EmployeeId = leave.EmployeeId;
                        recordExtra.Status = SpendAccountRecordStatus.DEDUCTED;
                        recordExtra.SpendBalance = leave.getBalanceValue() - account.ExpiryBalance;
                        recordExtra.CreatedDateTime = DateTime.Now;
                        recordExtra.UpdateDateTime = DateTime.Now;
                        recordExtra.RootSpendAccountRecordId = record.Id;

                        var spendBalance = account.SpendBalance + leave.getBalanceValue() - account.ExpiryBalance;
                        if (spendBalance > account.Balance)
                        {
                            recordExtra.SpendBalance = account.Balance - account.SpendBalance;
                        }
                        else
                        {
                            recordExtra.SpendBalance = leave.getBalanceValue() - account.ExpiryBalance;
                        }
                        db.SpendAccountRecords.Add(recordExtra);
                    }

                    db.SpendAccountRecords.Add(record);
                    leave.SpendAccountRecordId = record.Id;
                    account.Status = SpendAccountStatus.BUSY;
                    db.SaveChanges();

                    request.Background("processLeaveSpendAccount", (ctx) => processLeaveSpendAccount(ctx, account.Id));
                }

                if (account.SpendBalance != account.Balance)
                {
                    int lastSequence = db.SpendAccountRecords.
                        OrderByDescending(x => x.Sequence).
                        Where(x => x.EmployeeId == leave.EmployeeId && x.PackageId == package.Id).
                        Select(x => x.Sequence).
                        FirstOrDefault();

                    SpendAccountRecord record = new SpendAccountRecord();
                    record.Id = GeneratorExtension.GenerateUNIQ();
                    record.Sequence = lastSequence + 1;
                    record.PackageType = SpendAccountRecordPackageType.LEAVE;
                    record.SpendAccountId = account.Id;
                    record.PackageId = package.Id;
                    record.EmployeeId = leave.EmployeeId;
                    record.Status = SpendAccountRecordStatus.DEDUCTED;
                    record.CreatedDateTime = DateTime.Now;
                    record.UpdateDateTime = DateTime.Now;

                    var spendBalance = account.SpendBalance + leave.getBalanceValue();
                    if (spendBalance > account.Balance)
                    {
                        record.SpendBalance = account.Balance - account.SpendBalance;
                    }
                    else
                    {
                        record.SpendBalance = leave.getBalanceValue();
                    }

                    db.SpendAccountRecords.Add(record);
                    leave.SpendAccountRecordId = record.Id;
                    account.Status = SpendAccountStatus.BUSY;
                    db.SaveChanges();

                    request.Background("processLeaveSpendAccount", (ctx) => processLeaveSpendAccount(ctx, account.Id));
                }
            }
        }

        public static void processLeaveSpendAccount(Context ctx, string accountId)
        {
            DateTime current = DateTime.Now;
            using (var db = GetDatabase)
            {
                SpendAccount account = db.SpendAccounts.Find(accountId);
                Employee employee = ctx.FindEmployeeById(account.EmployeeId);
                DateTime registerDate = employee.CreatedAt;
                TimeSpan diff = current.Subtract(registerDate);
                TimeSpan firstYearDiff = registerDate.Subtract(new DateTime(registerDate.Year + 1, 1, 1));
                int workedDays = (int)diff.TotalDays;
                int firstYearDays = (int)firstYearDiff.TotalDays;
                int workerCurrentYear = firstYearDays >= workedDays ? 1 : ((workedDays - firstYearDays) / 365) + 1;
                LeavePackage package = db.LeavePackages.Find(account.PackageId);
                if (package.IsProRated)
                {
                    var currentSpendBalance = db.SpendAccountRecords.AsEnumerable()
                        .Where(x => x.SpendAccountId == account.Id)
                        .Where(x => x.PackageId == package.Id)
                        .Where(x => x.PackageType == SpendAccountRecordPackageType.LEAVE)
                        .Where(x => x.Status == SpendAccountRecordStatus.DEDUCTED)
                        .Sum(x => x.SpendBalance);

                    double oneDay = 0;
                    switch (package.PeriodType)
                    {
                        case LeavePackagePeriod.YEARLY:
                            oneDay = package.PeriodValue / (double)365;
                            break;
                        case LeavePackagePeriod.MONTHLY:
                            oneDay = package.PeriodValue / (double)30;
                            break;
                        default:
                            break;
                    }
                    account.Balance = (int)Math.Floor(oneDay * workedDays);
                    account.SpendBalance = currentSpendBalance;
                    account.ExpiryBalance = 0;
                }
                else
                {
                    var currentSpendBalance = db.SpendAccountRecords.AsEnumerable()
                        .Where(x => x.SpendAccountId == account.Id)
                        .Where(x => x.PackageId == package.Id)
                        .Where(x => x.PackageType == SpendAccountRecordPackageType.LEAVE)
                        .Where(x => x.CreatedDateTime >= new DateTime(current.Year, 1, 1))
                        .Where(x => x.Status != SpendAccountRecordStatus.DEDUCT_EXPIRY && x.Status != SpendAccountRecordStatus.REFUNDED)
                        .OrderBy(x => x.CreatedDateTime)
                        .Sum(x => x.SpendBalance);

                    account.Balance = package.Value;
                    account.SpendBalance = currentSpendBalance;
                    account.ExpiryBalance = 0;
                    if (workerCurrentYear > 1 && package.IsCarryForward)
                    {
                        DateTime maxCarryDateTime;
                        switch (package.CarryForwardPeriod)
                        {
                            case LeavePackageCarryForwardPeriod.DAY:
                                maxCarryDateTime = new DateTime(current.Year, 1, package.CarryForwardPeriodValue);
                                break;
                            case LeavePackageCarryForwardPeriod.MONTH:
                                maxCarryDateTime = new DateTime(current.Year, package.CarryForwardPeriodValue, DateTime.DaysInMonth(current.Year, package.CarryForwardPeriodValue));
                                break;
                            default:
                                maxCarryDateTime = new DateTime(current.Year, 1, 1);
                                break;
                        }

                        if (maxCarryDateTime > current)
                        {
                            var lastYearSpend = db.SpendAccountRecords
                                .Where(x => x.SpendAccountId == account.Id)
                                .Where(x => x.PackageId == package.Id)
                                .Where(x => x.PackageType == SpendAccountRecordPackageType.LEAVE)
                                .Where(x => x.CreatedDateTime >= new DateTime(current.Year - 1, 1, 1) && x.CreatedDateTime <= new DateTime(current.Year - 1, 12, 31))
                                .Where(x => x.Status == SpendAccountRecordStatus.DEDUCTED)
                                .Sum(x => x.SpendBalance);
                            if (package.PeriodValue - lastYearSpend > 0)
                            {
                                int carryForward = package.PeriodValue - lastYearSpend;
                                if (carryForward > package.CarryForwardValue)
                                {
                                    carryForward = package.CarryForwardValue;
                                }

                                var thisYearSpendCarried = db.SpendAccountRecords
                                    .Where(x => x.SpendAccountId == account.Id)
                                    .Where(x => x.PackageId == package.Id)
                                    .Where(x => x.PackageType == SpendAccountRecordPackageType.LEAVE)
                                    .Where(x => x.CreatedDateTime >= new DateTime(current.Year, 1, 1) && x.CreatedDateTime <= new DateTime(current.Year, 12, 31))
                                    .Where(x => x.Status == SpendAccountRecordStatus.DEDUCT_EXPIRY)
                                    .Sum(x => x.SpendBalance);
                                account.ExpiryBalance = carryForward - thisYearSpendCarried;
                            }
                        }
                    }
                }
                account.Status = SpendAccountStatus.FREE;
                account.UpdatedDateTime = DateTime.UtcNow;
                db.SaveChanges();
            }
        }
    }
}