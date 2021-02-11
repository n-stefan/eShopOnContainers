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
        private IBasketService basketService { get; set; }
        [Inject]
        private IEventService eventService { get; set; }
        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var userId = (await authenticationStateTask).User.GetSub();
            await UpdateBadge(userId);
            eventService.BasketItemAdded += UpdateBadge;
            eventService.OrderCreated += ResetBadge;
        }

        private async Task UpdateBadge(string userId)
        {
            var basket = await basketService.GetBasket(userId);
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
            eventService.BasketItemAdded -= UpdateBadge;
            eventService.OrderCreated -= ResetBadge;
        }
    }
}
