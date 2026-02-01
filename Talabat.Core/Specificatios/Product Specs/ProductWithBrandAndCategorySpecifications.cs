
using Talabat.Core.Entities;


namespace Talabat.Core.Specificatios.ProductSpecs
{
    public class ProductWithBrandAndCategorySpecifications:BaseSpecification<Product>
    {
        //This constructor will be used for creating an object, That will be used to get all products
        public ProductWithBrandAndCategorySpecifications(ProductSpecParams productSpec):base(
           
            P => string.IsNullOrEmpty(productSpec.search) ||P.Name.ToLower().Contains(productSpec.search.ToLower())&&
            (!productSpec.brandId.HasValue || P.BrandId == productSpec.brandId.Value) &&
            (!productSpec.categoryId.HasValue || P.ProductCategoryId == productSpec.categoryId)
            )
        {
            //    Includes.Add(P=>P.Brand);
            //    Includes.Add(P=>P.Category);
            AddIncludes();
            if (!string.IsNullOrEmpty(productSpec.sort)){
                switch (productSpec.sort)
                {
                    case "priceAsc":
                        AddOrderBy(p => p.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDesc(p => p.Price);
                        break;
                    default:
                        AddOrderBy(p => p.Name);
                        break;
                } }
            else
                AddOrderBy(p => p.Name);
            // totalProduct =18 ~20  => 4 pages
            // pageSize = 5
            // pageIndex = 3  skip=10  take=5
            ApplyPagination((productSpec.pageIndex-1)*productSpec.PageSize,productSpec.PageSize);

        }
        //This constructor will be used for creating an object, That will be used to get a specific product with id
        public ProductWithBrandAndCategorySpecifications(int id):base(P=>P.Id==id)
        {
            //Includes.Add(P => P.Brand);
            //Includes.Add(P => P.Category);
            AddIncludes();
        }
        private void AddIncludes()
        {
            Includes.Add(P => P.Brand);
            Includes.Add(P => P.Category);

        }
    }
}
