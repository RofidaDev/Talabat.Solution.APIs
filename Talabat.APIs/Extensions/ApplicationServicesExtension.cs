using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Repository;
using Talabat.Services;

namespace Talabat.APIs.Extensions
{
    public static class ApplicationServicesExtension
    { //To minimize the main
        public static IServiceCollection AddApplicationServices(this IServiceCollection services /*The container of services*/)
        {
            //builder.Services.AddScoped<IGenaricRepository<Product>,GenaricRepository<Product>>();  //not genaric
            services.AddScoped(typeof(IGenaricRepository<>), typeof(GenaricRepository<>));   //this for all
            services.AddSingleton(typeof(IResponseCacheService), typeof(ResponseCacheService));
            services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));
            services.AddScoped(typeof(IPaymentService),typeof(PaymentService));    
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            services.AddScoped(typeof(IOrderService), typeof(OrderService));
            services.AddScoped(typeof(IProductService), typeof(ProductService));
            services.AddScoped<IAuthService, AuthService>();
            services.AddAutoMapper(typeof(MappingProfiles));  //or
            //services.AddAutoMapper(M=>M.AddProfile(new MappingProfiles())); 
            services.Configure<ApiBehaviorOptions>(options =>    //override
            options.InvalidModelStateResponseFactory = (actionContext) =>  //actionContext=info about request
            {  //modelState=dictionary(Id for properity,Modelerror)
                var errors = actionContext.ModelState.Where(P => P.Value.Errors.Count() > 0) //propertiess that has error
                .SelectMany(P => P.Value.Errors) //list of modelErrors
                .Select(E => E.ErrorMessage)  //message itself
                .ToArray();
                var validationErrorResponse = new ApiValidationErrorResponse()
                {
                    Errors = errors
                };
                return new BadRequestObjectResult(validationErrorResponse);
            });
            return services;
        }
    }
}
//Model state:
//Key: "Email"  //property
//Value: ["Email is required"]

