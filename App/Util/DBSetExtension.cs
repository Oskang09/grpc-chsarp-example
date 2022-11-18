using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace amantiq.util
{
    public static class DBSetExtension
    {
        public static object Paginate<T>(this DbSet<T> db, int limit, int page, Func<IQueryable<T>, IQueryable<T>> populateQuery) where T : class
        {
            if (limit <= 0)
            {
                limit = 10;
            }

            if (page <= 0)
            {
                page = 1;
            }

            var query = db.AsQueryable();
            if (populateQuery != null)
            {
                query = populateQuery(query);
            }

            int count = query.Count();
            int maxPage = (int)Math.Ceiling((decimal)count / limit);
            return new
            {
                items = query.Skip((page - 1) * limit).Take(limit).ToList<T>(),
                meta = new
                {
                    first = 1,
                    next = page >= maxPage ? 0 : page + 1,
                    prev = page <= 1 ? 0 : page - 1,
                    last = maxPage,
                    limit = limit,
                },
            };
        }
    }
}