using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace amantiq.model
{
    [Table("payrolls")]
    public class Payroll
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }

        public DateTime Period { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string PackageId { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string EmployeeId { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string CompanyId { get; set; }

        public string Company { get; set; }

        public string Date { get; set; }

        public string PayPeriod { get; set; }

        public int EmployeeSalary { get; set; }

        public string EmployeeName { get; set; }

        public string EmployeeNRIC { get; set; }

        public string EmployeeIncomeTaxNo { get; set; }

        public string EmployeeEpfNo { get; set; }

        public string EmployeeSocsoNo { get; set; }

        public string EmployeeEisNo { get; set; }

        public int EmployeeEpf { get; set; }

        public int EmployeeSocso { get; set; }

        public int EmployeeZakat { get; set; }

        public int EmployeeEis { get; set; }

        public int EmployeeTax { get; set; }

        public int EmployerEpf { get; set; }

        public int EmployerSocso { get; set; }

        public int EmployerEis { get; set; }


        [Column(TypeName = "jsonb")]
        public List<PayrollDeduction> Deductions { get; set; }

        [Column(TypeName = "jsonb")]
        public List<PayrollEarning> Earnings { get; set; }

        public int TotalBenefits { get; set; }

        public int TotalTaxableBenefits { get; set; }

        public int TotalEarnings { get; set; }

        public int TotalTaxableEarnings { get; set; }

        public int TotalDeductions { get; set; }

        public int NetPay { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public DateTime UpdatedDateTime { get; set; }
    }

    public class PayrollEarning
    {
        public string Name { get; set; }
        public int Value { get; set; }
        public bool IsTaxable { get; set; }
    }

    public class PayrollDeduction
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }
}