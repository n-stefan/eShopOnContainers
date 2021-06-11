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
    public partial class OrdersDetail
    {
        private OrderDTO order = new();
        private readonly List<HeaderInfo> header = new() { new() { Url = "catalog", Text = "Back to catalog" } };
        
        [Inject]
        private IOrderingService OrderingService { get; set; }

        [Parameter]
        public int Id { get; set; }

        [CascadingParameter]
        private Task<AuthenticationState> AuthenticationStateTask { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var userId = (await AuthenticationStateTask).User.GetSub();
            await GetOrder(userId);
        }

        private async Task GetOrder(string userId) =>
            order = await OrderingService.GetOrder(userId, Id.ToString());
    }
}
