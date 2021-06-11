using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebBlazor.Client.Extensions;
using WebBlazor.Client.Services;
using WebBlazor.Client.Services.ModelDTOs;
using WebBlazor.Client.Shared.Models;

namespace WebBlazor.Client.Pages.Orders
{
    [Authorize]
    public partial class OrdersManagement
    {
        private List<OrderDTO> orders = new();
        private string userId;
        private readonly List<HeaderInfo> header = new() { new() { Url = "catalog", Text = "Back to catalog" } };

        [Inject]
        private IOrderingService OrderingService { get; set; }

        [CascadingParameter]
        private Task<AuthenticationState> AuthenticationStateTask { get; set; }

        protected override async Task OnInitializedAsync()
        {
            userId = (await AuthenticationStateTask).User.GetSub();
            await LoadData();
        }

        private async Task LoadData() =>
            orders = await OrderingService.GetMyOrders(userId);

        private async Task OrderProcess(string orderId, ChangeEventArgs e)
        {
            if (OrderProcessActionDTO.Ship.Code == e.Value.ToString())
            {
                await OrderingService.ShipOrder(orderId);
                await LoadData();
            }
        }
    }
}
