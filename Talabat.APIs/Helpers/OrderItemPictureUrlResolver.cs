using AutoMapper;
using Talabat.APIs.Dtos;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.APIs.Helpers
{
    public class OrderItemPictureUrlResolver:IValueResolver<OrderItem,OrderItemDto,string>
    {
        private readonly IConfiguration _configuration;   //to connect to appsettings

        public OrderItemPictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.Product.PictureUrl))
            {
                //return $"{"https://localhost:7197"}/{source.PictureUrl}";    //static
                return $"{_configuration["ApiBaseUrl"]}/{source.Product.PictureUrl}";
            }
            return string.Empty;
        }
    }
}
