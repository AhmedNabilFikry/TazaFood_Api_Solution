using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TazaFood.Core.Models.Order_Aggregate;

namespace TazaFood.Core.Specification.Order_Specifications
{
    public class OrderWithPaymentIntentIdSpecifications : BaseSpecification<Order>
    {
        public OrderWithPaymentIntentIdSpecifications(string PaymentIntentId)
            :base(O => O.PaymentIntentId == PaymentIntentId)
        {
            
        }
    }
}
