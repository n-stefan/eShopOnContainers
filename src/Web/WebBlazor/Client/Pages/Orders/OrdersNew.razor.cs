using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebBlazor.Client.Extensions;
using WebBlazor.Client.Services;
using WebBlazor.Client.Services.ModelDTOs;
using WebBlazor.Client.Shared.Models;

namespace WebBlazor.Client.Pages.Orders
{
    [Authorize]
    public partial class OrdersNew
    {
        private bool errorReceived;
        private bool isOrderProcessing;
        private readonly List<HeaderInfo> header = new() { new() { Url = "basket", Text = "Back to cart" } };
        private OrderDTO order;
        private EditContext editContext;
        
        [Inject]
        private IOrderingService OrderingService { get; set; }

        [Inject]
        private IBasketService BasketService { get; set; }

        [Inject]
        private NavigationManager Navigation { get; set; }

        [CascadingParameter]
        private Task<AuthenticationState> AuthenticationStateTask { get; set; }

        private bool CantPlaceOrder
        {
            get => !editContext.Validate() || isOrderProcessing;
        }

        public OrdersNew()
        {
            order = new OrderDTO();
            editContext = new EditContext(order);
        }

        protected override async Task OnInitializedAsync()
        {
            var user = (await AuthenticationStateTask).User;
            var orderDraft = await BasketService.GetOrderDraft(user.GetSub());
            order = OrderingService.MapUserInfoIntoOrder(user, orderDraft);
            order.CardExpirationShortFormat();
            order.RequestId = Guid.NewGuid();
            editContext = new EditContext(order);
        }

        private async Task SubmitForm()
        {
            if (!editContext.Validate())
                return;
            var basket = OrderingService.MapOrderToBasket(order);
            try
            {
                await BasketService.Checkout(basket);
                errorReceived = false;
                isOrderProcessing = true;
                await Task.Delay(500);
                Navigation.NavigateTo("orders");
            }
            catch (Exception)
            {
                errorReceived = true;
                isOrderProcessing = false;
                throw;
            }
        }

        private string ErrorClass(Expression<Func<object>> field)
        {
            return editContext.GetValidationMessages(field).Any() ? "border border-danger" : string.Empty;
        }
    }
}
