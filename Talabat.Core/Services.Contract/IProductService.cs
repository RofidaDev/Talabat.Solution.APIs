using Talabat.Core.Entities;
using Talabat.Core.Specificatios.ProductSpecs;


namespace Talabat.Core.Services.Contract
{
    public interface IProductService
    {
        Task<IReadOnlyList<Product>> GetProductsAsync(ProductSpecParams specParams);
        Task<Product?> GetProductAsync(int productId);
        Task<int> GetCountAsync (ProductSpecParams specParams);
        Task<IReadOnlyList<ProductBrand>> GetBrandssAsync();
        Task<IReadOnlyList<ProductCategory>> GetCategoriesAsync();
    }
}
