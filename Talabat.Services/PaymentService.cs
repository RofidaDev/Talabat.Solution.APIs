using Microsoft.Extensions.Configuration;
using Stripe;
using Product = Talabat.Core.Entities.Product;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specificatios.Order_Specs;

namespace Talabat.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IConfiguration configuration, IBasketRepository basketRepository, IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<CustomerBasket?> CreateOrUpdetePaymentIntent(string basketId)  //use it through endpoint
        {
            StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];
            var basket = await _basketRepository.GetBasketAsync(basketId);
            if (basket is null) return null;
            var shippingPrice = 0m;
            if (basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetAsync(basket.DeliveryMethodId.Value);
                basket.ShippingPrice = deliveryMethod.Cost;
                shippingPrice = deliveryMethod.Cost;
            }
            if (basket.Items.Count > 0)
            {
                foreach (var item in basket.Items)
                {
                    var product = await _unitOfWork.Repository<Product>().GetAsync(item.Id);
                    if (item.Price != product.Price)
                        item.Price = product.Price;
                    PaymentIntent paymentIntent;
                    PaymentIntentService paymentIntentService = new PaymentIntentService();
                    if (string.IsNullOrEmpty(basket.PaymentIntentId))  //create new payment intent
                    {
                        var createOptions = new PaymentIntentCreateOptions()
                        {
                            Amount = (long)basket.Items.Sum(items => items.Price * items.Quantity * 100) + (long)shippingPrice * 100,
                            Currency = "usd", //dollar
                            PaymentMethodTypes = new List<string>() { "card" }

                        };
                        paymentIntent = await paymentIntentService.CreateAsync(createOptions);  //Integrate with Stripe
                        basket.PaymentIntentId = paymentIntent.Id;
                        basket.ClientSecret = paymentIntent.ClientSecret;
                    }
                    else   //update existing payment intent
                    {
                        var updateOptions = new PaymentIntentUpdateOptions()
                        {
                            Amount = (long)basket.Items.Sum(items => items.Price * items.Quantity * 100) + (long)shippingPrice * 100
                        };
                        await paymentIntentService.UpdateAsync(basket.PaymentIntentId, updateOptions);
                    }
                } }
                    await _basketRepository.UpdateBasketAsync(basket);                 
                return basket;
        }

        async Task<Order> IPaymentService.UpdatePaymentIntentToSucceededOrFailed(string paymentIntentId,bool isSucceeded)
        {
            var spec = new OrderWithPaymentIntentSpecifications(paymentIntentId);
            var order = await _unitOfWork.Repository<Order>().GetWithSpecAsync(spec);
            if (isSucceeded)
                order.Status = OrderStatus.PaymentReceived;
            else
                order.Status = OrderStatus.PaymentFailed;

            _unitOfWork.Repository<Order>().Update(order);
            await _unitOfWork.CompleteAsync();
            return order;
        }
    }
}
