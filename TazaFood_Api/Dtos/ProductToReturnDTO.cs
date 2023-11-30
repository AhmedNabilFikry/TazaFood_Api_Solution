using TazaFood.Core.Models;

namespace TazaFood_Api.Dtos
{
    public class ProductToReturnDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public decimal Rate { get; set; }
        public int CategoryID { get; set; }   
        public string Category { get; set; }  
    }
}
