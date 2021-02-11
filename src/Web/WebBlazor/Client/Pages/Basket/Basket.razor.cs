using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBlazor.Client.Extensions;
using WebBlazor.Client.Services;
using WebBlazor.Client.Services.ModelDTOs;
using WebBlazor.Client.Shared.Models;

namespace WebBlazor.Client.Pages.Basket
{
    [Authorize]
    public partial class Basket
    {
        private bool errorUpdate;
        private BasketDTO basket = new BasketDTO();
        private string userId;
        private List<HeaderInfo> header = new List<HeaderInfo> { new HeaderInfo { Url = "/catalog", Text = "Back to catalog" } };
        
        [Inject]
        private IBasketService basketService { get; set; }
        [Inject]
        private NavigationManager navigation { get; set; }
        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }

        private bool HasItemWithInvalidQuantity
        {
            get => basket.Items.Any(x => x.Quantity < 1);
        }

        protected override async Task OnInitializedAsync()
        {
            userId = (await authenticationStateTask).User.GetSub();
            basket = await basketService.GetBasket(userId);
        }

        private void ItemQuantityChanged(BasketItemDTO item, ChangeEventArgs e)
        {
            item.Quantity = int.TryParse(e.Value.ToString(), out var result) ? result : 1;
            //if (item.Quantity < 1)
            //    return;
            //await BasketService.SetQuantities(userId, basket.Items.ToDictionary(x => x.Id, y => y.Quantity));
        }

        private async Task Update()
        {
            try
            {
                if (HasItemWithInvalidQuantity)
                    return;
                await basketService.UpdateBasket(basket);
                errorUpdate = false;
            }
            catch (Exception)
            {
                errorUpdate = true;
            }
        }

        private async Task CheckOut()
        {
            if (HasItemWithInvalidQuantity)
                return;
            await Update();
            navigation.NavigateTo("/ordersnew");
        }
    }
}
