using System.Collections.Generic;
using System.Threading.Tasks;
using WebBlazor.Client.Services.ModelDTOs;

namespace WebBlazor.Client.Services
{
    public interface IBasketService
    {
        Task<BasketDTO> GetBasket(string userId);

        Task AddItemToBasket(string userId, int productId);

        Task<BasketDTO> SetQuantities(string userId, Dictionary<string, int> quantities);

        Task<BasketDTO> UpdateBasket(BasketDTO basket);

        Task<OrderDTO> GetOrderDraft(string basketId);

        Task Checkout(BasketCheckoutDTO basket);
    }
}
