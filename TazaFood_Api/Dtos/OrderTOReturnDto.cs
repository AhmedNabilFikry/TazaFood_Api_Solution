using TazaFood.Core.Models.Order_Aggregate;

namespace TazaFood_Api.Dtos
{
    public class OrderTOReturnDto
    {
        public int Id { get; set; }
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public string Status { get; set; }
        public Address ShippingAddress { get; set; }
        public string DeliveryMethod { get; set; }
        public string DeliveryTime { get; set; }
        public decimal Cost { get; set; }
        public ICollection<OrderItemDto> OrderItems { get; set; }
        public decimal Total { get; set; }
        public string PaymentIntent { get; set; }
    }
}
