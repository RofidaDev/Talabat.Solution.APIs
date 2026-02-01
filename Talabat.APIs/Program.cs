using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Talabat.APIs.Extensions;
using Talabat.APIs.Helpers;
using Talabat.APIs.Middlewares;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services.Contract;
using Talabat.Repository.Data;
using Talabat.Repository.Identity;
using Talabat.Services;

namespace Talabat.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddDbContext<StoreContext>(options/*optionsBuilder:create options for Base(ctor) to send connectionStirng*/ =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
            );
            builder.Services.AddDbContext<AppIdentityDbContext>(optionsBuilder =>
            optionsBuilder.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"))
          
            );
           
            builder.Services.AddSingleton<IConnectionMultiplexer>((serviveProvider) =>  //service from serviceProvider of type(IConnectionMultiplexer)
            {
                var connection = builder.Configuration.GetConnectionString("Redis");  //read appsitting
                return ConnectionMultiplexer.Connect(connection);    //connect Redis(ConnectionMultiplexer) to localhost(my pc)
            });
            builder.Services.AddSwaggerServices();
            builder.Services.AddApplicationServices();
            builder.Services.AddIdentityServices(builder.Configuration);
            builder.Services.AddCors(options=>
            {
                options.AddPolicy("MyPolicy", options =>
                {
                    options.AllowAnyHeader().AllowAnyMethod().WithOrigins(builder.Configuration["FrontBaseUrl"]);
                });
            });
            var app = builder.Build();
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;   //obj from scopeServices  //provide any service in dependency injection container
            var _dbcontext = services.GetRequiredService<StoreContext>(); //ask CLR to create obj from DbContext explicitly
            var _identityDbContext = services.GetRequiredService<AppIdentityDbContext>();
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();  //obg from class that implement ILoggerFactory
            try
            {
                await _dbcontext.Database.MigrateAsync();  //Update Database  //not manual
                await StoreContextSeed.SeedAsync(_dbcontext);    //data seeding
                await _identityDbContext.Database.MigrateAsync();

                var _userManager = services.GetRequiredService<UserManager<AppUser>>();
                await AppIdentityDbContextSeed.SeedAsync(_userManager);

            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "an error has been occured during aplly migrations");
            }
            // Configure the HTTP request pipeline.
            app.UseMiddleware<ExceptionMiddleware>();
            if (app.Environment.IsDevelopment())
            { }
            app.UseSwaggerMiddleware();
            
            app.UseStatusCodePagesWithReExecute("/errors/{0}"); //excute error end poind
           /* app.UseStatusCodePagesWithRedirects("/errors/{0}");*/ //not 0 as number,it refers to a value
            //and this for redirection to error endpoint in case on Not  Found //Redirect=> its statusCode=302
            app.UseHttpsRedirection();
            app.UseStaticFiles();   //To allow the kestrel to handle requests ask for static files(like pictures)

            app.UseCors("MyPolicy");


            app.MapControllers();
            app.UseAuthentication();
            app.UseAuthorization();
            app.Run();
        }
    }
}
