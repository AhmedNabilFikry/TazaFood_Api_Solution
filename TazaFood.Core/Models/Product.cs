using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TazaFood.Core.Models
{
    public class Product :BaseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public decimal Rate { get; set; }       
        public int CategoryID  { get; set; }    // Foreign Key : Not Allow Null (Required "Every Product Has a Category")
        public Category Category { get; set; }  // Navigational Prop (one To Many)
    }
}
