using System;
using System.Collections.Generic;
using System.Linq;
using amantiq.constant;
using amantiq.model;
using amantiq.util;

namespace amantiq.service
{
    public class BenefitController : Controller
    {
        [Service("createBenefitPackage")]
        public object createBenefitPackage(Context request)
        {
            Validate(request, new
            {
                name = "required|string",
                valueType = "required|" + ValidatorExtension.EnumValues<BenefitPackageValueType>(),
                value = "required|numeric",
                periodType = "required|" + ValidatorExtension.EnumValues<BenefitPackagePeriodType>(),
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
                    carryForwardPeriod = "required|" + ValidatorExtension.EnumValues<BenefitPackageCarryForwardPeriod>(),
                    carryForwardPeriodValue = "required|integer",
                    carryForwardValue = "required|integer",
                });
            }

            BenefitPackage package = new BenefitPackage();
            package.Id = GeneratorExtension.GenerateUNIQ();
            package.Name = request.Value<string>("name");
            package.ValueType = request.ObjectValue<BenefitPackageValueType>("valueType");
            package.Value = request.Value<int>("value");
            package.PeriodType = request.ObjectValue<BenefitPackagePeriodType>("periodType");
            package.PeriodValue = request.Value<int>("periodValue");
            package.IsProRated = request.Value<bool>("isProRated");
            package.IsCarryForward = request.Value<bool>("isCarryForward");
            package.CarryForwardPeriod = request.ValueWithFallback<BenefitPackageCarryForwardPeriod>("carryForwardPeriod", BenefitPackageCarryForwardPeriod.DAY);
            package.CarryForwardPeriodValue = request.ValueWithFallback<int>("carryForwardPeriodValue", 0);
            package.CarryForwardValue = request.ValueWithFallback<int>("carryForwardValue", 0);
            package.CompanyId = request.Value<string>("companyId");
            using (var db = GetDatabase)
            {
                db.BenefitPackages.Add(package);
                db.SaveChanges();
            }
            return package;
        }

        [Service("findBenefitPackages")]
        public object findBenefitPackages(Context request)
        {
            Validate(request, new
            {
                name = "string",
                companyId = "string",
            });

            using (var db = GetDatabase)
            {
                return db.BenefitPackages.Paginate(50, 1, (query) =>
                {
                    string companyId = request.ValueWithFallback("companyId", "");
                    if (companyId != "")
                    {
                        query = query.Where(x => x.CompanyId == companyId);
                    }

                    string name = request.ValueWithFallback("name", "");
                    if (name != "")
                    {
                        query = query.Where(x => x.Name.Contains(name));
                    }
                    return query;
                });
            }
        }

        [Service("findBenefitPackageById")]
        public object findBenefitPackageById(Context request)
        {
            Validate(request, new
            {
                id = "required|string",
            });

            var id = request.Value<string>("id");
            using (var db = GetDatabase)
            {
                BenefitPackage package = db.BenefitPackages.Find(id);
                if (package == null)
                {
                    request.status = 404;
                    throw Error.BenefitNotFound;
                }
                return package;
            }
        }

        [Service("updateBenefitPackageById")]
        public object updateBenefitPackageById(Context request)
        {
            Validate(request, new
            {
                id = "required|string",
                name = "string",
                type = "string",
                value = "numeric",
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
                BenefitPackage package = db.BenefitPackages.Find(id);
                package.Name = request.ValueWithFallback<string>("name", package.Name);
                package.ValueType = request.ValueWithFallback<BenefitPackageValueType>("type", package.ValueType);
                package.Value = (int)request.ValueWithFallback<double>("value", package.Value);
                package.PeriodType = request.ValueWithFallback<BenefitPackagePeriodType>("periodType", package.PeriodType);
                package.PeriodValue = request.ValueWithFallback<int>("periodValue", package.PeriodValue);
                package.IsProRated = request.ValueWithFallback<bool>("isProRated", package.IsProRated);
                package.IsCarryForward = request.ValueWithFallback<bool>("isCarryForward", package.IsCarryForward);
                package.CarryForwardPeriod = request.ValueWithFallback<BenefitPackageCarryForwardPeriod>("carryForwardPeriod", package.CarryForwardPeriod);
                package.CarryForwardPeriodValue = request.ValueWithFallback<int>("carryForwardPeriodValue", package.CarryForwardPeriodValue);
                package.CarryForwardValue = request.ValueWithFallback<int>("carryForwardValue", package.CarryForwardValue);
                package.CompanyId = request.ValueWithFallback<string>("companyId", package.CompanyId);
                db.SaveChanges();
                return package;
            }
        }

        [Service("removeBenefitPackageById")]
        public object removeBenefitPackageById(Context request)
        {
            Validate(request, new
            {
                id = "required|string",
            });

            var id = request.Value<string>("id");
            using (var db = GetDatabase)
            {
                BenefitPackage package = db.BenefitPackages.Find(id);
                db.BenefitPackages.Remove(package);
                db.SaveChanges();
                return package;
            }
        }

        [Service("createBenefit")]
        public object createBenefit(Context request)
        {
            Validate(request, new
            {
                title = "string",
                description = "string",
                value = "required|numeric",
                status = ValidatorExtension.EnumValues<BenefitStatus>(),
                isTaxable = "required|boolean",
                packageId = "required|string",
                employeeId = "required|string",
                companyId = "required|string",
            });

            Benefit benefit = new Benefit();
            benefit.Id = GeneratorExtension.GenerateUNIQ();
            benefit.Title = request.Value<string>("title");
            benefit.Description = request.Value<string>("description");
            benefit.Value = request.Value<int>("value");
            benefit.Status = request.ValueWithFallback<BenefitStatus>("status", BenefitStatus.PENDING);
            benefit.IsTaxable = request.Value<bool>("isTaxable");
            benefit.PackageId = request.Value<string>("packageId");
            benefit.EmployeeId = request.Value<string>("employeeId");
            benefit.CompanyId = request.Value<string>("companyId");
            using (var db = GetDatabase)
            using (var trx = db.Database.BeginTransaction())
            {
                db.Benefits.Add(benefit);
                db.SaveChanges();
                processBenefit(request, db, benefit);
                trx.Commit();
            }
            return benefit;
        }

        [Service("findBenefits")]
        public object findBenefits(Context request)
        {
            Validate(request, new
            {
                companyId = "string",
                employeeId = "string",
                packageId = "string",
            });

            using (var db = GetDatabase)
            {
                return db.Benefits.Paginate(50, 1, (query) =>
                {
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

        [Service("updateBenefitById")]
        public object updateBenefitById(Context request)
        {
            Validate(request, new
            {
                id = "required|string",
                title = "string",
                description = "string",
                value = "numeric",
                status = ValidatorExtension.EnumValues<BenefitStatus>(),
                isTaxable = "boolean",
                reason = "string",
            });

            var id = request.Value<string>("id");
            using (var db = GetDatabase)
            using (var trx = db.Database.BeginTransaction())
            {
                Benefit benefit = db.Benefits.Find(id);
                if (benefit == null)
                {
                    throw Error.BenefitNotFound;
                }

                if (benefit.Status == BenefitStatus.APPROVED)
                {
                    if (!request.IsNullOrEmpty("value"))
                    {
                        throw Error.BenefitReadonlyAfterApprovedOrRejected;
                    }

                    if (!request.IsNullOrEmpty("status") && request.ObjectValue<LeaveStatus>("status") != LeaveStatus.REJECTED)
                    {
                        throw Error.BenefitReadonlyAfterApprovedOrRejected;
                    }
                }

                if (benefit.Status == BenefitStatus.REJECTED)
                {
                    if (!request.IsNullOrEmpty("value") || !request.IsNullOrEmpty("status"))
                    {
                        throw Error.BenefitReadonlyAfterApprovedOrRejected;
                    }
                }
                benefit.Title = request.ValueWithFallback<string>("title", benefit.Title);
                benefit.Description = request.ValueWithFallback<string>("description", benefit.Description);
                benefit.Value = request.ValueWithFallback<int>("value", benefit.Value);
                benefit.Status = request.ValueWithFallback<BenefitStatus>("status", benefit.Status);
                benefit.IsTaxable = request.ValueWithFallback<bool>("isTaxable", benefit.IsTaxable);
                benefit.Reason = request.ValueWithFallback<string>("reason", benefit.Reason);

                db.SaveChanges();
                processBenefit(request, db, benefit);
                trx.Commit();
                return benefit;
            }
        }

        private void processBenefit(Context request, Database db, Benefit benefit)
        {
            if (benefit.SpendAccountRecordId != null && benefit.Status == BenefitStatus.REJECTED)
            {
                SpendAccountRecord record = db.SpendAccountRecords.Find(benefit.SpendAccountRecordId);
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

                request.Background("processBenefitSpendAccount", (ctx) => processBenefitSpendAccount(ctx, account.Id));
            }

            if (benefit.SpendAccountRecordId == null && benefit.Status == BenefitStatus.APPROVED)
            {
                BenefitPackage package = db.BenefitPackages.Find(benefit.PackageId);
                if (package == null)
                {
                    return;
                }

                SpendAccount account = db.SpendAccounts.Single((x) => x.EmployeeId == benefit.EmployeeId && x.PackageId == benefit.PackageId);
                if (account.Status == SpendAccountStatus.BUSY)
                {
                    throw new Exception("Resource is busy, please try again later.");
                }

                if (account.ExpiryBalance > 0)
                {
                    int lastSequence = db.SpendAccountRecords.
                        OrderByDescending(x => x.Sequence).
                        Where(x => x.EmployeeId == benefit.EmployeeId && x.PackageId == package.Id).
                        Select(x => x.Sequence).
                        FirstOrDefault();

                    SpendAccountRecord record = new SpendAccountRecord();
                    record.Id = GeneratorExtension.GenerateUNIQ();
                    record.Sequence = lastSequence + 1;
                    record.PackageType = SpendAccountRecordPackageType.BENEFIT;
                    record.SpendAccountId = account.Id;
                    record.PackageId = package.Id;
                    record.EmployeeId = benefit.EmployeeId;
                    record.SpendBalance = benefit.Value;
                    record.Status = SpendAccountRecordStatus.DEDUCT_EXPIRY;
                    record.CreatedDateTime = DateTime.Now;
                    record.UpdateDateTime = DateTime.Now;

                    if (benefit.Value > account.ExpiryBalance)
                    {
                        record.SpendBalance = account.ExpiryBalance;

                        SpendAccountRecord recordExtra = new SpendAccountRecord();
                        recordExtra.Id = GeneratorExtension.GenerateUNIQ();
                        recordExtra.Sequence = lastSequence + 2;
                        recordExtra.PackageType = SpendAccountRecordPackageType.BENEFIT;
                        recordExtra.SpendAccountId = account.Id;
                        recordExtra.PackageId = package.Id;
                        recordExtra.EmployeeId = benefit.EmployeeId;
                        recordExtra.Status = SpendAccountRecordStatus.DEDUCTED;
                        recordExtra.SpendBalance = benefit.Value - account.ExpiryBalance;
                        recordExtra.CreatedDateTime = DateTime.Now;
                        recordExtra.UpdateDateTime = DateTime.Now;
                        recordExtra.RootSpendAccountRecordId = record.Id;

                        var spendBalance = account.SpendBalance + benefit.Value - account.ExpiryBalance;
                        if (spendBalance > account.Balance)
                        {
                            recordExtra.SpendBalance = account.Balance - account.SpendBalance;
                        }
                        else
                        {
                            recordExtra.SpendBalance = benefit.Value - account.ExpiryBalance;
                        }
                        db.SpendAccountRecords.Add(recordExtra);
                    }

                    db.SpendAccountRecords.Add(record);
                    benefit.SpendAccountRecordId = record.Id;
                    account.Status = SpendAccountStatus.BUSY;
                    db.SaveChanges();

                    request.Background("processBenefitSpendAccount", (ctx) => processBenefitSpendAccount(ctx, account.Id));
                }

                if (account.SpendBalance != account.Balance)
                {
                    int lastSequence = db.SpendAccountRecords.
                        OrderByDescending(x => x.Sequence).
                        Where(x => x.EmployeeId == benefit.EmployeeId && x.PackageId == package.Id).
                        Select(x => x.Sequence).
                        FirstOrDefault();

                    SpendAccountRecord record = new SpendAccountRecord();
                    record.Id = GeneratorExtension.GenerateUNIQ();
                    record.Sequence = lastSequence + 1;
                    record.PackageType = SpendAccountRecordPackageType.BENEFIT;
                    record.SpendAccountId = account.Id;
                    record.PackageId = package.Id;
                    record.EmployeeId = benefit.EmployeeId;
                    record.Status = SpendAccountRecordStatus.DEDUCTED;
                    record.CreatedDateTime = DateTime.Now;
                    record.UpdateDateTime = DateTime.Now;

                    var spendBalance = account.SpendBalance + benefit.Value;
                    if (spendBalance > account.Balance)
                    {
                        record.SpendBalance = account.Balance - account.SpendBalance;
                    }
                    else
                    {
                        record.SpendBalance = benefit.Value;
                    }

                    db.SpendAccountRecords.Add(record);
                    benefit.SpendAccountRecordId = record.Id;
                    account.Status = SpendAccountStatus.BUSY;
                    db.SaveChanges();

                    request.Background("processBenefitSpendAccount", (ctx) => processBenefitSpendAccount(ctx, account.Id));
                }
            }
        }

        public static void processBenefitSpendAccount(Context ctx, string accountId)
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

                BenefitPackage package = db.BenefitPackages.Find(account.PackageId);
                if (package.IsProRated)
                {
                    var currentSpendBalance = db.SpendAccountRecords.AsEnumerable()
                        .Where(x => x.SpendAccountId == account.Id)
                        .Where(x => x.PackageId == package.Id)
                        .Where(x => x.PackageType == SpendAccountRecordPackageType.BENEFIT)
                        .Where(x => x.Status == SpendAccountRecordStatus.DEDUCTED)
                        .Sum(x => x.SpendBalance);

                    double oneDay = 0;
                    switch (package.PeriodType)
                    {
                        case BenefitPackagePeriodType.YEARLY:
                            oneDay = package.PeriodValue / (double)365;
                            break;
                        case BenefitPackagePeriodType.MONTHLY:
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
                        .Where(x => x.PackageType == SpendAccountRecordPackageType.BENEFIT)
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
                            case BenefitPackageCarryForwardPeriod.DAY:
                                maxCarryDateTime = new DateTime(current.Year, 1, package.CarryForwardPeriodValue);
                                break;
                            case BenefitPackageCarryForwardPeriod.MONTH:
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
                                .Where(x => x.PackageType == SpendAccountRecordPackageType.BENEFIT)
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
                                    .Where(x => x.PackageType == SpendAccountRecordPackageType.BENEFIT)
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