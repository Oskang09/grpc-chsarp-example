using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace amantiq.model
{
    [Table("benefit_packages")]
    public class BenefitPackage
    {
        [Key]
        [Column(TypeName = "varchar(20)")]
        public string Id { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string CompanyId { get; set; }

        public string Name { get; set; }

        public BenefitPackageValueType ValueType { get; set; }

        public int Value { get; set; }

        public BenefitPackagePeriodType PeriodType { get; set; }

        public int PeriodValue { get; set; }

        public bool IsProRated { get; set; }

        public bool IsCarryForward { get; set; }

        public BenefitPackageCarryForwardPeriod CarryForwardPeriod { get; set; }

        public int CarryForwardPeriodValue { get; set; }

        public int CarryForwardValue { get; set; }
    }

    public enum BenefitPackageCarryForwardPeriod
    {
        DAY,
        MONTH
    }

    public enum BenefitPackageValueType
    {
        CURRENCY,
        TIMES
    }

    public enum BenefitPackagePeriodType
    {
        MONTHLY,
        YEARLY
    }

}