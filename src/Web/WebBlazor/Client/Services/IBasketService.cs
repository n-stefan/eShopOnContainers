using System.Threading.Tasks;
using WebBlazor.Client.Services.ModelDTOs;

namespace WebBlazor.Client.Services
{
    public interface IBasketService
    {
        Task<BasketDTO> GetBasket(/*ApplicationUser user*/string userId);

        Task AddItemToBasket(/*ApplicationUser user*/string userId, int productId);
    }
}
