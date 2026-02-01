using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Services.Contract;

namespace Talabat.APIs.Controllers
{
    [Authorize]
    public class OrdersController : BaseApiController
    {
        private readonly IOrderService _orederService;
        private readonly IMapper _mapper;

        public OrdersController(IOrderService orederService, IMapper mapper)
        {
            _orederService = orederService;
            _mapper = mapper;
        }
        [ProducesResponseType(typeof(OrderToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [HttpPost]  //api/Orders
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDTO orderDTO)
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var address = _mapper.Map<AddressDTO, Address>(orderDTO.ShippingAddress);
            var order = await _orederService.CreateOrderAsync(buyerEmail, orderDTO.BasketId, orderDTO.DeliveryMethodId, address);
            if (order is null) return BadRequest(new ApiResponse(400));
            return Ok(_mapper.Map<Core.Entities.Order_Aggregate.Order,OrderToReturnDto>(order));
        }
        [HttpGet] //api/Orders
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersForUser()
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var orders = await _orederService.GetOrdersForUserAsync(buyerEmail);
            return Ok(_mapper.Map<IReadOnlyList<Core.Entities.Order_Aggregate.Order>, IReadOnlyList<OrderToReturnDto>>(orders));
        }
        [HttpGet("{id}")] //api/Orders/1
        [ProducesResponseType(typeof(OrderToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderForUser(int id)
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var order = await _orederService.GetOrderByIdForUserAsync(id, buyerEmail);
            if (order is null) return NotFound(new ApiResponse(404));
            return Ok(_mapper.Map < Core.Entities.Order_Aggregate.Order, OrderToReturnDto >(order));

        }
        [HttpGet("DeliveryMethods")]   //api/Orders/DeliveryMethods
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            var deliveryMethods = await _orederService.GetDeliveryMethodsAsync();
            return Ok(deliveryMethods);
        }
    }
}
