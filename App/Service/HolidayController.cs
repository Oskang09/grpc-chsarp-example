using System;
using System.Collections.Generic;
using amantiq.model;
using Newtonsoft.Json.Linq;
using amantiq.util;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace amantiq.service
{
    public class HolidayController : Controller
    {
        [Service("findHolidays")]
        public object findHolidays(Context request)
        {
            Validate(request, new
            {
                limit = "integer",
                page = "integer",
                year = "integer",
                name = "string",
            });

            using (var db = GetDatabase)
            {
                return db.Holidays.Paginate(request.Value<int>("limit"), request.Value<int>("page"), (query) =>
                {
                    int year = request.ValueWithFallback("year", 0);
                    if (year > 0)
                    {
                        query = query.Where(x => x.Start.Year == year && x.End.Year == year);
                    }

                    string name = request.ValueWithFallback("name", "");
                    if (name != "")
                    {
                        query = query.Where(x => x.Name.Contains(name));
                    }
                    return query;
                });
            }
        }

        [Service("createHoliday")]
        public object createHoliday(Context request)
        {
            Validate(request, new
            {
                name = "required|string",
                start = "required|datetime",
                end = "datetime",
                applicable = "array"
            });

            Holiday holiday = new Holiday();
            holiday.Id = GeneratorExtension.GenerateUNIQ();
            holiday.Name = request.Value<string>("name");
            holiday.Start = request.Value<DateTime>("start");
            holiday.End = request.Value<DateTime>("end");
            holiday.Applicable = request.ObjectValue<List<string>>("applicable");
            holiday.CreatedDateTime = DateTime.Now;
            holiday.UpdatedDateTime = DateTime.Now;
            using (var db = GetDatabase)
            {
                db.Holidays.Add(holiday);
                db.SaveChanges();
            }
            return holiday;
        }

        [Service("updateHolidayById")]
        public object updateHolidayById(Context request)
        {
            Validate(request, new
            {
                id = "required|string",
                name = "string",
                start = "datetime",
                end = "datetime",
                applicable = "array"
            });

            string id = request.Value<string>("id");
            using (var db = GetDatabase)
            {
                Holiday holiday = db.Holidays.Find(id);
                if (holiday == null)
                {
                    throw new Exception("Entity not found.");
                }

                holiday.Name = request.ValueWithFallback<string>("name", holiday.Name);
                holiday.Start = request.ValueWithFallback<DateTime>("start", holiday.Start);
                holiday.End = (DateTime)request.ValueWithFallback<DateTime?>("end", holiday.End);
                holiday.Applicable = request.ValueWithFallback<List<string>>("applicable", holiday.Applicable);
                db.SaveChanges();

                return holiday;
            }
        }

        [Service("removeHolidayById")]
        public object removeHolidayById(Context request)
        {
            Validate(request, new
            {
                id = "required|string"
            });

            string id = request.Value<string>("id");
            using (var db = GetDatabase)
            {
                Holiday holiday = db.Holidays.Find(id);
                if (holiday == null)
                {
                    throw new Exception("Holiday not found");
                }

                db.Holidays.Remove(holiday);
                db.SaveChanges();
                return holiday;
            }
        }

        [Service("selectHoliday")]
        public object selectHoliday(Context request)
        {
            Validate(request, new
            {
                companyId = "required|string",
                year = "required|integer",
                holidayId = Rules.Array("string"),
            });

            string companyId = request.Value<string>("companyId");
            int year = request.Value<int>("year");
            using (var db = GetDatabase)
            {
                Company company = db.Companies.Find(companyId);
                company.Holidays = request.ValueWithFallback<List<string>>("holidayId", new List<string>());
                return company;
            }
        }
    }
}