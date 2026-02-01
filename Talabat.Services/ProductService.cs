
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specificatios.ProductSpecs;


namespace Talabat.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Product?> GetProductAsync(int productId)
        {
            var spec = new ProductWithBrandAndCategorySpecifications(productId);
            var product = await _unitOfWork.Repository<Product>().GetWithSpecAsync(spec);
            return product;
        }

        public async Task<IReadOnlyList<Product>> GetProductsAsync(ProductSpecParams specParams)
        {
            var spec = new ProductWithBrandAndCategorySpecifications(specParams);
            var products = await _unitOfWork.Repository<Product>().GetAllWithSpecAsync(spec);
            return products;

        }

        async Task<IReadOnlyList<ProductBrand>> IProductService.GetBrandssAsync()
         => await _unitOfWork.Repository<ProductBrand>().GetAllAsync();

        async Task<IReadOnlyList<ProductCategory>> IProductService.GetCategoriesAsync()
          => await _unitOfWork.Repository<ProductCategory>().GetAllAsync();

        async Task<int> IProductService.GetCountAsync(ProductSpecParams specParams)
        {
            var countSpec = new ProductWithFilterationForCountSpecifications(specParams);
            var count = await _unitOfWork.Repository<Product>().GetCountSpecAsync(countSpec);
            return count;
        }

    }
}
