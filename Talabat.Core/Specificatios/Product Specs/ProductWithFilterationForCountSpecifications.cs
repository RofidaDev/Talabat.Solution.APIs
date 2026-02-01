using Talabat.Core.Entities;


namespace Talabat.Core.Specificatios.ProductSpecs
{
    public class ProductWithFilterationForCountSpecifications:BaseSpecification<Product>
    {
        public ProductWithFilterationForCountSpecifications(ProductSpecParams productSpec):base(
            P => string.IsNullOrEmpty(productSpec.search) || P.Name.ToLower().Contains(productSpec.search.ToLower()) &&
            (!productSpec.brandId.HasValue || P.BrandId == productSpec.brandId.Value) &&
            (!productSpec.categoryId.HasValue || P.ProductCategoryId == productSpec.categoryId)
            )
        {
            
        }
    }
}
