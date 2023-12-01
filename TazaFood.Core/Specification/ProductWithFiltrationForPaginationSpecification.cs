using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TazaFood.Core.Models;

namespace TazaFood.Core.Specification
{
    public class ProductWithFiltrationForPaginationSpecification :BaseSpecification<Product>
    {
        public ProductWithFiltrationForPaginationSpecification( ProductSpecParams specParams)
             : base(P =>
                 (!specParams.Rate.HasValue || specParams.Rate == P.Rate) &&
                 (!specParams.Price.HasValue || specParams.Price == P.Price) &&
                 (!specParams.CategoryID.HasValue || specParams.CategoryID == P.CategoryID)
                 )
        {
            
        }
    }
}
