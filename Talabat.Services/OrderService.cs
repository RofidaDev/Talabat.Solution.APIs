using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specificatios.Order_Specs;


namespace Talabat.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepository;
        //private readonly IGenaricRepository<Product> _productRepository;
        //private readonly IGenaricRepository<DeliveryMethod> _deliveryRepo;
        //private readonly IGenaricRepository<Order> _orderRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;

        public OrderService(IBasketRepository basketRepo,IUnitOfWork unitOfWork,IPaymentService paymentService)
        {
            _basketRepository = basketRepo;

            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
        }
        public async Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address shippingAddress)
        {
           // 1. Get Basket From Baskets Repo
           var basket = await _basketRepository.GetBasketAsync(basketId);
            // 2. Get Selected Items at Basket From Products Repo
            var orderItems = new List<OrderItem>();
            if (basket?.Items?.Count > 0) {
                var productRepo = _unitOfWork.Repository<Product>();
                foreach (var item in orderItems)
                {
                   var product = await productRepo.GetAsync(item.Id);
                    var productItemOrdered = new ProductItemOrdered(item.Id,product.Name,product.PictureUrl);
                    var orderItem = new OrderItem(productItemOrdered,product.Price,item.Quantity);
                    orderItems.Add(orderItem);
                }
            }
            // 3. Calculate SubTotal
            var subtotal = orderItems.Sum(orderItem => orderItem.Price * orderItem.Quantity);

           // 4. Get DeliveryMethod From DeliveryMethods Repo
           var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetAsync(deliveryMethodId);
            var orderRepo = _unitOfWork.Repository<Order>();
            var orderSpecs = new OrderWithPaymentIntentSpecifications(basket.PaymentIntentId);
            var existingOrder = await orderRepo.GetWithSpecAsync(orderSpecs);
            if (existingOrder != null)
            {
                orderRepo.Delete(existingOrder);
                await _paymentService.CreateOrUpdetePaymentIntent(basketId);
            }
           // 5. Create Order
           var order = new Order(buyerEmail, shippingAddress,deliveryMethod,orderItems,subtotal,basket.PaymentIntentId);
            await orderRepo.AddAsync(order);
            // 6. Save to Database
           var result = await _unitOfWork.CompleteAsync(); //save changes
            if (result <= 0) return null;
            return order;
        }

        public Task<Order?> GetOrderByIdForUserAsync(int orderId, string buyerEmail)
        {
            var orderRepo = _unitOfWork.Repository<Order>();
            var spec = new OrderSpecifications(orderId,buyerEmail);
            var order = orderRepo.GetWithSpecAsync(spec);
            return order;
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var orderRepo = _unitOfWork.Repository<Order>();
            var spec = new OrderSpecifications(buyerEmail);
            var orders = await orderRepo.GetAllWithSpecAsync(spec);
            return orders;
           

        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        => await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();    
    }
}
