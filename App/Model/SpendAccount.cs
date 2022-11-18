using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using amantiq.util;

namespace amantiq.model
{
    [Table("spend_account")]
    public class SpendAccount
    {
        [Key]
        [Column(TypeName = "varchar(20)")]
        public string Id { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string PackageId { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string EmployeeId { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string CompanyId { get; set; }

        public SpendAccountPackageType PackageType { get; set; }

        public SpendAccountValueType ValueType { get; set; }

        public int Balance { get; set; }

        public int SpendBalance { get; set; }

        public int ExpiryBalance { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public DateTime UpdatedDateTime { get; set; }

        public SpendAccountStatus Status { get; set; }
    }

    public enum SpendAccountStatus
    {
        FREE,
        BUSY,
    }

    public enum SpendAccountPackageType
    {
        LEAVE,
        BENEFIT
    }

    public enum SpendAccountValueType
    {
        CURRENCY,
        TIMES
    }

}