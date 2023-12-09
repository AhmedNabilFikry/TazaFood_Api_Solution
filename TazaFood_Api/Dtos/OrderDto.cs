using System.ComponentModel.DataAnnotations;

namespace TazaFood_Api.Dtos
{
    public class OrderDto
    {
        [Required]
        public string BaskektId { get; set; }
        [Required]
        public int DeliveryMethod { get; set; }
        public AddressDto ShippingAddress { get; set; }
    }
}
