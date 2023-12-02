using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TazaFood.Core.Models
{
    public class BasketItem :BaseModel
    {
        public string Name { get; set; }
        //public string Description { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public string ImageUrl { get; set; }
        public int Quantity { get; set; } = 1;
    }
}
