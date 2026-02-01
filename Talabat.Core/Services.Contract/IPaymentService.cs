using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Core.Services.Contract
{
    public interface IPaymentService
    {
        Task<CustomerBasket?> CreateOrUpdetePaymentIntent(string basketId);
        Task<Order> UpdatePaymentIntentToSucceededOrFailed(string paymentIntentId, bool isSucceeded);
    }
}
