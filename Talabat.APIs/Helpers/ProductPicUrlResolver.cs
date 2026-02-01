using AutoMapper;

using Talabat.APIs.Dtos;
using Talabat.Core.Entities;


namespace Talabat.APIs.Helpers
{
    public class ProductPicUrlResolver : IValueResolver<Product, ProductToReturnDto, string>
    {
        private readonly IConfiguration _configuration;   //to connect to appsettings

        public ProductPicUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public string Resolve(Product source, ProductToReturnDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
            {
                //return $"{"https://localhost:7197"}/{source.PictureUrl}";    //static
                return $"{_configuration["ApiBaseUrl"]}/{source.PictureUrl}";
            }
            return string.Empty;
        }
    }
}