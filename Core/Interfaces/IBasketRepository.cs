using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IBasketRepository
    {
        Task<List<UsersBasket>> GetBasketAsync(string basketId);
        Task<List<UsersBasket>> UpdateBasketAsync(CustomerBasket basket);
        Task<bool> DeleteBasketAsync(string basketId);

        public void AddBasket(CustomerBasket basket);
    }
}
