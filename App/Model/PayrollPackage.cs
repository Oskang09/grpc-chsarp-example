using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace amantiq.model
{
    [Table("payroll_packages")]
    public class PayrollPackage
    {
        [Key]
        [Column(TypeName = "varchar(20)")]
        public string Id { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string CompanyId { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string EmployeeId { get; set; }

        public int Salary { get; set; }

        [Column(TypeName = "jsonb")]
        public List<string> Benefits { get; set; }

        [Column(TypeName = "jsonb")]
        public List<string> Leaves { get; set; }

        [Column(TypeName = "jsonb")]
        public List<PayrollPackageEarning> Earnings { get; set; }

        [Column(TypeName = "jsonb")]
        public List<PayrollPackageDeduction> Deductions { get; set; }

        public PayrollPackageStatus Status { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public DateTime UpdatedDateTime { get; set; }
    }

    public enum PayrollPackageStatus
    {
        ACTIVE,
        EXPIRED
    }

    public class PayrollPackageDeduction
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }
    public class PayrollPackageEarning
    {
        public string Name { get; set; }
        public int Value { get; set; }
        public bool IsTaxable { get; set; }
    }
}