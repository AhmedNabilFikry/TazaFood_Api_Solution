using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TazaFood.Core.Models;
using TazaFood.Core.Specification;

namespace TazaFood.Repository
{
    public static class SpecificationEvaluator<T> where T : BaseModel
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery ,ISpecification<T> spec) 
        {
            var query = inputQuery;
            if (spec.Criteria is not null) // where "Has A Product"
            {
                query = query.Where(spec.Criteria);
            }

            if (spec.OrderBy is not null)
            {
                query = query.OrderBy(spec.OrderBy);
            }

            if (spec.OrderByDesc is not null)
            {
                query = query.OrderByDescending(spec.OrderByDesc);
            }

            // Apply Pagination
            if (spec.IsPaginationEnabled)
            {
                query = query.Skip(spec.Skip).Take(spec.Take);
            }

            // Add Include with aggregate 
            query = spec.Includes.Aggregate(query, (CurrentQuery, IncludeExpression) => CurrentQuery.Include(IncludeExpression));
            return query;
        }
    }
}
