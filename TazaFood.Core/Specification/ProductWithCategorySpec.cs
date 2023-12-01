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
        public ProductWithCategorySpec()
        {
            Includes.Add(P => P.Category);
        }
        public ProductWithCategorySpec(string Sort) 
        {
            Includes.Add(P => P.Category);

            // If There IS not type of sorting 
            AddOrderBy(P => P.Name);

            if (!string.IsNullOrEmpty(Sort))
            { 
                switch (Sort)
                {
                    case "PriceAsc":AddOrderBy(P => P.Price);
                        break;
                    case "PriceDesc":AddOrderByDesc(P => P.Price);
                        break;
                    case "RateAsc":AddOrderBy(P => P.Rate);
                        break;
                    case "RateDesc":AddOrderByDesc(P => P.Rate);
                        break;
                    case "NameDesc":AddOrderByDesc(P => P.Name);
                        break;
                    default:AddOrderBy(P => P.Name);
                        break;
                }
            }
        }
        public ProductWithCategorySpec(int? Price, int? CategoryID, int? Rate)
            :base(P =>
            (!Price.HasValue || P.Price == Price) &&
            (!CategoryID.HasValue || P.CategoryID == CategoryID) &&
            (!Rate.HasValue || P.Rate == Rate)
            )
        {
            Includes.Add(P => P.Category);
        }

        // Get A Specific Product
        public ProductWithCategorySpec(int ID):base(P => P.ID == ID) // Where
        {
            Includes.Add(P => P.Category);
        }
        public ProductWithCategorySpec(ProductSpecParams specParams)
            :base( P =>
                 (!specParams.Rate.HasValue || specParams.Rate == P.Rate) &&
                 (!specParams.Price.HasValue || specParams.Price == P.Price) &&
                 (!specParams.CategoryID.HasValue || specParams.CategoryID == P.CategoryID)
                 )
        {
            // If There IS not type of sorting 
            AddOrderBy(P => P.Name);

            if (!string.IsNullOrEmpty(specParams.Sort))
            {
                switch (specParams.Sort)
                {
                    case "PriceAsc":
                        AddOrderBy(P => P.Price);
                        break;
                    case "PriceDesc":
                        AddOrderByDesc(P => P.Price);
                        break;
                    case "RateAsc":
                        AddOrderBy(P => P.Rate);
                        break;
                    case "RateDesc":
                        AddOrderByDesc(P => P.Rate);
                        break;
                    case "NameDesc":
                        AddOrderByDesc(P => P.Name);
                        break;
                    default:
                        AddOrderBy(P => P.Name);
                        break;
                }
            }
            ApplyPagination(specParams.PageSize * (specParams.PageIndex - 1) , specParams.PageSize );
        }
    }
}
