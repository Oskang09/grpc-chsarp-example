using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using amantiq.util;

namespace amantiq.model
{
    [Table("leaves")]
    public class Leave
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

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public LeaveStatus Status { get; set; }

        public string Reason { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public DateTime UpdateDateTime { get; set; }

        public bool IsOverMonth()
        {
            return this.Start.Month != this.End.Month;
        }

        public bool IsForwardMonth(DateTime date)
        {
            return this.Start.Month == date.Month;
        }

        public bool IsBackwardMonth(DateTime date)
        {
            return this.End.Month == date.Month;
        }
        public int getBalanceValue(DateTime dateTime = default(DateTime))
        {
            if (dateTime == default(DateTime))
            {
                int diff = (this.End - this.Start).Days + 1;
                return diff == 0 ? 50 : 100 * diff;
            }

            if (this.Start.Month == dateTime.Month)
            {
                DateTime end = dateTime.LastDayOfMonth();
                if (this.End.Month == dateTime.Month)
                {
                    end = this.End;
                }

                int diff = (this.Start - end).Days + 1;
                return diff * 100;
            }

            if (this.End.Month == dateTime.Month)
            {
                DateTime start = dateTime.FirstDayOfMonth();
                if (this.Start.Month == dateTime.Month)
                {
                    start = this.Start;
                }

                int diff = (start - this.End).Days + 1;
                return diff * 100;
            }
            return 0;
        }
    }

    public enum LeaveStatus
    {
        PENDING,
        APPROVED,
        REJECTED
    }

}