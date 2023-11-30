using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TazaFood.Core.Models;

namespace TazaFood.Core.Specification
{
    public class ProductWithCategorySpec:BaseSpecification<Product>
    {
        // Get All Products 
        public ProductWithCategorySpec() // includes 
        {
            Includes.Add(P => P.Category);
        }
        // Get A Specific Product
        public ProductWithCategorySpec(int ID):base(P => P.ID == ID) // Where
        {
            Includes.Add(P => P.Category);
        }
    }
}
