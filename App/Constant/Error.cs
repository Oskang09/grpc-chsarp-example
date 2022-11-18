using System;
using System.Collections.Generic;

namespace amantiq.constant
{
    public class Error
    {
        public static Exception LeaveNotFound = new Exception("leave.not_found");
        public static Exception LeaveReadonlyAfterApprovedOrRejected = new Exception("leave.readonly_after_approved_or_rejected");
        public static Exception BenefitNotFound = new Exception("benefit.not_found");
        public static Exception BenefitReadonlyAfterApprovedOrRejected = new Exception("benefit.readonly_after_approved_or_rejected");
        public static Exception EmployeeNoActivePayroll = new Exception("employee.no_active_payroll");
        public static Exception PayrollPackageNotFound = new Exception("payroll_package.not_found");
        public static Exception PayrollNotFound = new Exception("payroll.not_found");

        public static Dictionary<Exception, string> Mapper = new Dictionary<Exception, string>
        {
            [LeaveNotFound] = "Leave doesn't exists",
            [BenefitNotFound] = "Benefit doesn't exists",
            [LeaveReadonlyAfterApprovedOrRejected] = "Leave unable to update after approved or rejected",
            [BenefitReadonlyAfterApprovedOrRejected] = "Benefit unable to update after approved or rejected",
            [EmployeeNoActivePayroll] = "Employee doens't having an active payroll package",
            [PayrollPackageNotFound] = "Payroll package doesn't exists",
            [PayrollNotFound] = "Payroll doesn't exists",
        };
    }
}