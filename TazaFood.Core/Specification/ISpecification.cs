using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TazaFood.Core.Models;

namespace TazaFood.Core.Specification
{
    public interface ISpecification<T> where T :BaseModel
    {
        // Properties Signature
        public Expression<Func<T,bool>> Criteria { get; set; } // where(P => P.ID == ID);
        public List<Expression<Func<T,Object>>> Includes {  get; set; }  //include(P => P.Category)
        public Expression<Func<T,object>> OrderBy { get; set; } // OrderBy(P => P.Name)
        public Expression<Func<T,object>> OrderByDesc { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public bool IsPaginationEnabled { get; set; }
    }
}
