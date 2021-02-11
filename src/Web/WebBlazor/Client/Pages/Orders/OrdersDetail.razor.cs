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
        private OrderDTO order = new OrderDTO();
        private List<HeaderInfo> header = new List<HeaderInfo> { new HeaderInfo { Url = "/catalog", Text = "Back to catalog" } };
        
        [Inject]
        private IOrderingService orderingService { get; set; }
        [Parameter]
        public int Id { get; set; }
        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var userId = (await authenticationStateTask).User.GetSub();
            await GetOrder(userId);
        }

        private async Task GetOrder(string userId)
        {
            order = await orderingService.GetOrder(userId, Id.ToString());
        }
    }
}
