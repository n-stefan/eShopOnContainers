using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebBlazor.Client.Extensions;
using WebBlazor.Client.Services;
using WebBlazor.Client.Services.ModelDTOs;
using WebBlazor.Client.Shared.Models;

namespace WebBlazor.Client.Pages.Orders
{
    [Authorize]
    public partial class Orders
    {
        private bool errorReceived;
        private List<OrderDTO> orders = new List<OrderDTO>();
        private List<HeaderInfo> header = new List<HeaderInfo> {
            new HeaderInfo { Url = "/catalog", Text = "Back to catalog" },
            new HeaderInfo { Text = "/" },
            new HeaderInfo { Url = "/ordermanagement", Text = "Orders Management" } };
        
        [Inject]
        private IOrderingService orderingService { get; set; }
        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var userId = (await authenticationStateTask).User.GetSub();
            await GetOrders(userId);
            //this.signalrService.msgReceived$
            //    .subscribe(x => this.getOrders());
        }

        private async Task GetOrders(string userId)
        {
            try
            {
                orders = await orderingService.GetMyOrders(userId);
                errorReceived = false;
            }
            catch (Exception)
            {
                errorReceived = true;
                throw;
            }
        }

        private async Task CancelOrder(string orderNumber)
        {
            await orderingService.CancelOrder(orderNumber);
        }
    }
}
