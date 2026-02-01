
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Core.Specificatios.Order_Specs
{
    public class OrderWithPaymentIntentSpecifications:BaseSpecification<Order>
    {
        public OrderWithPaymentIntentSpecifications(string payentIntentId) : base(O => O.PaymentIntentId == payentIntentId)
        {

           
        }
    }
}
