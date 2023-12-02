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
        //public UserBasket()
        //{
            
        //}
        public UserBasket(string id)
        {
             ID = id;
        }
    }
}
