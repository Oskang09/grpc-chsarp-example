using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace amantiq.model
{
    [Table("dynamic_earning_package")]
    public class DynamicEarningPackage
    {
        [Key]
        [Column(TypeName = "varchar(20)")]
        public string Id { get; set; }

        public bool IsApplyAll { get; set; }

        /*
            If `IsApplyAll` 
            true   => Blacklisted employeeIds
            false  => Whitelisted employeeIds
        */
        public List<string> Criteria { get; set; }

        public string Name { get; set; }

        public int Value { get; set; }

        public string Rules { get; set; }

        public DynamicEarningPackageSatutory Satutory { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public DateTime ExpiredDateTime { get; set; }
    }

    public class DynamicEarningPackageSatutory
    {
        public bool Epf { get; set; }
        public bool Socso { get; set; }
        public bool Eis { get; set; }
        public bool Tax { get; set; }
    }
}