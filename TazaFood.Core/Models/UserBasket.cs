using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TazaFood.Core.Models
{
    public class UserBasket
    {
        public string ID { get; set; }
        public List<BasketItem> items { get; set; } = new List<BasketItem>();
        public string? PaymentIntentId { get; set; }
        public string? ClientSecret { get; set; }
        public int? DeliveryMethodId { get; set; }
        public decimal? ShippingCost { get; set; }
        //public UserBasket()
        //{

        //}

        public UserBasket(string id)
        {
             ID = id;
        }
    }
}
