using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TazaFood.Core.Models;

namespace TazaFood.Core.Specification
{
    public class BaseSpecification<T> : ISpecification<T> where T : BaseModel
    {
        public Expression<Func<T, bool>> Criteria { get; set; }
        public List<Expression<Func<T, object>>> Includes { get; set; } = new List<Expression<Func<T, object>>>();
        public Expression<Func<T, object>> OrderBy { get; set; }
        public Expression<Func<T, object>> OrderByDesc { get ; set; }
        public int Skip { get ; set; }
        public int Take { get; set; }
        public bool IsPaginationEnabled { get ; set ; }

        public BaseSpecification() {} // Include Condition , Setting Criteria with null
        public BaseSpecification(Expression<Func<T, bool>> _criteria) // Where Condition 
        {
            Criteria = _criteria;
        }

        // Methods For OrderBy Properties Setting 
        public void AddOrderBy(Expression<Func<T, object>> OrderByExpression)
        {
            OrderBy = OrderByExpression;
        }
        public void AddOrderByDesc(Expression<Func<T, object>> OrderbyDescExpression)
        {
            OrderByDesc = OrderbyDescExpression;
        }
        public void ApplyPagination(int _skip , int _take)
        {
            if (_skip >= 0 && _take >= 0) // Ensure non-negative values for skip and take
            {
                IsPaginationEnabled = true;
                Skip = _skip;
                Take = _take;
            }
        }
    }
}
