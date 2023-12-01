using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TazaFood.Core.Specification
{
    public class ProductSpecParams
    {
        public decimal? Price { get; set; }
        public decimal? Rate { get; set; }
        public string? Sort { get; set; }
        public int? CategoryID { get; set; }

        private const int MaxPageSize = 10;
        private int pageSize = 8;

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value > MaxPageSize ? MaxPageSize : value; }
        }

        public int PageIndex { get; set; } = 1;

    }
}
