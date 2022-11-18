using System;
using amantiq.model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging;

namespace amantiq
{
    public partial class Database : DbContext
    {
        public DbSet<Company> Companies { get; set; }
        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<Leave> Leaves { get; set; }
        public DbSet<LeavePackage> LeavePackages { get; set; }
        public DbSet<Benefit> Benefits { get; set; }
        public DbSet<BenefitPackage> BenefitPackages { get; set; }
        public DbSet<SpendAccount> SpendAccounts { get; set; }
        public DbSet<SpendAccountRecord> SpendAccountRecords { get; set; }
        public DbSet<PayrollPackage> PayrollPackages { get; set; }
        public DbSet<Payroll> Payrolls { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            base.OnConfiguring(options);

            constant.Environment.Initialize();
            options.UseNpgsql(constant.Environment.CONNECTION_STRING);
            if (amantiq.constant.Environment.IsDevelopment)
            {
                options.UseLoggerFactory(LoggerFactory.Create((builder) => builder.AddConsole()));
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType.BaseType == typeof(Enum))
                    {
                        var type = typeof(EnumToStringConverter<>).MakeGenericType(property.ClrType);
                        var converter = Activator.CreateInstance(type, new ConverterMappingHints()) as ValueConverter;
                        property.SetValueConverter(converter);
                    }
                }
            }
        }
    }
}