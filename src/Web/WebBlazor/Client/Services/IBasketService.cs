using System.Threading.Tasks;
using WebBlazor.Client.Services.ModelDTOs;

namespace WebBlazor.Client.Services
{
    public interface IBasketService
    {
        Task<BasketDTO> GetBasket(/*ApplicationUser user*/string userId);
    }
}
