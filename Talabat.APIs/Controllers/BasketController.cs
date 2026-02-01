using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;

namespace Talabat.APIs.Controllers
{
   
    public class BasketController : BaseApiController
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository basketRepo ,IMapper mapper)
        {
            _basketRepo = basketRepo;
            _mapper = mapper;
        }
        [HttpGet]  //api/Basket?id    
        //id => not as a segment (api/Basket/id)
        public async Task<ActionResult<CustomerBasketDto>> GetBasket(string id)
        {
           var basket = await _basketRepo.GetBasketAsync(id);

            return Ok(basket ?? new CustomerBasket(id));   //it was exist but it expired ,create new basket with the same id
        }
        [HttpDelete]
        public async Task DeleteBasket(string id)
        {
            await _basketRepo.DeleteBasketAsync(id);
        }
        [HttpPost]     //create or update
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto basket)
        {
          var mappedBasket = _mapper.Map<CustomerBasketDto,CustomerBasket>(basket);
          var createdOrUpdatedBasket = await _basketRepo.UpdateBasketAsync(mappedBasket);
          if (createdOrUpdatedBasket == null) return BadRequest(new ApiResponse(400));
          return Ok(createdOrUpdatedBasket);
        }
    }
}
