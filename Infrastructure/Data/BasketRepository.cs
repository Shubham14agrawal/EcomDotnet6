using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace Infrastructure.Data
{
    public class BasketRepository : IBasketRepository
    {
        private readonly StoreContext _context;
        //public BasketRepository(IConnectionMultiplexer redis)
        //{
        //    _database = redis.GetDatabase();
        //}

        public BasketRepository (StoreContext context)
        {
            _context = context;
        }
        
        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            try
            {
                var userBasketItems = await _context.UserBaskets.Where(basket => basket.Id == basketId).ToListAsync();
                _context.UserBaskets.RemoveRange(userBasketItems);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<List<UsersBasket>> GetBasketAsync(string basketId)
        {
            var items = await _context.UserBaskets.Where(basketdetails => basketId == basketdetails.Id).ToListAsync();
            return items;
        }

        public async Task<List<UsersBasket>> UpdateBasketAsync(CustomerBasket basket)
        {

            await DeleteBasketAsync(basket.Id);
            AddBasket(basket);

            return await GetBasketAsync(basket.Id);
        }

        public void AddBasket(CustomerBasket basket)
        {
            foreach(var basketItem in basket.Items)
            {
                var userBasketItem = new UsersBasket();
                userBasketItem.Id = basket.Id;
                userBasketItem.BasketItemId = 
                userBasketItem.Quantity = basketItem.Quantity;
                userBasketItem.BasketItemId = basketItem.Id;
                userBasketItem.ClientSecret = basket.ClientSecret;
                userBasketItem.ShippingPrice = basket.ShippingPrice;
                userBasketItem.DeliveryMethodId = basket.DeliveryMethodId;
                userBasketItem.PaymentIntentId = basket.PaymentIntentId;
                _context.UserBaskets.Add(userBasketItem);
            }
            _context.SaveChanges();
        }
    }
}