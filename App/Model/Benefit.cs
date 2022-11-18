using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace amantiq.model
{
    [Table("benefits")]
    public class Benefit
    {
        [Key]
        [Column(TypeName = "varchar(20)")]
        public string Id { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string SpendAccountRecordId { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string CompanyId { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string EmployeeId { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string PackageId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public BenefitStatus Status { get; set; }

        public string Reason { get; set; }

        public int Value { get; set; }

        public bool IsTaxable { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public DateTime UpdateDateTime { get; set; }
    }

    public enum BenefitStatus
    {
        PENDING,
        APPROVED,
        REJECTED
    }
}