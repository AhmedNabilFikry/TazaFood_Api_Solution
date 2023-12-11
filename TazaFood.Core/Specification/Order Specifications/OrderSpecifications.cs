using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TazaFood.Core.Models.Order_Aggregate;

namespace TazaFood.Core.Specification.Order_Specifications
{
    public class OrderSpecifications : BaseSpecification<Order>
    {
        public OrderSpecifications(string email)
            :base(O => O.BuyerEmail == email) 
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.orderItems);
            AddOrderByDesc(O => O.OrderDate);   
        }
        public OrderSpecifications(int Id, string email)
            :base(O => O.BuyerEmail == email && O.ID == Id)
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.orderItems);
        }
    }
}
