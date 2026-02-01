using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specificatios.ProductSpecs;


namespace Talabat.APIs.Controllers
{
    public class ProductsController : BaseApiController
    {
        //private readonly IGenaricRepository<Product> _productsRepo;
        //private readonly IGenaricRepository<ProductBrand> _brandsRepo;
        //private readonly IGenaricRepository<ProductCategory> _categoriesRepo;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductService _productService;

        public ProductsController(/*IGenaricRepository<Product> ProductsRepo,IGenaricRepository<ProductBrand> BrandsRepo*//*,IGenaricRepository<ProductCategory> CategoriesRepo,*/IMapper mapper,IUnitOfWork unitOfWork,IProductService productService)
        {
            //_productsRepo = ProductsRepo;
            //_brandsRepo = BrandsRepo;
            //_categoriesRepo = CategoriesRepo;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _productService = productService;
        }
        //Improving Swagger Documentation
        [ProducesResponseType(typeof(ProductToReturnDto),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [Authorize/*(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)*/]
        [Cached(600)] //Action Filter (Attrebute)
        [HttpGet]  //routing attribute   //not action filter
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts([FromQuery]ProductSpecParams productParams) 
        {

            var products = await _productService.GetProductsAsync(productParams);
            int count = await _productService.GetCountAsync(productParams);  //_dbContext.Set<Product>().Where(P=>P.    ) before pagination
            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);
           
            return Ok(new Pagination<ProductToReturnDto>(productParams.PageSize,productParams.pageIndex,count,data));    
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetById(int id)
        {
            
            var product = await _productService.GetProductAsync(id);
            if (product == null) {return NotFound(new ApiResponse(404));}
            return Ok(_mapper.Map<Product,ProductToReturnDto>(product));
        }
        [HttpGet("brands")] //api/Product/brands
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
          var brands =await _productService.GetBrandssAsync();
            return Ok(brands);
        }
        [HttpGet("categories")]  //api/Product/categories
        public async Task<ActionResult<IReadOnlyList<ProductCategory>>> GetCategoriess()
        {
            var categories = await _productService.GetCategoriesAsync();
            return Ok(categories);
        }
    }
}