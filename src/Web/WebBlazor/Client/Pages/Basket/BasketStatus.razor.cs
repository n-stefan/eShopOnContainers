using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Threading.Tasks;
using WebBlazor.Client.Extensions;
using WebBlazor.Client.Services;

namespace WebBlazor.Client.Pages.Basket
{
    [Authorize]
    public partial class BasketStatus : IDisposable
    {
        private int badge;
        
        [Inject]
        private IBasketService BasketService { get; set; }

        [Inject]
        private IEventService EventService { get; set; }

        [CascadingParameter]
        private Task<AuthenticationState> AuthenticationStateTask { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var userId = (await AuthenticationStateTask).User.GetSub();
            await UpdateBadge(userId);
            EventService.BasketItemAdded += UpdateBadge;
            EventService.OrderCreated += ResetBadge;
        }

        private async Task UpdateBadge(string userId)
        {
            var basket = await BasketService.GetBasket(userId);
            badge = basket.Items.Count;
            StateHasChanged();
        }

        private void ResetBadge()
        {
            badge = 0;
            StateHasChanged();
        }

        public void Dispose()
        {
            EventService.BasketItemAdded -= UpdateBadge;
            EventService.OrderCreated -= ResetBadge;
        }
    }
}
