using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace amantiq.model
{
    [Table("spend_account_records")]
    public class SpendAccountRecord
    {
        [Key]
        [Column(TypeName = "varchar(20)")]
        public string Id { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string SpendAccountId { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string PackageId { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string EmployeeId { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string RootSpendAccountRecordId { get; set; }

        public int Sequence { get; set; }

        public SpendAccountRecordPackageType PackageType { get; set; }

        public int SpendBalance { get; set; }

        public SpendAccountRecordStatus Status { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public DateTime UpdateDateTime { get; set; }
    }

    public enum SpendAccountRecordStatus
    {
        DEDUCTED,
        DEDUCT_EXPIRY,
        REFUNDED
    }

    public enum SpendAccountRecordPackageType
    {
        LEAVE,
        BENEFIT
    }
}