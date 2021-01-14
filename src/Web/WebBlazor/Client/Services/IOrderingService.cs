using System.Security.Claims;
using WebBlazor.Client.Services.ModelDTOs;

namespace WebBlazor.Client.Services
{
    interface IOrderingService
    {
        OrderDTO MapUserInfoIntoOrder(/*ApplicationUser*/ClaimsPrincipal user, OrderDTO order);

        BasketCheckoutDTO MapOrderToBasket(OrderDTO order);
    }
}
