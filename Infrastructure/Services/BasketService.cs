using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IProductRepository _productRepo;
        private readonly IMapper _mapper;
        public BasketService(IBasketRepository basketRepo,
            IProductRepository productRepo, IMapper mapper)
        {
            _basketRepo = basketRepo;
            _productRepo = productRepo;
            _mapper = mapper;
        }

        public async Task DeleteBasketAsync(string basketId)
        {
            await _basketRepo.DeleteBasketAsync(basketId);
        }

        public async Task<CustomerBasket> GetBasketAsync(string basketId)
        {
            var items = await _basketRepo.GetBasketAsync(basketId);
            if(items.Count == 0)
            {
                return null;
            }
            var customerBasket = new CustomerBasket(basketId);

            List<BasketItem> basketItems = new List<BasketItem>();
            foreach(var _basketItem in items)
            {
                var item = await _productRepo.GetProductByIdAsync(_basketItem.BasketItemId);
                var basketItemWithQuantity = _mapper.Map<BasketItem>(item);
                basketItemWithQuantity.Quantity = _basketItem.Quantity;

                basketItems.Add(basketItemWithQuantity);
            }

            customerBasket.Items = basketItems;
            customerBasket.ClientSecret = items[0].ClientSecret;
            customerBasket.ShippingPrice= items[0].ShippingPrice;
            customerBasket.DeliveryMethodId = items[0].DeliveryMethodId;
            customerBasket.PaymentIntentId= items[0].PaymentIntentId;

            return customerBasket;
        }

        public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
        {
            await _basketRepo.UpdateBasketAsync(basket);
            return await GetBasketAsync(basket.Id);

        }
    }
}
