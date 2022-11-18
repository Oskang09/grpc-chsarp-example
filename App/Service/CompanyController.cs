using System;
using System.Linq;
using amantiq.model;
using amantiq.util;

namespace amantiq.service
{
    public class CompanyController : Controller
    {
        [Service("createCompany")]
        public object createCompany(Context request)
        {
            Validate(request, new
            {
                name = "required|string",
                registrationNumber = "required|string"
            });

            Company company = new Company();
            company.Id = GeneratorExtension.GenerateUNIQ();
            company.Name = request.Value<string>("name");
            company.RegistrationNumber = request.Value<string>("regNo");
            company.Status = CompanyStatus.ACTIVE;
            using (var db = GetDatabase)
            {
                db.Companies.Add(company);
                db.SaveChanges();
            }
            return company;
        }

        [Service("findCompanyById")]
        public object findCompanyById(Context request)
        {
            Validate(request, new
            {
                id = "required|string",
            });

            string id = request.Value<string>("id");
            using (var db = GetDatabase)
            {
                Company company = db.Companies.Find(id);
                if (company == null)
                {
                    throw new Exception("Company not found");
                }
                return company;
            }
        }


        [Service("findCompanies")]
        public object findCompanies(Context request)
        {
            Validate(request, new
            {
                limit = "integer",
                page = "integer",
                name = "string",
            });


            using (var db = GetDatabase)
            {
                return db.Companies.Paginate(request.Value<int>("limit"), request.Value<int>("page"), (query) =>
                {
                    string name = request.ValueWithFallback("name", "");
                    if (name != "")
                    {
                        query = query.Where(x => x.Name.Contains(name));
                    }
                    return query;
                });
            }
        }

        [Service("updateCompanyById")]
        public object updateCompanyById(Context request)
        {
            Console.WriteLine(ValidatorExtension.EnumValues<CompanyStatus>());
            Validate(request, new
            {
                id = "required|string",
                name = "string",
                registrationNumber = "string",
                status = ValidatorExtension.EnumValues<CompanyStatus>(),
            });

            string id = request.Value<string>("id");
            using (var db = GetDatabase)
            {
                Company company = db.Companies.Find(id);
                if (company == null)
                {
                    throw new Exception("Company not found");
                }
                company.Name = request.ValueWithFallback<string>("name", company.Name);
                company.RegistrationNumber = request.ValueWithFallback<string>("registrationNumber", company.RegistrationNumber);
                company.Status = request.ValueWithFallback<CompanyStatus>("status", company.Status);
                db.SaveChanges();
                return company;
            }
        }

        [Service("removeCompanyById")]
        public object removeCompanyById(Context request)
        {
            Validate(request, new
            {
                id = "required|string",
            });

            string id = request.Value<string>("id");
            using (var db = GetDatabase)
            {
                Company company = db.Companies.Find(id);
                if (company == null)
                {
                    throw new Exception("Company not found");
                }

                db.Companies.Remove(company);
                return company;
            }
        }


        [Service("assignMasterCompany")]
        public object assignMasterCompany(Context request)
        {
            Validate(request, new
            {
                masterCompanyId = "required|string",
                childCompanyId = "required|string",
            });

            string masterCompanyId = request.Value<string>("masterCompanyId");
            string childCompanyId = request.Value<string>("childCompanyId");
            using (var db = GetDatabase)
            {
                Company master = db.Companies.Find(masterCompanyId);
                if (master == null)
                {
                    throw new Exception("Master company not found");
                }

                Company child = db.Companies.Find(childCompanyId);
                if (child == null)
                {
                    throw new Exception("Child company not found");
                }

                child.BranchOf = master.Id;
                db.SaveChanges();
                return new
                {
                    master = master,
                    child = child,
                };
            }
        }

        [Service("createChildCompany")]
        public object createChildCompany(Context request)
        {
            Validate(request, new
            {
                masterCompanyId = "required|string",
                registrationNumber = "string",
                name = "required|alpha_num"
            });

            string companyId = request.Value<string>("id");
            using (var db = GetDatabase)
            {
                Company instance = db.Companies.Find(companyId);
                if (instance == null)
                {
                    throw new Exception("Master company not found.");
                }

                Company company = new Company();
                company.Name = request.Value<string>("name");
                company.RegistrationNumber = request.Value<string>("regNo");
                company.BranchOf = request.Value<string>("masterCompanyId");
                company.Status = CompanyStatus.ACTIVE;
                db.Companies.Add(company);
                db.SaveChanges();
                return company;
            }
        }
    }
}