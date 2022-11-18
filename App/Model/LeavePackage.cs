using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace amantiq.model
{
    [Table("leave_packages")]
    public class LeavePackage
    {
        [Key]
        [Column(TypeName = "varchar(20)")]
        public string Id { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string CompanyId { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public int Value { get; set; }

        public LeavePackagePeriod PeriodType { get; set; }

        public int PeriodValue { get; set; }

        public bool IsProRated { get; set; }

        public bool IsCarryForward { get; set; }

        public LeavePackageCarryForwardPeriod CarryForwardPeriod { get; set; }

        public int CarryForwardPeriodValue { get; set; }

        public int CarryForwardValue { get; set; }
    }

    public enum LeavePackageCarryForwardPeriod
    {
        DAY,
        MONTH
    }

    public enum LeavePackagePeriod
    {
        MONTHLY,
        YEARLY
    }
}