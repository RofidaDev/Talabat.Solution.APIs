using StackExchange.Redis;
using System;
using System.Collections.Generic;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;

namespace Talabat.Repository
{
   
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;
        public BasketRepository(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }
        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            return await _database.KeyDeleteAsync(basketId);
        }

        public async Task<CustomerBasket?> GetBasketAsync(string basketId)
        {
            var basket = await _database.StringGetAsync(basketId); //return redisValue(struct) =>json
            return basket.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(basket);
        }

        public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket)
        { //return created or updated basket
            var createdOrUpdate = await _database.StringSetAsync(basket.Id, JsonSerializer.Serialize(basket), TimeSpan.FromDays(30)); //bool
            if (createdOrUpdate is false) return null;
            return await GetBasketAsync(basket.Id);
        } 
    }
}
