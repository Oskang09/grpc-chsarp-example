using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace amantiq.model
{
    [Table("companies")]
    public class Company
    {
        [Key]
        [Column(TypeName = "varchar(20)")]
        public string Id { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string BranchOf { get; set; }

        public string Name { get; set; }

        public string RegistrationNumber { get; set; }

        public List<string> Holidays { get; set; }

        public CompanyStatus Status { get; set; }
    }

    public enum CompanyStatus
    {
        ACTIVE,
        INACTIVE
    }
}