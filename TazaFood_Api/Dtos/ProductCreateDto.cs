namespace TazaFood_Api.Dtos
{
    public class ProductCreateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Rate { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public IFormFile formFile { get; set; }
    }
}
