using AutoMapper;
using Talabat.APIs.Dtos;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.APIs.Helpers
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(d => d.Brand, o => o.MapFrom(s => s.Brand.Name))
                .ForMember(d=>d.Category,o=>o.MapFrom(s=>s.Category.Name))
                .ForMember(d=>d.PictureUrl,o=>o.MapFrom<ProductPicUrlResolver>());
            //d=destination  o=option  s=source
            CreateMap<CustomerBasketDto, CustomerBasket>();
            CreateMap<BasketItem, BasketItemDto>();
            CreateMap<AddressDTO, Core.Entities.Order_Aggregate.Address>();  
            CreateMap<Core.Entities.Identity.Address, AddressDTO>().ReverseMap();
            CreateMap<BasketItemDto, BasketItem>();
            CreateMap<Order, OrderToReturnDto>()
                .ForMember(d=>d.DeliveryMethod,o=>o.MapFrom(s=>s.DeliveryMethod.ShortName))
                .ForMember(d => d.DeliveryMethodCost, o => o.MapFrom(s => s.DeliveryMethod.Cost));
            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d=>d.ProductName,o=>o.MapFrom(s=>s.Product.ProductName))
                 .ForMember(d => d.ProductId, o => o.MapFrom(s => s.Product.ProductId))
                  .ForMember(d => d.PictureUrl, o => o.MapFrom(s => s.Product.PictureUrl))
                  .ForMember(d => d.PictureUrl, o => o.MapFrom<OrderItemPictureUrlResolver>()); ;

        }
    }
}
