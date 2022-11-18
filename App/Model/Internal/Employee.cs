
using System;
using System.Collections.Generic;
using amamtiq.util;
using Newtonsoft.Json;

namespace amantiq.model
{
    public class Employee
    {
        public string Id { get; set; }

        public string CompanyId { get; set; }

        public string RoleId { get; set; }

        public string EmployeeNo { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Type { get; set; }

        public int Age { get; set; }

        public List<string> Permissions { get; set; }

        [JsonProperty("dob")]
        public DateTime DateOfBirth { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public EmployeeContact Contact { get; set; }

        public EmployeeDesignation Designation { get; set; }

        public EmployeeFamily Family { get; set; }

        public EmployeeFinancial Financial { get; set; }

        public EmployeeNationality Nationality { get; set; }

        public EmployeeStatutory Statutory { get; set; }

        public EmployeeWorking Working { get; set; }
    }

    public class EmployeeContact
    {
        public List<EmployeeContactAddress> Address { get; set; }
        public List<EmployeeContactEmail> Email { get; set; }
        public List<EmployeeContactEmergency> Emergency { get; set; }
        public List<EmployeeContactPhoneNumber> PhoneNumber { get; set; }
    }

    public class EmployeeContactAddress
    {
        public string Type { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Line3 { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PostCode { get; set; }
        public string State { get; set; }
    }

    public class EmployeeContactEmail
    {
        public string Type { get; set; }
        public string Value { get; set; }
    }

    public class EmployeeContactEmergency
    {
        public string Relation { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
    }

    public class EmployeeContactPhoneNumber
    {
        public string Type { get; set; }
        public string Value { get; set; }
    }

    public class EmployeeDesignation
    {
        public string Branch { get; set; }
        public string Department { get; set; }
        public int Grade { get; set; }
        public string Title { get; set; }
    }

    public class EmployeeFamily
    {
        public string MartialStatus { get; set; }
        public List<EmployeeFamilyChild> Children { get; set; }
        public List<EmployeeFamilyDependent> Dependent { get; set; }
        public List<EmployeeFamilySpouse> Spouse { get; set; }
    }

    public class EmployeeFamilyChild
    {
        [JsonProperty("dob")]
        public DateTime DateOfBirth { get; set; }
        public string Name { get; set; }
        public string Nationality { get; set; }
        public string NRIC { get; set; }
        public bool Studying { get; set; }
    }

    public class EmployeeFamilyDependent
    {
        [JsonProperty("dob")]
        public DateTime DateOfBirth { get; set; }
        public string Name { get; set; }
        public string Nationality { get; set; }
        public string NRIC { get; set; }
    }

    public class EmployeeFamilySpouse
    {
        [JsonProperty("dob")]
        public DateTime DateOfBirth { get; set; }
        public string Name { get; set; }
        public string Nationality { get; set; }
        public string NRIC { get; set; }
        public bool Working { get; set; }
    }

    public class EmployeeFinancial
    {
        public string BankAccount { get; set; }
        public string BankAccountName { get; set; }
        public string BankName { get; set; }
        public string BankType { get; set; }
        public string InsuranceAccount { get; set; }
        public string InsuranceName { get; set; }
        public string InsuranceType { get; set; }
    }

    public class EmployeeNationality
    {
        public bool Bumiputera { get; set; }
        public string IdentityNo { get; set; }
        public string Nationality { get; set; }
        public string Religion { get; set; }
    }

    public class EmployeeStatutory
    {
        public EmployeeStatutoryEpf Epf { get; set; }
        public EmployeeStatutorySocso Socso { get; set; }
        public EmployeeStatutoryEis Eis { get; set; }
        public EmployeeStatutoryTax Tax { get; set; }
        public EmployeeStatutoryZakat Zakat { get; set; }
    }

    public class EmployeeStatutoryEpf
    {
        public string AccountNo { get; set; }
        public bool IsMalaysia { get; set; }
        public int Rate { get; set; }
    }

    public class EmployeeStatutoryEis
    {
        public string AccountNo { get; set; }
    }

    public class EmployeeStatutorySocso
    {
        public string AccountNo { get; set; }
    }

    public class EmployeeStatutoryTax
    {
        public string AccountNo { get; set; }
        public bool KnowledgeWorker { get; set; }
        public bool Rep { get; set; }
        public bool TaxResidence { get; set; }
    }

    public class EmployeeStatutoryZakat
    {
        public string Authority { get; set; }
        public string Type { get; set; }
        public int Value { get; set; }
    }

    public class EmployeeWorking
    {
        public List<DateTime> Friday { get; set; }
        public List<DateTime> Monday { get; set; }
        public List<DateTime> Saturday { get; set; }
        public List<DateTime> Sunday { get; set; }
        public List<DateTime> Thrusday { get; set; }
        public List<DateTime> Tuesday { get; set; }
        public List<DateTime> Wednesday { get; set; }
    }

    public static class EmployeeExtension
    {
        public static Employee FindEmployeeById(this Context context, string id)
        {
            return context.InternalService<Employee>("user-service", "internal.findEmployeeById", new { id = id });
        }
    }
}