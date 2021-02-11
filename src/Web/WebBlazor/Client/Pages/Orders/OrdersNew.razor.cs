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
        private List<HeaderInfo> header = new List<HeaderInfo> { new HeaderInfo { Url = "/basket", Text = "Back to cart" } };
        private OrderDTO order;
        private EditContext editContext;
        
        [Inject]
        private IOrderingService orderingService { get; set; }
        [Inject]
        private IBasketService basketService { get; set; }
        [Inject]
        private NavigationManager navigation { get; set; }

        private bool CantPlaceOrder
        {
            get => !editContext.Validate() || isOrderProcessing;
        }

        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }

        public OrdersNew()
        {
            order = new OrderDTO();
            editContext = new EditContext(order);
        }

        protected override async Task OnInitializedAsync()
        {
            var user = (await authenticationStateTask).User;
            var orderDraft = await basketService.GetOrderDraft(user.GetSub());
            order = orderingService.MapUserInfoIntoOrder(user, orderDraft);
            order.CardExpirationShortFormat();
            order.RequestId = Guid.NewGuid();
            editContext = new EditContext(order);
        }

        private async Task SubmitForm()
        {
            if (!editContext.Validate())
                return;
            var basket = orderingService.MapOrderToBasket(order);
            try
            {
                await basketService.Checkout(basket);
                errorReceived = false;
                isOrderProcessing = true;
                await Task.Delay(1000);
                navigation.NavigateTo("/orders");
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
