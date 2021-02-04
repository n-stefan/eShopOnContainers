using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using WebBlazor.Client.Services.ModelDTOs;

namespace WebBlazor.Client.Services
{
    interface IOrderingService
    {
        OrderDTO MapUserInfoIntoOrder(/*ApplicationUser*/ClaimsPrincipal user, OrderDTO order);

        BasketCheckoutDTO MapOrderToBasket(OrderDTO order);

        Task<List<OrderDTO>> GetMyOrders(/*ApplicationUser*/string userId);

        Task CancelOrder(string orderId);
    }
}
